using SproomInbox.API.Utils.Paging;

namespace SproomInbox.API.Utils.Parametrization
{
    public class QueryParameters
    {
        public string UserName { get; set; } = String.Empty;
        public string Type { get; set; } = String.Empty;
        public string State { get; set; } = String.Empty;
        public string Search { get; set; } = String.Empty;
        public string Fields { get; set; } = String.Empty;
        public PagedListMetadata Paging { get; set; } = new PagedListMetadata();
    }
}
