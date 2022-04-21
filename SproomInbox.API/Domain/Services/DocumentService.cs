using SproomInbox.API.Domain.Models;
using SproomInbox.API.Domain.Repositories;
using SproomInbox.API.Utils.ErrorHandling;
using SproomInbox.API.Utils.Paging;
using SproomInbox.WebApp.Shared.Resources.Parametrization;
using System.Net;

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

        public async Task<Status<PagedList<Document>>> ListDocumentsAsync(DocumentsQueryParameters queryParameters)
        {
            var response = await _unitOfWork.DocumentRepository.ListAsync(queryParameters);
            if (response == null)
                throw new Exception("Failed to get documents. Internal error.");

            return new Status<PagedList<Document>>(response);
        }
        public async Task<Status<Document>> FindByIdAsync(DocumentsFindByIdParameters findParameters)
        {
            var  response = await _unitOfWork.DocumentRepository.FindByIdAsync(findParameters);    
            if (response == null)
                return new Status<Document>(HttpStatusCode.NotFound, $"Document with id = {findParameters.Id} not found.");

            return new Status<Document>(response);
        }
  
        public async Task<Status<IEnumerable<Document>>> UpdateAsync(DocumentsUpdateStatusParameters updateParameters)
        {
            // updateParameters.NewState already validated by fluent validator
            var newStateId = Enum.Parse<State>(updateParameters.NewState);
            var updatedDocuments = new List<Document>();

            foreach (var documentId in updateParameters.DocumentIds)
            {
                // documentId already validated by fluent validator
                var IdValue = Guid.Parse(documentId);   
                DocumentsFindByIdParameters findDocumentByIdParameters = new DocumentsFindByIdParameters()
                {
                    Id = IdValue,
                    UserName = updateParameters.UserName
                };

                var dbDocument = await _unitOfWork.DocumentRepository.FindByIdAsync(findDocumentByIdParameters);
                if (dbDocument == null)
                    return new Status<IEnumerable<Document>>(HttpStatusCode.BadRequest, 
                                            $"Document {IdValue} not found.");

                if (dbDocument.StateId != State.Received)
                    return new Status<IEnumerable<Document>>(HttpStatusCode.BadRequest,
                                                            $"Document {documentId}'s state cannot be modified.");
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

            return new Status<IEnumerable<Document>>(updatedDocuments);
        }
    }
}
