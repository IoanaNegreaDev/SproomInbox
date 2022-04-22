using SproomInbox.WebApp.Shared.Resources.Parametrization.Paging;
using System.ComponentModel.DataAnnotations;

namespace SproomInbox.WebApp.Shared.Resources.Parametrization
{
    public class DocumentsQueryParameters
    {
        [Required]
        /// <summary>
        /// UserName special
        /// </summary>
        public string UserName { get; set; } = String.Empty;
        public string Type { get; set; } = String.Empty;
        public string State { get; set; } = String.Empty;
        public string Search { get; set; } = String.Empty;
      //  public string Fields { get; set; } = String.Empty;
        public PagedListMetadata Page { get; set; } = new PagedListMetadata();
    }
}
