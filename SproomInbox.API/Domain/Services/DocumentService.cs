using SproomInbox.API.Domain.Models;
using SproomInbox.API.Domain.Repositories;
using SproomInbox.API.Utils.Paging;
using SproomInbox.WebApp.Shared.Resources.Parametrization;

namespace SproomInbox.API.Domain.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IUnitOfWork _unitOfWork;
        public DocumentService(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));

            _unitOfWork = unitOfWork;
        }

        public async Task<PagedList<Document>> ListDocumentsAsync(DocumentListQueryParameters queryParameters)
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
            if (!Enum.TryParse<State>(newState, true, out var newStateId) ||
                !Enum.IsDefined<State>(newStateId) ||
                newStateId == State.Received)
                throw new Exception($"Invalid new state for update :{newState}. Allowed update states: {String.Join(", ", Enum.GetNames<State>())}.");

            var dbDocument = await FindByIdAsync(findParameters);
            if (dbDocument == null)
                throw new Exception($"Document with {findParameters.Id} not found.");

            if (dbDocument.StateId != State.Received)
                throw new Exception($"Document {findParameters.Id} state cannot be modified.");

            try
            {
                var newDocumentState = new DocumentState()
                {
                    DocumentId = dbDocument.Id,
                    Timestamp = DateTime.Now,
                    StateId = dbDocument.StateId,
                };

                dbDocument.StateId = newStateId;
                _unitOfWork.DocumentRepository.Update(dbDocument);
                await _unitOfWork.DocumentStateRepository.AddAsync(newDocumentState);
                _unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update the database with {newState} state. Error: {ex.Message}.");
            }
            return dbDocument;
        }

        public async Task<IEnumerable<Document>> UpdateAsync(List<DocumentsFindByIdParameters> findParametersList, string newState)
        {
            if (!Enum.TryParse<State>(newState, true, out var newStateId) ||
                !Enum.IsDefined<State>(newStateId) ||
                newStateId == State.Received)
               throw new Exception($"Invalid new state for update :{newState}. Allowed update states: {String.Join(", ", Enum.GetNames<State>())}.");

            List<Document> updatedDocuments= new List<Document>();

            foreach (var findParameters in findParametersList)
            {
                var dbDocument = await FindByIdAsync(findParameters);
                if (dbDocument == null)
                    throw new Exception($"Document with {findParameters.Id} not found.");
            
                if (dbDocument.StateId != State.Received)
                    throw new Exception($"Document {findParameters.Id} state cannot be modified.");
           
                var newDocumentState = new DocumentState()
                {
                    DocumentId = dbDocument.Id,
                    Timestamp = dbDocument.CreationDate,
                    StateId = dbDocument.StateId,
                };

                dbDocument.StateId = newStateId;
                dbDocument.CreationDate = DateTime.Now;
                _unitOfWork.DocumentRepository.Update(dbDocument);
                await _unitOfWork.DocumentStateRepository.AddAsync(newDocumentState);
                    
                updatedDocuments.Add(dbDocument);
            }

            if (updatedDocuments.Count > 0)
                _unitOfWork.SaveChanges();

            return updatedDocuments;
        }
    }
}
