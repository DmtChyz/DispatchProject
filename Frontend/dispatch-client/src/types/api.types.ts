export interface ApiResponse<TData = never> {
  success: boolean;
  code: string;
  data?: TData;
}
export interface ApiResponse<TData = never> {
  success: boolean;
  code: string;
  data?: TData;
}

export interface ValidationFailureData {
  fields: Record<string, string>;
}