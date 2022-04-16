namespace SproomInbox.API.Utils.Parametrization
{
    public class DocumentsFindByIdParameters
    {    
        public string UserName { get; set; } = String.Empty;
        public Guid Id { get; set; } = Guid.Empty;
    }
}
