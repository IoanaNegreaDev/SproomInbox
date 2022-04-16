using SproomInbox.API.Domain.Models;

namespace SproomInbox.API.Domain.Repositories
{
    public interface IDocumentStateRepository
    {
        Task<DocumentState> AddAsync(DocumentState newDocumentState);
    }
}