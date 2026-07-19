export const TRANSLATION_KEYS = {
  common: {
    submit: "common.submit",
    save: "common.save",
    cancel: "common.cancel",
    close: "common.close",
    retry: "common.retry",
    loading: "common.loading",
  },

  navigation: {
    dashboard: "navigation.dashboard",
    history: "navigation.history",
    login: "navigation.login",
    register: "navigation.register",
    logout: "navigation.logout",
  },

  auth: {
    loginTitle: "auth.login.title",
    loginDescription: "auth.login.description",
    loginButton: "auth.login.button",
    loginLoading: "auth.login.loading",

    registerTitle: "auth.register.title",
    registerDescription: "auth.register.description",
    registerButton: "auth.register.button",
    registerLoading: "auth.register.loading",

    usernameLabel: "auth.username.label",
    usernamePlaceholder: "auth.username.placeholder",

    passwordLabel: "auth.password.label",
    passwordPlaceholder: "auth.password.placeholder",
  },

  language: {
    label: "language.label",
    english: "language.english",
    ukrainian: "language.ukrainian",
    change: "language.change",
  },

  theme: {
    label: "theme.label",
    dark: "theme.dark",
    light: "theme.light",
    switchToDark: "theme.switch_to_dark",
    switchToLight: "theme.switch_to_light",
  },

  home: {
    eyebrow: "home.eyebrow",
    title: "home.title",
    description: "home.description",

    getStarted: "home.get_started",
    openDashboard: "home.open_dashboard",

    previewEyebrow: "home.preview.eyebrow",
    previewTitle: "home.preview.title",
    live: "home.preview.live",

    email: "home.preview.email",
    emailDescription: "home.preview.email_description",

    sms: "home.preview.sms",
    smsDescription: "home.preview.sms_description",

    tracking: "home.preview.tracking",
    trackingDescription: "home.preview.tracking_description",

    ready: "home.preview.ready",
    connected: "home.preview.connected",

    featuresEyebrow: "home.features.eyebrow",
    featuresTitle: "home.features.title",
    featuresDescription: "home.features.description",

    sendTitle: "home.features.send.title",
    sendDescription: "home.features.send.description",

    trackTitle: "home.features.track.title",
    trackDescription: "home.features.track.description",

    historyTitle: "home.features.history.title",
    historyDescription: "home.features.history.description",

    ctaTitle: "home.cta.title",
    ctaDescription: "home.cta.description",
  },

  dashboard: {
    eyebrow: "dashboard.eyebrow",
    title: "dashboard.title",
    description: "dashboard.description",

    connectionTitle: "dashboard.connection.title",
    connectionDescription: "dashboard.connection.description",
    connectionReady: "dashboard.connection.ready",
    connectionUnavailable: "dashboard.connection.unavailable",

    latestOperationTitle: "dashboard.latest_operation.title",
    latestOperationDescription:
      "dashboard.latest_operation.description",
    latestOperationEmpty: "dashboard.latest_operation.empty",
    latestOperationStatus: "dashboard.latest_operation.status",
  },

  notificationForm: {
    title: "notification_form.title",
    description: "notification_form.description",

    channelLabel: "notification_form.channel.label",
    email: "notification_form.channel.email",
    sms: "notification_form.channel.sms",

    recipientLabel: "notification_form.recipient.label",
    recipientPlaceholder:
      "notification_form.recipient.placeholder",

    detailsLabel: "notification_form.details.label",
    detailsPlaceholder:
      "notification_form.details.placeholder",

    correlationId:
      "notification_form.result.correlation_id",
    createdAt: "notification_form.result.created_at",

    submit: "notification_form.submit",
    submitting: "notification_form.submitting",
  },

  notificationStatus: {
    queued: "notification_status.queued",
    processing: "notification_status.processing",
    sent: "notification_status.sent",
    failed: "notification_status.failed",
  },

  signalR: {
    connected: "signalr.connected",

    statusConnected: "signalr.status.connected",
    statusConnecting: "signalr.status.connecting",
    statusReconnecting: "signalr.status.reconnecting",
    statusDisconnected: "signalr.status.disconnected",
  },
  
} as const;