using SproomInbox.API.Domain.Models;

namespace SproomInbox.API.Domain
{
    public class DataSeeder
    {
        private readonly SproomDocumentsDbContext _dbContext;

        public DataSeeder(SproomDocumentsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Seed()
        {
            _dbContext.Database.EnsureCreated();

            if (!_dbContext.Users.Any())
            {
                var users = new List<User>() 
                {
                    new User
                    {
                        FirstName = "Paul",
                        LastName = "Atreides",
                        UserName = "MuadDib"
                    },
                    new User
                    {
                        FirstName = "Frodo",
                        LastName = "Baggins",
                        UserName = "Hobbit"
                    },
                    new User
                    {
                        FirstName = "Harry",
                        LastName = "Potter",
                        UserName = "Wizard"
                    }
                };

                _dbContext.Users.AddRange(users);
                _dbContext.SaveChanges();
            }

            if (!_dbContext.Documents.Any())
            {
                Random rnd = new Random();
                List<Document> documents = new List<Document>();
                for (int i = 0; i < 300; i++)
                {
                    var document = new Document
                    {
                        UserId = rnd.Next(1, 4),
                        TypeId = (DocumentType)rnd.Next(1, 3),
                        StateId = State.Received,
                        CreationDate = DateTime.Now.AddDays(-rnd.Next(1, 21)),
                        FileReference = "\\SproomDocumentFiles\\"
                    };
                    documents.Add(document);
                }

                _dbContext.Documents.AddRange(documents);
                _dbContext.SaveChanges();
            }
        }
    }
}
