namespace SproomInbox.WebApp.Shared.Resources.Parametrization.Paging
{
    public class PagedListMetadata
    {
        const int maxPageSize = 50;
        private int _pageSize = 10;
        public int Current { get; set; } = 1;            
        public int Size
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }
}
