namespace SproomInbox.WebApp.Shared.Resources.Parametrization
{
    public class PagedListMetadata
    {
        const int maxPageSize = 50;
        private int _pageSize = 10;
        public int TotalPages { get;  set; } = 0;
        public int TotalCount { get;  set; } = 0;
        public bool HasPrevious { get => (Current > 1); }
        public bool HasNext { get => (Current < TotalPages); }
        public int Current { get; set; } = 1;
        public string PreviousPageLink {get; set; } = string.Empty;
        public string NextPageLink {get; set; }= string.Empty;
        public int Size
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }
}
