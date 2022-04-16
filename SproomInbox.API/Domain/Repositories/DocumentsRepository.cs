using Microsoft.EntityFrameworkCore;
using SproomInbox.API.Domain.Models;
using SproomInbox.API.Utils.Extensions;
using SproomInbox.API.Utils.Paging;
using SproomInbox.API.Utils.Parametrization;
using System.Globalization;

namespace SproomInbox.API.Domain.Repositories
{
    public class DocumentsRepository : IDocumentsRepository
    {
        protected DbSet<Document> _table = null;
        protected readonly SproomDocumentsDbContext _context;

        public DocumentsRepository(SproomDocumentsDbContext context)
        {
            if (context == null)
                throw new ArgumentNullException("Null DbContext.");

            _context = context;
            _table = _context.Set<Document>();
        }

        public async Task<PagedList<Document>> ListAsync()
        {
            PagedListMetadata defaultPagingMetadata = new PagedListMetadata();
            var collection = _table as IQueryable<Document>;

            return await PagedList<Document>.Create(collection, defaultPagingMetadata);
        }

        public async Task<PagedList<Document>> ListAsync(QueryParameters queryParameters)
        {
            if (queryParameters == null)
                return await ListAsync();

            var collection = _table as IQueryable<Document>;
            bool ingnoreCase = true;


            if (!string.IsNullOrWhiteSpace(queryParameters.UserName))
            {
                collection = collection.Where(document => document.User.UserName == queryParameters.UserName);
            }

            if (!string.IsNullOrWhiteSpace(queryParameters.Type))
            {
                if (!Enum.TryParse<DocumentType>(queryParameters.Type, ingnoreCase, out var queryTypeId))
                    throw new Exception("Invalid document type parameter.");

                collection = collection.Where(document => document.TypeId == queryTypeId);
            }

            if (!string.IsNullOrWhiteSpace(queryParameters.State))
            {
                if (!Enum.TryParse<State>(queryParameters.State, ingnoreCase, out var queryStateId))
                    throw new Exception("Invalid document state parameter.");

                collection = collection.Where(document => document.StateId == queryStateId);
            }

            if (!string.IsNullOrWhiteSpace(queryParameters.Search))
            {
                var lowerCaseSearchString = queryParameters.Search.ToLower();
  
                var documentTypeString = Enum.GetNames(typeof(DocumentType)).Where(type => type.ToLower().Contains(lowerCaseSearchString)).FirstOrDefault();
                Enum.TryParse<DocumentType>(documentTypeString, ingnoreCase, out var documentTypeValue);

                var documentStateString = Enum.GetNames(typeof(State)).Where(state => state.ToLower().Contains(lowerCaseSearchString)).FirstOrDefault();
                Enum.TryParse<State>(documentStateString, ingnoreCase, out var documentStateValue);

                collection = collection.Where(document => document.User.UserName.ToLower().Contains(lowerCaseSearchString) ||
                                                          document.User.FirstName.ToLower().Contains(lowerCaseSearchString) ||
                                                          document.TypeId == documentTypeValue ||
                                                          document.StateId == documentStateValue ||                                           
                                                          document.CreationDate.ToString().ToLower().Contains(lowerCaseSearchString));
            }

            return await PagedList<Document>.Create(collection, queryParameters.Paging);
        }
    }
}
