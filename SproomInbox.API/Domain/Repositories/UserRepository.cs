using Microsoft.EntityFrameworkCore;
using SproomInbox.API.Domain.Models;
using System.Linq;

namespace SproomInbox.API.Domain.Repositories
{
    public class UserRepository : IUserRepository
    {
        protected DbSet<User> _table;
        protected readonly SproomDocumentsDbContext _context;

        public UserRepository(SproomDocumentsDbContext context)
        {
            if (context == null)
                throw new ArgumentNullException("Null DbContext.");

            _context = context;
            _table = _context.Set<User>();
        }

        public async Task<IEnumerable<User>> ListAsync()
            => await _table.ToListAsync();
    }
}
