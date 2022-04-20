using SproomInbox.WebApp.Shared.Resources.Parametrization;

namespace SproomInbox.WebApp.Server.Services
{
    public class EmailService : IEmailService
    {
        public EmailService()
        { }

        public async Task SendApprovedDocumentsEmail(DocumentListStatusUpdateParameters updatedDocuments)
        {
            // take the email address from somewhere
        }
    }
}
