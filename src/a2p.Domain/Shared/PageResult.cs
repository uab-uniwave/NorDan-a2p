namespace Domain.Shared
{
    public class PagedResult<T> : Result<IEnumerable<T>>
    {
        public int TotalCount { get; }
        public int PageIndex { get; }
        public int PageSize { get; }

        private PagedResult(IEnumerable<T> items, int totalCount, int pageIndex, int pageSize)
        : base(true, items) // ✅ now works because base ctor is protected
        {
            TotalCount = totalCount;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public static PagedResult<T> Success(IEnumerable<T> items, int totalCount, int pageIndex, int pageSize)
        => new(items, totalCount, pageIndex, pageSize);

        public static new PagedResult<T> Failure(string message)
        => new(Array.Empty<T>(), 0, 0, 0);
    }
}
