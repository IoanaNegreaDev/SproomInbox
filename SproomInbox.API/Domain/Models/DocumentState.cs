using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SproomInbox.API.Domain.Models
{
    [Table("DocumentState")]
    public class DocumentState
    {
        [Key]
        public int Id { get; set; }
        public Guid DocumentId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Timestamp { get; set; }
        public State StateId { get; set; }

        [ForeignKey("DocumentId")]
        [InverseProperty("StateHistory")]
        public virtual Document Document { get; set; } = null!;
    }
}
