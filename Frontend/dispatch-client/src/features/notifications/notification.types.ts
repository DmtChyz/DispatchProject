export type NotificationActionType = "SendEmail" | "SendSms";

export type SendNotificationRequest = {
  actionType: NotificationActionType;
  recipient: string;
  details: string;
};

export type NotificationOperationStatus =
  | "queued"
  | "processing"
  | "sent"
  | "failed";

export type DispatchOperationResponse = {
  createdAtUtc: string;
  correlationId: string;
  status: NotificationOperationStatus;
};