export interface Task {
  id: string;
  title: string;
  description: string;
  responsibleName: string;
  status: TaskStatus;
  createdAt: string;
}

export interface CreateTaskRequest {
  title: string;
  description: string;
  responsibleId: string;
}

export interface EditTaskRequest {
  title?: string;
  description?: string;
  responsibleId?: string;
  status?: TaskStatus;
}

export interface TaskFilter {
  status?: TaskStatus;
  responsibleId?: string;
  page?: number;
  pageSize?: number;
}

export interface PaginatedTaskResponse {
  data: Task[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
}

export enum TaskStatus {
  Pending = 'Pending',
  InProgress = 'InProgress',
  Completed = 'Completed'
}

export const TaskStatusLabels = {
  [TaskStatus.Pending]: 'Pendente',
  [TaskStatus.InProgress]: 'Em andamento',
  [TaskStatus.Completed]: 'Concluída'
};