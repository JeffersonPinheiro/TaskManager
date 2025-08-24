import { Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';
import { adminGuard } from './guards/admin.guard';
import { LayoutAuthenticatedComponent } from './components/layout-authenticated/layout-authenticated.component';

export const routes: Routes = [
  { path: 'login', loadComponent:() => import('./pages/login/login.component').then(m => m.LoginComponent) },
  {
    path: '',
    component: LayoutAuthenticatedComponent,
    canActivate: [authGuard],
    children: [{ 
        path: 'dashboard', 
        loadComponent:() => import('./pages/dashboard/dashboard.component').then(m => m.DashboardComponent),
      },
      { 
        path: 'tasks', 
        loadComponent:() => import('./pages/tasks/tasks.component').then(m => m.TasksComponent), 
      },
      { 
        path: 'users', 
        loadComponent:() => import('./pages/users/users.component').then(m => m.UsersComponent), 
        canActivate: [adminGuard] 
    }]
  },
  // { 
  //   path: 'dashboard', 
  //   loadComponent:() => import('./pages/dashboard/dashboard.component').then(m => m.DashboardComponent),
  //   canActivate: [authGuard] 
  // },
  // { 
  //   path: 'tasks', 
  //   loadComponent:() => import('./pages/tasks/tasks.component').then(m => m.TasksComponent), 
  //   canActivate: [authGuard] 
  // },
  // { 
  //   path: 'users', 
  //   loadComponent:() => import('./pages/users/users.component').then(m => m.UsersComponent), 
  //   canActivate: [authGuard, adminGuard] 
  // },
  { path: '**', redirectTo: '/dashboard' }
];
