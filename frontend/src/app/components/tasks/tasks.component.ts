import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { TaskService } from '../../services/task.service';
import { UserService } from '../../services/user.service';
import { AuthService } from '../../services/auth.service';
import { Task, CreateTaskRequest, EditTaskRequest, TaskStatus, TaskFilter, TaskStatusLabels } from '../../models/task.model';
import { User } from '../../models/user.model';

@Component({
  selector: 'app-tasks',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './tasks.component.html',
  styleUrls: ['./tasks.component.css']
})
export class TasksComponent implements OnInit {
  tasks: Task[] = [];
  users: User[] = [];
  isLoading = true;
  showCreateForm = false;
  editingTask: Task | null = null;
  
  // Pagination properties
  currentPage = 1;
  pageSize = 10;
  totalPages = 1;
  totalItems = 0;
  pageSizeOptions = [5, 10, 20, 50];
  
  taskForm: FormGroup;
  filterForm: FormGroup;
  
  taskStatuses = Object.values(TaskStatus);
  taskStatusLabels = TaskStatusLabels;

  constructor(
    private fb: FormBuilder,
    private taskService: TaskService,
    private userService: UserService,
    private authService: AuthService,
    private router: Router
  ) {
    this.taskForm = this.fb.group({
      title: ['', [Validators.required, Validators.maxLength(100)]],
      description: ['', [Validators.required, Validators.maxLength(500)]],
      responsibleId: [''], // Will be set conditionally based on user role
      status: [TaskStatus.Pending, Validators.required]
    });

    this.filterForm = this.fb.group({
      status: [''],
      responsibleId: [''],
      pageSize: [this.pageSize]
    });
  }

  ngOnInit(): void {
    this.loadUsers();
    this.loadTasks();
  }

  loadUsers(): void {
    if (this.authService.isAdmin()) {
      this.userService.getUsers().subscribe({
        next: (users) => {
          this.users = users;
        },
        error: (error) => {
          console.error('Error loading users:', error);
        }
      });
    } else {
      // For normal users, load just their own user info
      const currentUser = this.authService.getCurrentUser();
      if (currentUser) {
        this.users = [currentUser];
      }
    }
  }

  loadTasks(): void {
    this.isLoading = true;
    // Load tasks without any filters for initial load
    const filter: TaskFilter = {
      page: this.currentPage,
      pageSize: this.pageSize
    };

    this.taskService.getTasks(filter).subscribe({
      next: (tasks) => {
        this.tasks = tasks;
        this.isLoading = false;
        this.updatePaginationInfo(tasks.length);
      },
      error: (error) => {
        console.error('Error loading tasks:', error);
        this.isLoading = false;
      }
    });
  }

  applyFilters(): void {
    this.isLoading = true;
    const filter: TaskFilter = {
      page: this.currentPage,
      pageSize: this.pageSize
    };
    const formValue = this.filterForm.value;
    
    if (formValue.status) {
      filter.status = formValue.status;
    }
    if (formValue.responsibleId) {
      filter.responsibleId = formValue.responsibleId;
    }

    this.taskService.getTasks(filter).subscribe({
      next: (tasks) => {
        this.tasks = tasks;
        this.isLoading = false;
        this.updatePaginationInfo(tasks.length);
      },
      error: (error) => {
        console.error('Error applying filters:', error);
        this.isLoading = false;
      }
    });
  }

  updatePaginationInfo(returnedTasksCount: number): void {
    // If we got less tasks than the page size, we're on the last page
    if (returnedTasksCount < this.pageSize) {
      this.totalPages = this.currentPage;
    } else {
      // Estimate that there might be more pages
      this.totalPages = this.currentPage + 1;
    }
    
    // If we're not on the first page and got no results, go back one page
    if (returnedTasksCount === 0 && this.currentPage > 1) {
      this.currentPage--;
      this.applyFilters(); // Use applyFilters to maintain current filters
    }
  }

  openCreateForm(): void {
    this.showCreateForm = true;
    this.editingTask = null;
    
    // Set validation and default values based on user role
    if (this.authService.isAdmin()) {
      this.taskForm.get('responsibleId')?.setValidators([Validators.required]);
      this.taskForm.reset({
        status: TaskStatus.Pending
      });
    } else {
      // For normal users, remove responsibleId validation and set it to current user
      this.taskForm.get('responsibleId')?.clearValidators();
      const currentUser = this.authService.getCurrentUser();
      this.taskForm.reset({
        status: TaskStatus.Pending,
        responsibleId: currentUser?.id || ''
      });
    }
    this.taskForm.get('responsibleId')?.updateValueAndValidity();
  }

  openEditForm(task: Task): void {
    this.editingTask = task;
    this.showCreateForm = true;
    
    // Find responsible user ID
    const responsibleUser = this.users.find(u => u.name === task.responsibleName);
    
    // Set validation based on user role
    if (this.authService.isAdmin()) {
      this.taskForm.get('responsibleId')?.setValidators([Validators.required]);
    } else {
      this.taskForm.get('responsibleId')?.clearValidators();
    }
    this.taskForm.get('responsibleId')?.updateValueAndValidity();
    
    this.taskForm.patchValue({
      title: task.title,
      description: task.description,
      responsibleId: responsibleUser?.id || '',
      status: task.status
    });
  }

  closeForm(): void {
    this.showCreateForm = false;
    this.editingTask = null;
    this.taskForm.reset();
  }

  onSubmit(): void {
    if (this.taskForm.valid) {
      if (this.editingTask) {
        this.updateTask();
      } else {
        this.createTask();
      }
    }
  }

  createTask(): void {
    const formValue = this.taskForm.value;
    
    // For normal users, ensure they are assigned as the responsible user
    let responsibleId = formValue.responsibleId;
    if (!this.authService.isAdmin()) {
      const currentUser = this.authService.getCurrentUser();
      responsibleId = currentUser?.id || '';
    }
    
    const request: CreateTaskRequest = {
      title: formValue.title,
      description: formValue.description,
      responsibleId: responsibleId
    };
    
    this.taskService.createTask(request).subscribe({
      next: () => {
        this.currentPage = 1; // Reset to first page after creating
        this.applyFilters(); // Apply current filters
        this.closeForm();
      },
      error: (error) => {
        console.error('Error creating task:', error);
      }
    });
  }

  updateTask(): void {
    if (!this.editingTask) return;
    
    const request: EditTaskRequest = this.taskForm.value;
    
    this.taskService.updateTask(this.editingTask.id, request).subscribe({
      next: () => {
        this.applyFilters(); // Keep current page and filters after updating
        this.closeForm();
      },
      error: (error) => {
        console.error('Error updating task:', error);
      }
    });
  }

  deleteTask(task: Task): void {
    if (confirm(`Tem certeza que deseja excluir a tarefa "${task.title}"?`)) {
      this.taskService.deleteTask(task.id).subscribe({
        next: () => {
          this.applyFilters(); // Keep current page and filters after deleting
        },
        error: (error) => {
          console.error('Error deleting task:', error);
        }
      });
    }
  }

  navigateToDashboard(): void {
    this.router.navigate(['/dashboard']);
  }

  navigateToUsers(): void {
    this.router.navigate(['/users']);
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  get currentUser() {
    return this.authService.getCurrentUser();
  }

  get isAdmin() {
    return this.authService.isAdmin();
  }

  get title() { return this.taskForm.get('title'); }
  get description() { return this.taskForm.get('description'); }
  get responsibleId() { return this.taskForm.get('responsibleId'); }
  get status() { return this.taskForm.get('status'); }

  getStatusLabel(status: TaskStatus): string {
    return this.taskStatusLabels[status];
  }

  // Pagination methods
  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages && page !== this.currentPage) {
      this.currentPage = page;
      this.applyFilters(); // Use applyFilters to maintain current filters
    }
  }

  goToFirstPage(): void {
    this.goToPage(1);
  }

  goToPreviousPage(): void {
    this.goToPage(this.currentPage - 1);
  }

  goToNextPage(): void {
    this.goToPage(this.currentPage + 1);
  }

  goToLastPage(): void {
    this.goToPage(this.totalPages);
  }

  onPageSizeChange(): void {
    const newPageSize = this.filterForm.get('pageSize')?.value;
    if (newPageSize && newPageSize !== this.pageSize) {
      this.pageSize = newPageSize;
      this.currentPage = 1; // Reset to first page when changing page size
      this.applyFilters(); // Apply current filters with new page size
    }
  }

  onStatusFilterChange(): void {
    this.currentPage = 1; // Reset to first page when status filter changes
    this.applyFilters();
  }

  onResponsibleFilterChange(): void {
    this.currentPage = 1; // Reset to first page when responsible filter changes
    this.applyFilters();
  }

  getPageNumbers(): number[] {
    const pages: number[] = [];
    const maxPagesToShow = 5;
    const halfRange = Math.floor(maxPagesToShow / 2);
    
    let startPage = Math.max(1, this.currentPage - halfRange);
    let endPage = Math.min(this.totalPages, startPage + maxPagesToShow - 1);
    
    // Adjust start page if we're near the end
    if (endPage - startPage < maxPagesToShow - 1) {
      startPage = Math.max(1, endPage - maxPagesToShow + 1);
    }
    
    for (let i = startPage; i <= endPage; i++) {
      pages.push(i);
    }
    
    return pages;
  }
}