using Shared.Constants;

namespace Shared.DTO.Contracts.ApiResponses
{
    public static class ApiResponseFactory
    {
        public static ApiResponse<ValidationFailureData> Validation(string field, string code)
        {
            var data = new ValidationFailureData(
                new Dictionary<string, string>
                {
                    [field] = code
                }
            );

            return ApiResponse<ValidationFailureData>.Fail(
                ResponseCodes.Validation.InvalidRequest,
                data
            );
        }

        public static ApiResponse<ValidationFailureData> Validation(
            string firstField,
            string firstCode,
            string secondField,
            string secondCode)
        {
            var data = new ValidationFailureData(
                new Dictionary<string, string>
                {
                    [firstField] = firstCode,
                    [secondField] = secondCode
                }
            );

            return ApiResponse<ValidationFailureData>.Fail(
                ResponseCodes.Validation.InvalidRequest,
                data
            );
        }

        public static ApiResponse<ValidationFailureData> Validation(IReadOnlyDictionary<string, string> fields)
        {
            var data = new ValidationFailureData(fields);

            return ApiResponse<ValidationFailureData>.Fail(
                ResponseCodes.Validation.InvalidRequest,
                data
            );
        }
    }
}