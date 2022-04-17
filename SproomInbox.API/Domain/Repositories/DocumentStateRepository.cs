using Microsoft.EntityFrameworkCore;
using SproomInbox.API.Domain.Models;

namespace SproomInbox.API.Domain.Repositories
{
    public class DocumentStateRepository : IDocumentStateRepository
    {
        protected DbSet<DocumentState> _table;
        protected readonly SproomDocumentsDbContext _context;

        public DocumentStateRepository(SproomDocumentsDbContext context)
        {
            if (context == null)
                throw new ArgumentNullException("Null DbContext.");

            _context = context;
            _table = _context.Set<DocumentState>();
        }

        public async Task<DocumentState> AddAsync(DocumentState newDocumentState)
        {
            await _table.AddAsync(newDocumentState);
            return newDocumentState;
        }
    }
}

