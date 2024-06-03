using SMSLive247.OpenApi;

namespace SMSLive247.UI
{
    public class PagedList<T> : List<T>
    {
        public PageState PageState { get; private set; }

        internal PagedList(List<T> items, int pageSize, int pageNumber, int? totalCount = null)
        {
            PageState = new PageState(pageSize, pageNumber, totalCount);
            AddRange(items);
        }
    }

    public class PageState(int pageSize, int pageNumber, int? totalCount = null)
    {
        public int PageSize { get; private set; } = pageSize;
        public int PageNumber { get; private set; } = pageNumber;
        public int? TotalCount { get; private set; } = totalCount;
        //public int TotalPages { get; private set; } = (int)Math.Ceiling((decimal)totalCount / pageSize);
        public int? TotalPages
        {
            get
            {
                if (TotalCount == null)
                    return null;

                return (int)Math.Ceiling((decimal)TotalCount / PageSize);
            }
        }
        public bool HasPrevious => PageNumber > 1;
        public bool HasNext => PageNumber < TotalPages;
    }

    public static class PagedListExtensions
    {
        public static PagedList<T> ToPagedList<T>(this SwaggerResponse<ICollection<T>> source)
        {
            var pageSize = GetHeaderValue("x-page-size", 20);
            var pageNumber = GetHeaderValue("x-page-number", 1);
            var totalCount = GetHeaderValue("x-total-count", 100);
            //var totalPages = GetHeaderValue("x-total-pages");

            var items = source.Result.ToList();
            return new PagedList<T>(items, pageSize, pageNumber, totalCount);

            int GetHeaderValue(string headerKey, int errorValue)
            {
                if (source.Headers.Keys.Contains(headerKey))
                {
                    var value = source.Headers[headerKey].FirstOrDefault();

                    if (int.TryParse(value, out int valid))
                        return valid;
                }
                return errorValue;
            }
        }
    }
}
