using System.ComponentModel.DataAnnotations;

namespace SproomInbox.WebApp.Shared.Resources.Parametrization
{
    public class DocumentsUpdateStatusParameters
    {
        /// <summary>
        /// A list of document Ids to be updated
        /// </summary>
        public List<Guid> DocumentIds { get; set; } = new List<Guid>();
        /// <summary>
        /// The user name o a specifiic user
        /// </summary>
        [Required]
        public string UserName {get; set; } = String.Empty;
        /// <summary>
        /// The new state. Allowed values: Approved, Rejected
        /// </summary>
        public string NewState {get; set; } = String.Empty;
    }
}
