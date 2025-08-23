import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { TasksComponent } from './components/tasks/tasks.component';
import { UsersComponent } from './components/users/users.component';
import { authGuard } from './guards/auth.guard';
import { adminGuard } from './guards/admin.guard';

export const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { 
    path: 'dashboard', 
    component: DashboardComponent, 
    canActivate: [authGuard] 
  },
  { 
    path: 'tasks', 
    component: TasksComponent, 
    canActivate: [authGuard] 
  },
  { 
    path: 'users', 
    component: UsersComponent, 
    canActivate: [authGuard, adminGuard] 
  },
  { path: '**', redirectTo: '/login' }
];
