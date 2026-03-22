export interface LoginRequest {
  username: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  expiresAtUtc: string;
  refreshToken?: string;
  refreshTokenExpiresAtUtc?: string;
  uniqueId: string;
  email: string;
  fullName: string;
  mustChangePassword?: boolean;
}

export interface AdminLoginResponse {
  token: string;
  expiresAtUtc: string;
  uniqueId: string;
  email: string;
  fullName: string;
}

export interface RefreshRequest {
  refreshToken: string;
}

export interface ChangePasswordRequest {
  currentPassword: string;
  newPassword: string;
}
