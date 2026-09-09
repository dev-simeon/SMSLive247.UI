namespace SMSLive247.OpenApi;

public sealed class PagingState
{
    public int PageSize { get; set; } = 25;
    public int CurrentPage { get; set; } = 1;
    public int? TotalRecords { get; set; }
    public int? TotalPages => TotalRecords.HasValue
        ? (int)Math.Ceiling((decimal)TotalRecords / (PageSize <= 0 ? 1 : PageSize)) : null;
}

public static class PaginationExtensions
{
    public static PagingState ToPagingState<T>(this SwaggerResponse<ICollection<T>> source)
    {
        var items = source.Result.ToList();
        var pageSize = GetHeaderValue("x-page-size", items.Count);
        var pageNumber = GetHeaderValue("x-page-number", 1);
        var totalCount = GetHeaderValue("x-total-count", items.Count);

        return new PagingState()
        {
            PageSize = pageSize,
            CurrentPage = pageNumber,
            TotalRecords = totalCount,
        };

        int GetHeaderValue(string headerKey, int errorValue)
        {
            var header = source.Headers
                .FirstOrDefault(h => string.Equals(h.Key, headerKey,
                    StringComparison.OrdinalIgnoreCase));

            var value = header.Value?.FirstOrDefault();

            return int.TryParse(value, out var result)
                ? result : errorValue;
        }

        //int GetHeaderValue(string headerKey, int errorValue)
        //{
        //    if (source.Headers.Keys.Contains(headerKey))
        //    {
        //        var value = source.Headers[headerKey].FirstOrDefault();

        //        if (int.TryParse(value, out int valid))
        //            return valid;
        //    }
        //    return errorValue;
        //}
    }
}
