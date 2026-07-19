namespace Shared.DTO.Contracts.SignalRResponses
{
    public sealed record RealtimeMessage<TData>(
        bool Success,
        string Code,
        TData? Data = default,
        string? OperationId = null,
        DateTimeOffset? TimestampUtc = null
    )
    {
        public static RealtimeMessage<TData> Ok(
            string code,
            TData data,
            string? operationId = null)
        {
            return new RealtimeMessage<TData>(
                true,
                code,
                data,
                operationId,
                DateTimeOffset.UtcNow
            );
        }

        public static RealtimeMessage<TData> Fail(
            string code,
            TData? data = default,
            string? operationId = null)
        {
            return new RealtimeMessage<TData>(
                false,
                code,
                data,
                operationId,
                DateTimeOffset.UtcNow
            );
        }
    }
}