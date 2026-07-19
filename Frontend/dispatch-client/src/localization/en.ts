export const en = {
  // Common
  "common.submit": "Submit",
  "common.save": "Save",
  "common.cancel": "Cancel",
  "common.close": "Close",
  "common.retry": "Retry",
  "common.loading": "Loading...",

  // Navigation
  "navigation.dashboard": "Dashboard",
  "navigation.history": "History",
  "navigation.login": "Sign in",
  "navigation.register": "Create account",
  "navigation.logout": "Log out",

  // Authentication UI
  "auth.login.title": "Welcome back",
  "auth.login.description":
    "Sign in to send notifications and track their status.",
  "auth.login.button": "Sign in",
  "auth.login.loading": "Signing in...",

  "auth.register.title": "Create your account",
  "auth.register.description":
    "Register to start sending and tracking notifications.",
  "auth.register.button": "Create account",
  "auth.register.loading": "Creating account...",

  "auth.username.label": "Username",
  "auth.username.placeholder": "Enter your username",

  "auth.password.label": "Password",
  "auth.password.placeholder": "Enter your password",

  // Language
  "language.label": "Language",
  "language.english": "English",
  "language.ukrainian": "Ukrainian",
  "language.change": "Change language",

  // Theme
  "theme.label": "Theme",
  "theme.dark": "Dark",
  "theme.light": "Light",
  "theme.switch_to_dark": "Switch to dark theme",
  "theme.switch_to_light": "Switch to light theme",

  // Home page
  "home.eyebrow": "Dispatch system",
  "home.title": "Send notifications. Track every result.",
  "home.description":
    "A distributed notification system for sending email and SMS messages while tracking their status in real time.",

  "home.get_started": "Get started",
  "home.open_dashboard": "Open dashboard",

  "home.preview.eyebrow": "System overview",
  "home.preview.title": "Delivery channels",
  "home.preview.live": "Live",

  "home.preview.email": "Email",
  "home.preview.email_description":
    "Send email notifications through the email service.",

  "home.preview.sms": "SMS",
  "home.preview.sms_description":
    "Deliver time-sensitive messages directly to phones.",

  "home.preview.tracking": "Real-time tracking",
  "home.preview.tracking_description":
    "Receive notification status updates through SignalR.",

  "home.preview.ready": "Ready",
  "home.preview.connected": "Connected",

  "home.features.eyebrow": "How it works",
  "home.features.title":
    "One system for the complete notification flow",
  "home.features.description":
    "Create a notification, send it through the selected channel, and track every operation from one interface.",

  "home.features.send.title": "Send notifications",
  "home.features.send.description":
    "Choose email or SMS, enter the recipient and message, and queue the notification for delivery.",

  "home.features.track.title": "Track delivery status",
  "home.features.track.description":
    "Follow queued, processing, sent, and failed statuses through real-time updates.",

  "home.features.history.title": "Review operation history",
  "home.features.history.description":
    "Inspect previous notification operations, correlation identifiers, dates, and delivery results.",

  "home.cta.title": "Ready to send your first notification?",
  "home.cta.description":
    "Create an account or open the dashboard to start sending and tracking messages.",

  // Dashboard
  "dashboard.eyebrow": "Dispatch system",
  "dashboard.title": "Dashboard",
  "dashboard.description":
    "Send notifications and monitor their delivery status in real time.",

  "dashboard.connection.title": "Live connection",
  "dashboard.connection.description":
    "SignalR connection used to receive notification status updates.",
  "dashboard.connection.ready":
    "The system is ready to receive real-time updates.",
  "dashboard.connection.unavailable":
    "Real-time updates are currently unavailable.",

  "dashboard.latest_operation.title": "Latest operation",
  "dashboard.latest_operation.description":
    "The most recently accepted notification request.",
  "dashboard.latest_operation.empty":
    "Send a notification to see its live operation status here.",
  "dashboard.latest_operation.status": "Status",

  // Notification form
  "notification_form.title": "Send notification",
  "notification_form.description":
    "Select a channel, enter the recipient, and prepare your message.",

  "notification_form.channel.label": "Delivery channel",
  "notification_form.channel.email": "Email",
  "notification_form.channel.sms": "SMS",

  "notification_form.recipient.label": "Recipient",
  "notification_form.recipient.placeholder":
    "Enter an email address or phone number",

  "notification_form.details.label": "Message",
  "notification_form.details.placeholder":
    "Enter notification details",

  "notification_form.result.correlation_id": "Correlation ID",
  "notification_form.result.created_at": "Created at",

  "notification_form.submit": "Send notification",
  "notification_form.submitting": "Sending...",

  // Notification operation statuses
  "notification_status.queued": "Queued",
  "notification_status.processing": "Processing",
  "notification_status.sent": "Sent",
  "notification_status.failed": "Failed",

  // SignalR
  "signalr.connected":
    "Connected to the system. Ready to receive live status updates.",

  "signalr.status.connected": "Connected",
  "signalr.status.connecting": "Connecting",
  "signalr.status.reconnecting": "Reconnecting",
  "signalr.status.disconnected": "Disconnected",

  // Authentication API responses
  "auth.login.success": "Signed in successfully.",
  "auth.register.success": "Account created successfully.",
  "auth.logout.success": "Logged out successfully.",
  "auth.current_user.success": "Current user loaded successfully.",
  "auth.invalid_credentials": "Invalid username or password.",
  "auth.unauthorized": "You must sign in to continue.",
  "auth.forbidden":
    "You do not have permission to perform this action.",

  // Notification API responses
  "notification.queued":
    "The notification was added to the queue.",
  "notification.processing":
    "The notification is being processed.",
  "notification.sent":
    "The notification was sent successfully.",
  "notification.failed":
    "The notification could not be sent.",
  "notification.queue_failed":
    "The notification could not be added to the queue.",

  // HTTP responses
  "http.bad_request": "The request is invalid.",
  "http.not_found": "The requested resource was not found.",
  "http.method_not_allowed":
    "This request method is not supported.",
  "http.unsupported_media_type":
    "The request content type is not supported.",

  // System responses
  "system.unexpected_error":
    "Something went wrong. Please try again later.",

  // Validation responses
  "validation.invalid_request":
    "Some of the provided values are invalid.",

  "validation.username.required": "Username is required.",
  "validation.username.invalid": "Username is invalid.",
  "validation.username.too_short": "Username is too short.",
  "validation.username.too_long": "Username is too long.",
  "validation.username.already_exists":
    "This username is already in use.",

  "validation.password.required": "Password is required.",
  "validation.password.too_short": "Password is too short.",
  "validation.password.too_long": "Password is too long.",
  "validation.password.weak": "Password is too weak.",

  "validation.details.required": "Message details are required.",
  "validation.details.too_short":
    "Message details are too short.",
  "validation.details.too_long":
    "Message details are too long.",

  "validation.action_type.required":
    "Notification type is required.",
  "validation.action_type.invalid":
    "The selected notification type is invalid.",

  "validation.recipient.required": "Recipient is required.",
  "validation.recipient.invalid":
    "Enter a valid email address or phone number.",

  "validation.identity.failed":
    "The account could not be processed.",
} as const;

export type TranslationKey = keyof typeof en;