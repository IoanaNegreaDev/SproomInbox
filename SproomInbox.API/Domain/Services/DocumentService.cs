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

        public async Task<Status<PagedList<Document>>> ListDocumentsAsync(DocumentListQueryParameters queryParameters)
        {
            var response = await _unitOfWork.DocumentRepository.ListAsync(queryParameters);
            return new Status<PagedList<Document>>(response);
        }
        public async Task<Status<Document>> FindByIdAsync(DocumentsFindByIdParameters findParameters)
        {
            Document result = null; 
            try
            {
                result = await _unitOfWork.DocumentRepository.FindByIdAsync(findParameters);
            }
            catch
            {
                return new Status<Document>(HttpStatusCode.InternalServerError, 
                    $"Internal error ocurred while looking for document with id = {findParameters.Id}.");
            }
            if (result == null)
                return new Status<Document>(HttpStatusCode.NotFound, $"Document with id = {findParameters.Id} not found.");

            return new Status<Document>(result);
        }
       /* public async Task<Status<Document>> UpdateAsync(DocumentsFindByIdParameters findParameters, string newState)
        {
            if (!Enum.TryParse<State>(newState, true, out var newStateId) ||
                !Enum.IsDefined<State>(newStateId) ||
                newStateId == State.Received)
                return new Status<Document>(HttpStatusCode.BadRequest,
                  $"Invalid new state for update :{newState}. Allowed update states: {String.Join(", ", Enum.GetNames<State>())}.");

            var findResponse = await FindByIdAsync(findParameters);
            if (!findResponse.Success)
                return new Status<Document>(findResponse.StatusCode, findResponse.Message);

            var dbDocument = findResponse._entity;
            if (dbDocument.StateId != State.Received)
                return new Status<Document>(HttpStatusCode.BadRequest, $"Document {findParameters.Id} state cannot be modified.");

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
            catch 
            {
                return new Status<Document>(HttpStatusCode.InternalServerError, $"Failed to update the database with the {newState} state. "); 
            }

            return new Status<Document>(dbDocument);
        }*/

        public async Task<Status<IEnumerable<Document>>> UpdateAsync(DocumentListStatusUpdateParameters listUpdate)
        {
            if (!Enum.TryParse<State>(listUpdate.NewState, true, out var newStateId) ||
                !Enum.IsDefined<State>(newStateId) ||
                newStateId == State.Received)
                return new Status<IEnumerable<Document>>(HttpStatusCode.BadRequest, 
                    $"Invalid new state for update :{listUpdate.NewState}. Allowed update states: {String.Join(", ", Enum.GetNames<State>())}.");

            var updatedDocuments = new List<Document>();

            try
            {
                foreach (var documentId in listUpdate.DocumentIds)
                {
                    if (!Guid.TryParse(documentId, out var IdValue))
                        return new Status<IEnumerable<Document>>(HttpStatusCode.BadRequest,
                             $"Invalid state Id :{documentId}. Not a GUID value.");

                    DocumentsFindByIdParameters findDocumentByIdParameters = new DocumentsFindByIdParameters()
                    {
                        Id = IdValue,
                        UserName = listUpdate.UserName
                    };

                    var findResponse = await FindByIdAsync(findDocumentByIdParameters);
                    if (!findResponse.Success)
                        return new Status<IEnumerable<Document>>(findResponse.StatusCode, findResponse.Message);

                    var dbDocument = findResponse._entity;
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
            }
            catch
            {
                return new Status<IEnumerable<Document>>(HttpStatusCode.InternalServerError, 
                    $"Failed to update the documents with the {listUpdate.NewState} state. ");
            }

            return new Status<IEnumerable<Document>>(updatedDocuments);
        }
    }
}
