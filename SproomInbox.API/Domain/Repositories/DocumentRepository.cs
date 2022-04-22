using Microsoft.EntityFrameworkCore;
using SproomInbox.API.Domain.Models;
using SproomInbox.API.Utils.Paging;
using SproomInbox.WebApp.Shared.Resources.Parametrization;
using SproomInbox.WebApp.Shared.Resources.Parametrization.Paging;

namespace SproomInbox.API.Domain.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        protected DbSet<Document> _table;
        protected readonly SproomDocumentsDbContext _context;

        public DocumentRepository(SproomDocumentsDbContext context)
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
            collection = collection.Include(document => document.StateHistory);
            return await PagedList<Document>.Create(collection, defaultPagingMetadata);
        }
        public async Task<PagedList<Document>> ListAsync(DocumentsQueryParameters queryParameters)
        {
            if (queryParameters == null)
                return await ListAsync();

            var collection = _table as IQueryable<Document>;
            collection = ApplyFilter(queryParameters, collection);
            collection = ApplySearch(queryParameters.Search, collection);
            collection = collection.Include(document => document.StateHistory);

            return await PagedList<Document>.Create(collection, queryParameters.Page);
        }
        private IQueryable<Document> ApplyFilter(DocumentsQueryParameters queryParameters, 
                                                 IQueryable<Document> collection)
        {
            bool ingnoreCase = true;

            if (!string.IsNullOrWhiteSpace(queryParameters.UserName))
            {
                collection = collection.Where(document => document.User.UserName == queryParameters.UserName);
            }

            if (!string.IsNullOrWhiteSpace(queryParameters.Type))
            {
                if (int.TryParse(queryParameters.Type, out _) == false && 
                    Enum.TryParse<DocumentType>(queryParameters.Type, ingnoreCase, out var queryTypeId))
                    collection = collection.Where(document => document.TypeId == queryTypeId);
            }

            if (!string.IsNullOrWhiteSpace(queryParameters.State))
            {
                if (int.TryParse(queryParameters.State, out _) == false &&
                    Enum.TryParse<State>(queryParameters.State, ingnoreCase, out var queryStateId))
                    collection = collection.Where(document => document.StateId == queryStateId);
            }

            return collection;
        }
        private IQueryable<Document> ApplySearch(string searchQuery, IQueryable<Document> collection)
        {
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                bool ingnoreCase = true;
                var lowerCaseSearchString = searchQuery.Trim().ToLower();

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
            return collection;
        }
        public async Task<Document?> FindByIdAsync(DocumentsFindByIdParameters findParameters)
        {
            if (findParameters == null)
                return null;

            return await _table.Where(document => document.Id == findParameters.Id &&
                                      document.User.UserName == findParameters.UserName)
                               .FirstOrDefaultAsync();
        }

        public void Update(Document document)
          => _table.Update(document);   
      }
}
