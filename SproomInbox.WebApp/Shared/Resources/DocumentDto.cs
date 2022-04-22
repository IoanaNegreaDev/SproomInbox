namespace SproomInbox.WebApp.Shared.Resources
{
    public class DocumentDto
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Type { get; set; } = String.Empty;
        public string State { get; set; } = String.Empty;
        public DateTime CreationDate { get; set; }
        public string FileReference { get; set; } = String.Empty;
        public virtual ICollection<DocumentStateDto> StateHistory { get; set; }=  new HashSet<DocumentStateDto>();
    }
}
