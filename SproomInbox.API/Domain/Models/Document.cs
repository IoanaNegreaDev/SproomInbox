using SproomInbox.WebApp.Shared.Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SproomInbox.API.Domain.Models
{
    [Table("Document")]
    public class Document
    {
        public Document()
        {
            StateHistory = new HashSet<DocumentState>();
        }

        [Key]
        public Guid Id { get; set; }
        public DocumentType TypeId { get; set; }
        public State StateId { get; set; }
        public DateTime CreationDate { get; set; }
        public string FileReference { get; set; } = null!;
        public int UserId { get; set; }

        [InverseProperty("Document")]
        public virtual ICollection<DocumentState> StateHistory { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("Documents")]
        public virtual User User { get; set; }
    }
}
