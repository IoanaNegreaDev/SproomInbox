using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SproomInbox.API.Domain.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string? PasswordHash { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<Document> Documents { get; set; } = new HashSet<Document>();
    }
}
