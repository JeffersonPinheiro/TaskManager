import { Component } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-layout-authenticated',
  imports: [RouterOutlet, CommonModule],
  templateUrl: './layout-authenticated.component.html',
})
export class LayoutAuthenticatedComponent {
  constructor(
    private authService: AuthService,
    private router: Router
  ){}

  navigateToTasks(): void {
    this.router.navigate(['/tasks']);
  }

  navigateToUsers(): void {
    if(!this.authService.isAdmin()) return;

    this.router.navigate(['/users']);
  }
  
  navigateToDashboard(): void{
    this.router.navigate(['/dashboard']);
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
}
