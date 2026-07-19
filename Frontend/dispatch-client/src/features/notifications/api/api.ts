import { apiClient } from "../../../api/apiClient";
import type { ApiResponse } from "../../../types/api.types";
import type {
  DispatchOperationResponse,
  SendNotificationRequest,
} from "../notification.types";

export async function sendNotification(
  request: SendNotificationRequest
): Promise<ApiResponse<DispatchOperationResponse>> {
  const response = await apiClient.post<
    ApiResponse<DispatchOperationResponse>
  >("/dispatch", request);

  return response.data;
}