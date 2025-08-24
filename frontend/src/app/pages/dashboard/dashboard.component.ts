import { Component, OnInit, OnDestroy, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Chart, ChartConfiguration, ChartData, ChartType, registerables } from 'chart.js';
import { DashboardService } from '../../services/dashboard.service';
import { Dashboard } from '../../models/dashboard.model';
import { Task, TaskStatus, TaskStatusLabels } from '../../models/task.model';

Chart.register(...registerables);

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.component.html',
})
export class DashboardComponent implements OnInit, OnDestroy, AfterViewInit {
  dashboardData: Dashboard | null = null;
  isLoading = true;
  error: string | null = null;
  taskStatusLabels = TaskStatusLabels;
  chart: Chart | null = null;
  private dataLoaded = false;

  constructor(
    private dashboardService: DashboardService,
  ) {}

  ngOnInit(): void {
    this.loadDashboardData();
  }

  ngAfterViewInit(): void {
    // Chart will be created after data is loaded
  }

  ngOnDestroy(): void {
    if (this.chart) {
      this.chart.destroy();
    }
  }

  loadDashboardData(): void {
    this.isLoading = true;
    this.error = null;
    
    this.dashboardService.getDashboardData().subscribe({
      next: (data) => {
        this.dashboardData = data;
        this.isLoading = false;
        this.dataLoaded = true;
        // Use setTimeout to ensure the DOM is ready
        setTimeout(() => {
          this.createChart();
        }, 100);
      },
      error: (error) => {
        console.error('Error loading dashboard data:', error);
        this.error = error.error?.message || error.message || 'Erro ao carregar dados do dashboard';
        this.isLoading = false;
      }
    });
  }

  createChart(): void {
    // Chart creation skipped - no data or not loaded yet
    if (!this.dashboardData || !this.dataLoaded) {
      return;
    }

    const canvas = document.getElementById('tasksChart') as HTMLCanvasElement;
    if (!canvas) {
      console.error('Chart canvas not found');
      return;
    }

    // Destroy existing chart if it exists
    if (this.chart) {
      this.chart.destroy();
    }

    try {
      this.chart = new Chart(canvas, {
        type: 'doughnut' as ChartType,
        data: {
          labels: ['Pendentes', 'Em Andamento', 'Concluídas'],
          datasets: [{
            data: [
              this.dashboardData.pendingCount,
              this.dashboardData.inProgressCount,
              this.dashboardData.completedCount
            ],
            backgroundColor: [
              '#f59e0b', // yellow for pending
              '#3b82f6', // blue for in progress
              '#10b981'  // green for completed
            ],
            borderWidth: 2,
            borderColor: '#ffffff'
          }]
        },
        options: {
          responsive: true,
          maintainAspectRatio: false,
          plugins: {
            legend: {
              position: 'bottom'
            }
          }
        }
      });
    } catch (error) {
      console.error('Error creating chart:', error);
    }
  }

  updateChartData(): void {
    // This method is now handled by createChart
    this.createChart();
  }

  getStatusLabel(status: TaskStatus): string {
    return this.taskStatusLabels[status] || status;
  }
}