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
            modelBuilder.Entity<User>().HasData
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

            modelBuilder.Entity<Document>().HasData
            (
                new Document { Id = Guid.NewGuid(), 
                               UserId = 1,
                               TypeId = DocumentType.Invoice, 
                               StateId = State.Received, 
                               CreationDate = DateTime.Now.AddDays(-3), 
                               FileReference = "\\SproomDocumentFiles\\Invoices\\" },

                new Document { Id = Guid.NewGuid(), 
                                UserId = 1,
                                TypeId = DocumentType.Invoice,
                                StateId = State.Received, 
                                CreationDate = DateTime.Now.AddDays(-5), 
                                FileReference = "\\SproomDocumentFiles\\Invoices\\" },

                new Document { Id = Guid.NewGuid(),
                                UserId = 1,
                                TypeId = DocumentType.Invoice,
                                StateId = State.Received,
                                CreationDate = DateTime.Now.AddDays(-1),
                                FileReference = "\\SproomDocumentFiles\\Invoices\\" },

                new Document { Id = Guid.NewGuid(),
                                UserId = 2,     
                                TypeId = DocumentType.Invoice, 
                                StateId = State.Received, 
                                CreationDate = DateTime.Now.AddDays(-1),
                                FileReference = "\\SproomDocumentFiles\\Invoices\\" },

                new Document { Id = Guid.NewGuid(),
                                UserId = 2,     
                                TypeId = DocumentType.CreditNote, 
                                StateId = State.Received, 
                                CreationDate = DateTime.Now.AddDays(-10), 
                                FileReference = "\\SproomDocumentFiles\\CreditNotes\\" },

                new Document { Id = Guid.NewGuid(), 
                                UserId = 2,     
                                TypeId = DocumentType.CreditNote, 
                                StateId = State.Received, 
                                CreationDate = DateTime.Now.AddDays(-4), 
                                FileReference = "\\SproomDocumentFiles\\CreditNotes\\" },

                new Document { Id = Guid.NewGuid(),
                                UserId = 3,     
                                TypeId = DocumentType.CreditNote, 
                                StateId = State.Received,
                                CreationDate = DateTime.Now, 
                                FileReference = "\\SproomDocumentFiles\\CreditNotes\\" }
            );          
        }
    }
}
