using SproomInbox.API.Domain.Models;
using SproomInbox.API.Domain.Repositories;
using SproomInbox.API.Utils.Paging;
using SproomInbox.API.Utils.Parametrization;

namespace SproomInbox.API.Domain.Services
{
    public class DocumentsService : IDocumentsService
    {
        private readonly IDocumentsRepository _documentsRepository;
        public DocumentsService(IDocumentsRepository documentsRepository)
        {
            if (documentsRepository == null)
                throw new ArgumentNullException(nameof(documentsRepository));

            _documentsRepository = documentsRepository;
        }

        public async Task<PagedList<Document>> ListDocumentsAsync(QueryParameters queryParameters)
        {
            return await _documentsRepository.ListAsync(queryParameters);
        }
    }
}
