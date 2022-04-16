namespace SproomInbox.API.Utils.Paging
{
    public class PagedListMetadata
    {
        const int maxPageSize = 50;
        private int _pageSize = 10;
        public int CurrentPage { get; set; } = 1;            
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }
}
