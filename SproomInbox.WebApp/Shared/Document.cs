namespace SproomInbox.WebApp.Shared
{
    public class Document
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreationDate { get; set; }
    }
}