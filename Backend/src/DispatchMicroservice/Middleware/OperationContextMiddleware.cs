namespace DispatchMicroservice.Middleware
{
    public class OperationContextMiddleware
    {
        public const string HeaderName = "X-Operation-Id";
        public const string ItemKey = "__OperationId";

        private readonly RequestDelegate _next;

        public OperationContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var operationId = Guid.NewGuid().ToString("N");

            context.Items[ItemKey] = operationId; // internal http storage

            context.Response.OnStarting(() => // visible http header
            {
                context.Response.Headers[HeaderName] = operationId;
                return Task.CompletedTask;
            });

            await _next(context);
        }

        public static string GetOperationId(HttpContext context)
        {
            if (context.Items.TryGetValue(ItemKey, out var value)
                && value is string operationId
                && !string.IsNullOrWhiteSpace(operationId))
            {
                return operationId;
            }

            var newOperationId = Guid.NewGuid().ToString("N");
            context.Items[ItemKey] = newOperationId;

            return newOperationId;
        }
    }
}