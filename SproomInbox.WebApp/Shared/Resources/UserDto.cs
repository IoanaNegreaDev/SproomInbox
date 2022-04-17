namespace SproomInbox.WebApp.Shared.Resources
{
    public class UserDto
    {
        public UserDto()
        {
            Documents = new HashSet<DocumentDto>();
        }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<DocumentDto> Documents { get; set; }
    }
}
