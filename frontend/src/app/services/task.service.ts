import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Task, CreateTaskRequest, EditTaskRequest, TaskFilter, PaginatedTaskResponse } from '../models/task.model';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private apiUrl = 'http://localhost:5001/api/tasks';

  constructor(private http: HttpClient) {}

  getTasks(filter?: TaskFilter): Observable<Task[]> {
    let params = new HttpParams();
    
    if (filter) {
      if (filter.status) {
        params = params.set('status', filter.status);
      }
      if (filter.responsibleId) {
        params = params.set('responsibleId', filter.responsibleId);
      }
      if (filter.page) {
        params = params.set('page', filter.page.toString());
      }
      if (filter.pageSize) {
        params = params.set('pageSize', filter.pageSize.toString());
      }
    }

    return this.http.get<Task[]>(this.apiUrl, { params });
  }

  getPaginatedTasks(filter?: TaskFilter): Observable<PaginatedTaskResponse> {
    let params = new HttpParams();
    
    if (filter) {
      if (filter.status) {
        params = params.set('status', filter.status);
      }
      if (filter.responsibleId) {
        params = params.set('responsibleId', filter.responsibleId);
      }
      if (filter.page) {
        params = params.set('page', filter.page.toString());
      }
      if (filter.pageSize) {
        params = params.set('pageSize', filter.pageSize.toString());
      }
    }

    return this.http.get<PaginatedTaskResponse>(`${this.apiUrl}/paginated`, { params });
  }

  getTask(id: string): Observable<Task> {
    return this.http.get<Task>(`${this.apiUrl}/${id}`);
  }

  createTask(task: CreateTaskRequest): Observable<Task> {
    return this.http.post<Task>(this.apiUrl, task);
  }

  updateTask(id: string, task: EditTaskRequest): Observable<Task> {
    return this.http.patch<Task>(`${this.apiUrl}/${id}`, task);
  }

  deleteTask(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}