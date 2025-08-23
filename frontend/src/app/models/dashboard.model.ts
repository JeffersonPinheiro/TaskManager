import { Task } from './task.model';

export interface Dashboard {
  pendingCount: number;
  inProgressCount: number;
  completedCount: number;
  lastCreatedTasks: Task[];
}