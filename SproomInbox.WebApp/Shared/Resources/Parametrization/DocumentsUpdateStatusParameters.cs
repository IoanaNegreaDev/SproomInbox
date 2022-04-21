namespace SproomInbox.WebApp.Shared.Resources.Parametrization
{
    public class DocumentsUpdateStatusParameters
    {
        public List<string> DocumentIds { get; set; }
        public string UserName {get; set;}
        public string NewState {get; set;}
    }
}
