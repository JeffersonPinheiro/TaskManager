import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from '../../services/user.service';
import { AuthService } from '../../services/auth.service';
import { User, CreateUserRequest, EditUserRequest } from '../../models/user.model';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './users.component.html',
})
export class UsersComponent implements OnInit {
  users: User[] = [];
  isLoading = true;
  showCreateForm = false;
  editingUser: User | null = null;
  
  userForm: FormGroup;
  
  userRoles = ['Admin', 'User'];

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private authService: AuthService,
    private router: Router
  ) {
    this.userForm = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(100)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      role: ['User', Validators.required]
    });
  }

  ngOnInit(): void {
    // Check if user is admin
    if (!this.authService.isAdmin()) {
      this.router.navigate(['/dashboard']);
      return;
    }
    
    this.loadUsers();
  }

  loadUsers(): void {
    this.isLoading = true;
    this.userService.getUsers().subscribe({
      next: (users) => {
        this.users = users;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading users:', error);
        this.isLoading = false;
      }
    });
  }

  openCreateForm(): void {
    this.showCreateForm = true;
    this.editingUser = null;
    this.userForm.reset({
      role: 'User'
    });
    
    // Enable password field for new users
    this.userForm.get('password')?.enable();
    this.userForm.get('password')?.setValidators([Validators.required, Validators.minLength(6)]);
  }

  openEditForm(user: User): void {
    this.editingUser = user;
    this.showCreateForm = true;
    
    this.userForm.patchValue({
      name: user.name,
      email: user.email,
      role: user.role
    });
    
    // Disable and clear password for editing
    this.userForm.get('password')?.disable();
    this.userForm.get('password')?.clearValidators();
    this.userForm.get('password')?.setValue('');
  }

  closeForm(): void {
    this.showCreateForm = false;
    this.editingUser = null;
    this.userForm.reset();
  }

  onSubmit(): void {
    if (this.userForm.valid) {
      if (this.editingUser) {
        this.updateUser();
      } else {
        this.createUser();
      }
    }
  }

  createUser(): void {
    const request: CreateUserRequest = this.userForm.value;
    
    this.userService.createUser(request).subscribe({
      next: () => {
        this.loadUsers();
        this.closeForm();
      },
      error: (error) => {
        console.error('Error creating user:', error);
        alert('Erro ao criar usuário. Verifique se o email já não está sendo usado.');
      }
    });
  }

  updateUser(): void {
    if (!this.editingUser) return;
    
    const request: EditUserRequest = {
      name: this.userForm.get('name')?.value,
      email: this.userForm.get('email')?.value,
      role: this.userForm.get('role')?.value
    };
    
    this.userService.updateUser(this.editingUser.id, request).subscribe({
      next: () => {
        this.loadUsers();
        this.closeForm();
      },
      error: (error) => {
        console.error('Error updating user:', error);
        alert('Erro ao atualizar usuário.');
      }
    });
  }

  get name() { return this.userForm.get('name'); }
  get email() { return this.userForm.get('email'); }
  get password() { return this.userForm.get('password'); }
  get role() { return this.userForm.get('role'); }
}