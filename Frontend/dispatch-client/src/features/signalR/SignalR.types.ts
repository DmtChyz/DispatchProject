export type SignalRStatus =
  | "idle"
  | "connecting"
  | "connected"
  | "reconnecting"
  | "disconnected";

export const SIGNALR_ACTION_STATUS = {
  completed: 0,
  failed: 1,
} as const;

export type SignalRActionStatus =
  (typeof SIGNALR_ACTION_STATUS)[keyof typeof SIGNALR_ACTION_STATUS];

export const SIGNALR_ACTION_TYPE = {
  sendEmail: 1,
  sendSms: 2,
} as const;

export type SignalRActionType =
  (typeof SIGNALR_ACTION_TYPE)[keyof typeof SIGNALR_ACTION_TYPE];

export type NotificationStatusUpdate = {
  correlationId: string;
  ownerId: string;
  createdAt: string;
  status: SignalRActionStatus;
  actionType: SignalRActionType;
};

export type SignalRContextValue = {
  status: SignalRStatus;
  isConnected: boolean;
  latestUpdate: NotificationStatusUpdate | null;
};