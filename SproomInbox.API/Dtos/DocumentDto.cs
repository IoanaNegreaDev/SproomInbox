using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SproomInbox.WebApp.Shared.Resources
{
    /// <summary>
    /// The reprezentaion of Sproom documents
    /// </summary>
    public class DocumentDto
    {
        /// <summary>
        /// The Id of the document
        /// </summary>
        [Required]
        [Description("The Id of the document")]
        public Guid Id { get; set; } = Guid.Empty;
        /// <summary>
        /// The type of the document, in string format. Can be: Invoice, CreditNote
        /// </summary>
        [Description("The type of the document")]
        public string Type { get; set; } = String.Empty;
        /// <summary>
        /// The state of a document, in string format. Can be: Received, Approved, Rejected
        /// </summary>
        [Description("The state of a document,")]
        public string State { get; set; } = String.Empty;
        /// <summary>
        /// The creation date for the document
        /// </summary>
        [Description("The creation date for the document")]
        public DateTime CreationDate { get; set; }
        /// <summary>
        /// The file reference for the document
        /// </summary>
        [Description("The file reference for the document")]
        public string FileReference { get; set; } = String.Empty;
        public virtual ICollection<DocumentStateDto> StateHistory { get; set; }=  new HashSet<DocumentStateDto>();
    }
}
