using System.ComponentModel.DataAnnotations;

namespace SproomInbox.API.Parametrization
{
    /// <summary>
    /// query parameters fro documents
    /// </summary>
    public class DocumentsQueryParameters
    {
        [Required]
        /// <summary>
        /// The UserName of a user. Unique in the system. Required.
        public string UserName { get; set; } = String.Empty;
        /// <summary>
        /// The type of the requested document. Can be: Invoice or CreditNote
        /// </summary>
        public string Type { get; set; } = String.Empty;
        /// <summary>
        /// The state of the requested document. Can be: Received, Rejected, Approved
        /// </summary>
        public string State { get; set; } = String.Empty;
        /// <summary>
        /// Search string for parameterized search.
        /// </summary>
        public string Search { get; set; } = String.Empty;
        /// <summary>
        /// The current page number. ! based. Default value is 1.
        /// </summary>
        public int CurrentPage { get; set; } = 1;
        /// <summary>
        /// The number of dicuments per page. Default is 10. Maximum is 50.
        /// </summary>
        public int PageSize { get; set; } = 10;
    }
}
