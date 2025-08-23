export interface User {
  id: string;
  name: string;
  email: string;
  role: string;
}

export interface CreateUserRequest {
  name: string;
  email: string;
  password: string;
  role: string;
}

export interface EditUserRequest {
  name?: string;
  email?: string;
  password?: string;
  role?: string;
}