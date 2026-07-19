export interface AuthUser {
  userId: string;
  userName: string;
}

export interface AuthResponse {
  user: AuthUser;
}

export interface AuthCredentials {
  userName: string;
  password: string;
}

export type AuthMode = "login" | "register";

export interface AuthUser {
  userId: string;
  userName: string;
}

export interface AuthResponse {
  user: AuthUser;
}

export interface AuthCredentials {
  userName: string;
  password: string;
}