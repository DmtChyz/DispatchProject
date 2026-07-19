namespace Shared.DTO.Contracts.ApiResponses
{

    /// <summary>
    /// Basic structure of the ApiResponse contract defined for this project
    /// </summary>
    /// <param name="Success"></param>
    /// <param name="Code"></param>
    public sealed record ApiResponse(
        bool Success,
        string Code
    ) : ResponseBase(Success, Code)
    {
        public static ApiResponse Ok(string code)
        {
            return new ApiResponse(true, code);
        }

        public static ApiResponse Fail(string code)
        {
            return new ApiResponse(false, code);
        }
    }

    public sealed record ApiResponse<TData>(
        bool Success,
        string Code,
        TData? Data = default
    ) : ResponseBase(Success, Code)
    {
        public static ApiResponse<TData> Ok(string code, TData data)
        {
            return new ApiResponse<TData>(true, code, data);
        }

        public static ApiResponse<TData> Fail(string code, TData? data = default)
        {
            return new ApiResponse<TData>(false, code, data);
        }
    }
}