using SproomInbox.WebApp.Shared.Resources.Parametrization;

namespace SproomInbox.WebApp.Server.Services
{
    public interface IEmailService
    {
        Task SendApprovedDocumentsEmail(DocumentListStatusUpdateParameters updatedDocuments);
    }
}