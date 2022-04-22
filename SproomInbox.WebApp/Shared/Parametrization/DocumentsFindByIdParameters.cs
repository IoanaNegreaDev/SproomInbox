namespace SproomInbox.WebApp.Shared.Resources.Parametrization
{
    public class DocumentsFindByIdParameters
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string UserName { get; set; } = String.Empty;
    }
}
