namespace SproomInbox.WebApp.Shared.Resources.Parametrization
{
    public class DocumentsUpdateStatusParameters
    {
        public List<Guid> DocumentIds { get; set; } = new List<Guid>();
        public string UserName {get; set; } = String.Empty;
        public string NewState {get; set; } = String.Empty;
    }
}
