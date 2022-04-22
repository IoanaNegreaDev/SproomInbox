using Microsoft.EntityFrameworkCore;
using SproomInbox.API.Domain.Models;

namespace SproomInbox.API.Domain
{
    public partial class SproomDocumentsDbContext : DbContext
    {
        public SproomDocumentsDbContext()
        {
        }

        public SproomDocumentsDbContext(DbContextOptions<SproomDocumentsDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Document> Documents { get; set; } = null!;
        public virtual DbSet<DocumentState> DocumentStates { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {               
                optionsBuilder.UseSqlServer("Name=SproomDocumentsDbConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });


            modelBuilder.Entity<Document>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(d => d.User)
                 .WithMany(p => p.Documents)
                 .HasForeignKey(d => d.UserId)
                 .OnDelete(DeleteBehavior.ClientSetNull)
                 .HasConstraintName("FK_Document_User");
            });

            modelBuilder.Entity<DocumentState>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(d => d.Document)
                    .WithMany(p => p.StateHistory)
                    .HasForeignKey(d => d.DocumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DocumentState_Document");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
           /* modelBuilder.Entity<User>().HasData
            (
                new User
                {
                    Id = 1,
                    FirstName = "Paul",
                    LastName = "Atreides",
                    UserName = "MuadDib"
                },
                new User
                {
                    Id = 2,
                    FirstName = "Frodo",
                    LastName = "Baggins",
                    UserName = "Hobbit"
                },
                new User
                {
                    Id = 3,
                    FirstName = "Harry",
                    LastName = "Potter",
                    UserName = "Wizard"
                }
            );

            Random rnd = new Random();
            List<Document> seedDocuments = new List<Document>();
            for (int i = 0; i < 300; i++)
            {
                var document = new Document
                {
                    Id = Guid.NewGuid(),
                    UserId = rnd.Next(1, 4),
                    TypeId = (DocumentType)rnd.Next(1, 3),
                    StateId = State.Received,
                    CreationDate = DateTime.Now.AddDays(-rnd.Next(1, 21)),
                    FileReference = "\\SproomDocumentFiles\\"
                };
                seedDocuments.Add(document);
            }

            modelBuilder.Entity<Document>().HasData(seedDocuments);     */
        }
    }
}
