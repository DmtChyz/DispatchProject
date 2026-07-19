import type { TranslationKey } from "./en";

export const uk = {
  // Common
  "common.submit": "Підтвердити",
  "common.save": "Зберегти",
  "common.cancel": "Скасувати",
  "common.close": "Закрити",
  "common.retry": "Спробувати знову",
  "common.loading": "Завантаження...",

  // Navigation
  "navigation.dashboard": "Панель керування",
  "navigation.history": "Історія",
  "navigation.login": "Увійти",
  "navigation.register": "Створити акаунт",
  "navigation.logout": "Вийти",

  // Authentication UI
  "auth.login.title": "З поверненням",
  "auth.login.description":
    "Увійдіть, щоб надсилати сповіщення та відстежувати їхній статус.",
  "auth.login.button": "Увійти",
  "auth.login.loading": "Вхід...",

  "auth.register.title": "Створіть свій акаунт",
  "auth.register.description":
    "Зареєструйтеся, щоб надсилати сповіщення та відстежувати їхній статус.",
  "auth.register.button": "Створити акаунт",
  "auth.register.loading": "Створення акаунта...",

  "auth.username.label": "Ім’я користувача",
  "auth.username.placeholder": "Введіть ім’я користувача",

  "auth.password.label": "Пароль",
  "auth.password.placeholder": "Введіть пароль",

  // Language
  "language.label": "Мова",
  "language.english": "Англійська",
  "language.ukrainian": "Українська",
  "language.change": "Змінити мову",

  // Theme
  "theme.label": "Тема",
  "theme.dark": "Темна",
  "theme.light": "Світла",
  "theme.switch_to_dark": "Увімкнути темну тему",
  "theme.switch_to_light": "Увімкнути світлу тему",

  // Home page
  "home.eyebrow": "Система сповіщень",
  "home.title":
    "Надсилайте сповіщення. Відстежуйте кожен результат.",
  "home.description":
    "Розподілена система для надсилання повідомлень електронною поштою та SMS із відстеженням статусу в реальному часі.",

  "home.get_started": "Розпочати",
  "home.open_dashboard": "Відкрити панель",

  "home.preview.eyebrow": "Огляд системи",
  "home.preview.title": "Канали доставки",
  "home.preview.live": "Онлайн",

  "home.preview.email": "Електронна пошта",
  "home.preview.email_description":
    "Надсилайте сповіщення через сервіс електронної пошти.",

  "home.preview.sms": "SMS",
  "home.preview.sms_description":
    "Доставляйте термінові повідомлення безпосередньо на телефони.",

  "home.preview.tracking": "Відстеження в реальному часі",
  "home.preview.tracking_description":
    "Отримуйте оновлення статусу сповіщень через SignalR.",

  "home.preview.ready": "Готово",
  "home.preview.connected": "Підключено",

  "home.features.eyebrow": "Як це працює",
  "home.features.title":
    "Одна система для повного циклу сповіщень",
  "home.features.description":
    "Створіть сповіщення, надішліть його вибраним каналом і відстежуйте кожну операцію в одному інтерфейсі.",

  "home.features.send.title": "Надсилання сповіщень",
  "home.features.send.description":
    "Виберіть електронну пошту або SMS, введіть одержувача й повідомлення та додайте сповіщення до черги.",

  "home.features.track.title": "Відстеження доставки",
  "home.features.track.description":
    "Стежте за статусами: у черзі, обробляється, надіслано або помилка — у реальному часі.",

  "home.features.history.title": "Історія операцій",
  "home.features.history.description":
    "Переглядайте попередні операції, ідентифікатори кореляції, дати та результати доставки.",

  "home.cta.title": "Готові надіслати перше сповіщення?",
  "home.cta.description":
    "Створіть акаунт або відкрийте панель, щоб надсилати й відстежувати повідомлення.",

  // Dashboard
  "dashboard.eyebrow": "Система сповіщень",
  "dashboard.title": "Панель керування",
  "dashboard.description":
    "Надсилайте сповіщення та відстежуйте їхню доставку в реальному часі.",

  "dashboard.connection.title":
    "Підключення в реальному часі",
  "dashboard.connection.description":
    "SignalR-підключення для отримання оновлень статусу сповіщень.",
  "dashboard.connection.ready":
    "Система готова отримувати оновлення в реальному часі.",
  "dashboard.connection.unavailable":
    "Оновлення в реальному часі наразі недоступні.",

  "dashboard.latest_operation.title": "Остання операція",
  "dashboard.latest_operation.description":
    "Останній прийнятий запит на надсилання сповіщення.",
  "dashboard.latest_operation.empty":
    "Надішліть сповіщення, щоб побачити тут його поточний статус.",
  "dashboard.latest_operation.status": "Статус",

  // Notification form
  "notification_form.title": "Надіслати сповіщення",
  "notification_form.description":
    "Виберіть канал, введіть одержувача та підготуйте повідомлення.",

  "notification_form.channel.label": "Канал доставки",
  "notification_form.channel.email": "Електронна пошта",
  "notification_form.channel.sms": "SMS",

  "notification_form.recipient.label": "Одержувач",
  "notification_form.recipient.placeholder":
    "Введіть електронну адресу або номер телефону",

  "notification_form.details.label": "Повідомлення",
  "notification_form.details.placeholder":
    "Введіть текст сповіщення",

  "notification_form.result.correlation_id":
    "Ідентифікатор кореляції",
  "notification_form.result.created_at": "Створено",

  "notification_form.submit": "Надіслати сповіщення",
  "notification_form.submitting": "Надсилання...",

  // Notification operation statuses
  "notification_status.queued": "У черзі",
  "notification_status.processing": "Обробляється",
  "notification_status.sent": "Надіслано",
  "notification_status.failed": "Помилка",

  // SignalR
  "signalr.connected":
    "Підключено до системи. Готово до отримання оновлень статусу.",

  "signalr.status.connected": "Підключено",
  "signalr.status.connecting": "Підключення",
  "signalr.status.reconnecting": "Повторне підключення",
  "signalr.status.disconnected": "Відключено",

  // Authentication API responses
  "auth.login.success": "Вхід виконано успішно.",
  "auth.register.success": "Акаунт успішно створено.",
  "auth.logout.success": "Вихід виконано успішно.",
  "auth.current_user.success":
    "Дані поточного користувача успішно завантажено.",
  "auth.invalid_credentials":
    "Неправильне ім’я користувача або пароль.",
  "auth.unauthorized": "Для продовження необхідно увійти.",
  "auth.forbidden":
    "У вас немає дозволу на виконання цієї дії.",

  // Notification API responses
  "notification.queued": "Сповіщення додано до черги.",
  "notification.processing": "Сповіщення обробляється.",
  "notification.sent": "Сповіщення успішно надіслано.",
  "notification.failed": "Не вдалося надіслати сповіщення.",
  "notification.queue_failed":
    "Не вдалося додати сповіщення до черги.",

  // HTTP responses
  "http.bad_request": "Запит містить неправильні дані.",
  "http.not_found": "Запитаний ресурс не знайдено.",
  "http.method_not_allowed":
    "Цей метод запиту не підтримується.",
  "http.unsupported_media_type":
    "Тип вмісту запиту не підтримується.",

  // System responses
  "system.unexpected_error":
    "Щось пішло не так. Спробуйте ще раз пізніше.",

  // Validation responses
  "validation.invalid_request":
    "Деякі введені значення є неправильними.",

  "validation.username.required":
    "Ім’я користувача є обов’язковим.",
  "validation.username.invalid":
    "Ім’я користувача має неправильний формат.",
  "validation.username.too_short":
    "Ім’я користувача занадто коротке.",
  "validation.username.too_long":
    "Ім’я користувача занадто довге.",
  "validation.username.already_exists":
    "Це ім’я користувача вже зайняте.",

  "validation.password.required": "Пароль є обов’язковим.",
  "validation.password.too_short": "Пароль занадто короткий.",
  "validation.password.too_long": "Пароль занадто довгий.",
  "validation.password.weak": "Пароль занадто слабкий.",

  "validation.details.required":
    "Текст повідомлення є обов’язковим.",
  "validation.details.too_short":
    "Текст повідомлення занадто короткий.",
  "validation.details.too_long":
    "Текст повідомлення занадто довгий.",

  "validation.action_type.required":
    "Тип сповіщення є обов’язковим.",
  "validation.action_type.invalid":
    "Вибраний тип сповіщення є неправильним.",

  "validation.recipient.required":
    "Одержувач є обов’язковим.",
  "validation.recipient.invalid":
    "Введіть правильну електронну адресу або номер телефону.",

  "validation.identity.failed":
    "Не вдалося обробити дані акаунта.",
} satisfies Readonly<Record<TranslationKey, string>>;