namespace SproomInbox.API.DTOs
{
    public class DocumentDto
    {
        public DocumentDto()
        {
            StateHistory = new HashSet<DocumentStateDto>();
        }
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string State { get; set; }
        public DateTime CreationDate { get; set; }
        public string FileReference { get; set; } = null!;
        public virtual ICollection<DocumentStateDto> StateHistory { get; set; }
    }
}
