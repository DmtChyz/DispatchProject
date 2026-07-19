namespace Shared.DTO.Generic
{
    public class PagedResult<T>
    {
        public List<T>? Items { get; private set; }
        public int TotalCount { get; private set; }
        public int Page { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages => PageSize == 0 ? 0 : (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool IsSuccess { get; private set; }
        public string? ErrorMessage { get; private set; }

        public static PagedResult<T> Success(List<T> items, int totalCount, int page, int pageSize) =>
            new PagedResult<T>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                IsSuccess = true
            };

        public static PagedResult<T> Fail(string errorMessage) =>
            new PagedResult<T>
            {
                IsSuccess = false,
                ErrorMessage = errorMessage
            };
    }
}