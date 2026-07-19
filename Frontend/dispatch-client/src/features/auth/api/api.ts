import { apiClient } from "../../../api/apiClient";
import type { ApiResponse } from "../../../types/api.types";
import type {
  AuthCredentials,
  AuthResponse,
} from "../../../types/auth.types";

const USER_API_ROUTE = "/user";

export const authApi = {
  async getCurrentUser(): Promise<ApiResponse<AuthResponse>> {
    const response = await apiClient.get<ApiResponse<AuthResponse>>(
      `${USER_API_ROUTE}/me`
    );

    return response.data;
  },

  async login(
    credentials: AuthCredentials
  ): Promise<ApiResponse<AuthResponse>> {
    const response = await apiClient.post<ApiResponse<AuthResponse>>(
      `${USER_API_ROUTE}/login`,
      credentials
    );

    return response.data;
  },

  async register(
    credentials: AuthCredentials
  ): Promise<ApiResponse<AuthResponse>> {
    const response = await apiClient.post<ApiResponse<AuthResponse>>(
      `${USER_API_ROUTE}/register`,
      credentials
    );

    return response.data;
  },

  async logout(): Promise<ApiResponse> {
    const response = await apiClient.post<ApiResponse>(
      `${USER_API_ROUTE}/logout`
    );

    return response.data;
  },
};