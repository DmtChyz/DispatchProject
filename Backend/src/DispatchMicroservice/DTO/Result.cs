public class Result<T>
{
    public bool Success { get; init; }
    public T? Value { get; init; }
    public string Errors { get; init; }


    public static Result<T> IsSuccess(T value)
    {
        return new Result<T> { Success = true, Value = value, Errors = String.Empty };
    }
    public static Result<T> IsFailure(String errors)
    {
        return new Result<T> { Success = false, Value = default(T), Errors = errors };
    }
}   