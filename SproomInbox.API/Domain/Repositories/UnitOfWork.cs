namespace SproomInbox.API.Domain.Repositories
{
    public interface IUnitOfWork
    {
        IDocumentStateRepository DocumentStateRepository { get; }
        IDocumentsRepository DocumentRepository { get; }

        IUserRepository UserRepository { get; }
        void SaveChanges();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private SproomDocumentsDbContext _context;

        public UnitOfWork(SproomDocumentsDbContext context)
        {
            _context = context;
        }

        private IDocumentStateRepository _documentStateRepository;
      
        public IDocumentStateRepository DocumentStateRepository
        {
            get
            {
                if (_documentStateRepository == null)
                {
                    _documentStateRepository = new DocumentStateRepository(_context);
                }

                return _documentStateRepository;
            }
        }

        private IDocumentsRepository _documentRepository;
        public IDocumentsRepository DocumentRepository
        {
            get
            {
                if(_documentRepository == null)
                {
                    _documentRepository = new DocumentsRepository(_context);
                }

                return _documentRepository;
            }
        }

        private IUserRepository _userRepository;
        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(_context);
                }

                return _userRepository;
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
