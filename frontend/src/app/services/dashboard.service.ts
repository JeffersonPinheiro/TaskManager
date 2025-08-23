import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Dashboard } from '../models/dashboard.model';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  private apiUrl = 'http://localhost:5001/api/tasks/dashboard';

  constructor(private http: HttpClient) {}

  getDashboardData(): Observable<Dashboard> {
    return this.http.get<Dashboard>(this.apiUrl);
  }
}