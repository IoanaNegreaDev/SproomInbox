using SproomInbox.API.Domain.Models;
using SproomInbox.API.Domain.Repositories;
using SproomInbox.API.Utils.Paging;
using SproomInbox.API.Utils.Parametrization;

namespace SproomInbox.API.Domain.Services
{
    public class DocumentsService : IDocumentsService
    {
        private readonly IUnitOfWork _unitOfWork;
      //  private readonly IDocumentsRepository _documentsRepository;
        public DocumentsService(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));

            _unitOfWork = unitOfWork;
        }

        public async Task<PagedList<Document>> ListDocumentsAsync(DocumentsQueryParameters queryParameters)
        {
            return await _unitOfWork.DocumentRepository.ListAsync(queryParameters);
        }
        public async Task<Document> FindByIdAsync(DocumentsFindByIdParameters findParameters)
        {
            var result = await _unitOfWork.DocumentRepository.FindByIdAsync(findParameters);
            return result;
        }
        public async Task<Document> UpdateAsync(DocumentsFindByIdParameters findParameters, string newState)
        {
            var dbDocument = await FindByIdAsync(findParameters);
            if (dbDocument == null)
                throw new Exception($"Document with {findParameters.Id} not found.");

            if (Enum.TryParse<State>(newState, true, out var newStateId) &&
                Enum.IsDefined<State>(newStateId))
            {
                var newDocumentState = new DocumentState()
                {
                    DocumentId = dbDocument.Id,
                    Timestamp = DateTime.Now,
                    StateId = dbDocument.StateId,
                };

                dbDocument.StateId = newStateId;
                _unitOfWork.DocumentRepository.Update(dbDocument);
                _unitOfWork.DocumentStateRepository.AddAsync(newDocumentState);
                _unitOfWork.SaveChanges();
            }


            return dbDocument;
        }
    }
}
