using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SproomInbox.API.Domain.Models
{
    public class User
    {
        public User()
        {
            Documents = new HashSet<Document>();
        }

        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string? PasswordHash { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<Document> Documents { get; set; }
    }
}
