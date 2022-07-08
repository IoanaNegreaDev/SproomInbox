using Microsoft.EntityFrameworkCore;
using SproomInbox.API.Domain;
using SproomInbox.API.Domain.Models;
using SproomInbox.API.Domain.Repositories;
using SproomInbox.API.Parametrization;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SproomInbox.API.Test.Domanin.Repositories
{
 
    public class DocumentRepositoryFacts
    {
        private SproomDocumentsDbContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<SproomDocumentsDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var databaseContext = new SproomDocumentsDbContext(options);
            databaseContext.Database.EnsureCreated();

            if (databaseContext.Documents.Count() <= 0)
            {
                var id1 = Guid.NewGuid();
                var id2 = Guid.NewGuid();
                var id3 = Guid.NewGuid();  

                databaseContext.Documents.Add(new Document()
                {
                    Id = id1,
                    TypeId = DocumentType.Invoice,
                    StateId = State.Received,
                    CreationDate = DateTime.Now,
                    FileReference = String.Empty,
                    UserId = 1
                });

                databaseContext.Documents.Add(new Document()
                {
                    Id = id2,
                    TypeId = DocumentType.CreditNote,
                    StateId = State.Approved,
                    CreationDate = DateTime.Now,
                    FileReference = String.Empty,
                    UserId = 1,
                    StateHistory = new List<DocumentState>()
                    {
                        new DocumentState
                        {
                            StateId = State.Received,
                            Timestamp = DateTime.Now.AddDays(-1)
                        }
                    }
                });

                databaseContext.Documents.Add(new Document()
                {               
                    Id = id3,
                    TypeId = DocumentType.Invoice,
                    StateId = State.Rejected,
                    CreationDate = DateTime.Now,
                    FileReference = String.Empty,
                    UserId = 2,
                    StateHistory = new List<DocumentState>()
                    {
                        new DocumentState
                        {
                            StateId = State.Received,
                            Timestamp = DateTime.Now.AddDays(-1)
                        }
                    }
                });

                databaseContext.Users.Add(new User()
                {
                    Id = 1,
                    UserName = "user1",
                    FirstName = "first1",
                    LastName = "last1",
                });

                databaseContext.Users.Add(new User()
                {
                    Id = 2,
                    UserName = "user2",
                    FirstName = "first2",
                    LastName = "last2",
                });

                databaseContext.SaveChanges();
            }
            return databaseContext; 
        }

        [Fact]
        public async void ListAsync_NullParameters_Returns_All_Items()
        {
            var dbInMemoryContext = GetDatabaseContext();
            DocumentRepository repository = new DocumentRepository(dbInMemoryContext);
            var response = await repository.ListAsync(null);
            Assert.True(response.Count == 3);
            dbInMemoryContext.Dispose();
        }
     
        [Fact]
        public async void ListAsync_NoParameters_Returns_3_items()
        {
            var dbInMemoryContext = GetDatabaseContext();
            DocumentRepository repository = new DocumentRepository(dbInMemoryContext);
            DocumentsQueryParameters documentsQueryParameters = new DocumentsQueryParameters();
            var response = await repository.ListAsync(documentsQueryParameters);
            Assert.True(response.Count == 3);
            dbInMemoryContext.Dispose();
        }

        [Fact]
        public async void ListAsync_QueryUsername_Returns_2_items()
        {
            var dbInMemoryContext = GetDatabaseContext();
            DocumentRepository repository = new DocumentRepository(dbInMemoryContext);
            DocumentsQueryParameters documentsQueryParameters = new DocumentsQueryParameters()
            { UserName = "user1" };
            var response = await repository.ListAsync(documentsQueryParameters);
            Assert.True(response.Count == 2);
            dbInMemoryContext.Dispose();
        }

        [Fact]
        public async void ListAsync_QueryUsername_And_State_Returns_1_Item()
        {
            var dbInMemoryContext = GetDatabaseContext();
            DocumentRepository repository = new DocumentRepository(dbInMemoryContext);
            DocumentsQueryParameters documentsQueryParameters = new DocumentsQueryParameters()
            { UserName = "user1", State = "Approved" };
            var response = await repository.ListAsync(documentsQueryParameters);
            Assert.True(response.Count == 1);
        }

        [Fact]
        public async void ListAsync_QueryUsername_And_State_DeosntApplyFStateFilter_Returns_2_Items()
        {
            var dbInMemoryContext = GetDatabaseContext();
            DocumentRepository repository = new DocumentRepository(dbInMemoryContext);
            DocumentsQueryParameters documentsQueryParameters = new DocumentsQueryParameters()
            { UserName = "user1", State = "Approvede" };
            var response = await repository.ListAsync(documentsQueryParameters);
            Assert.True(response.Count == 2);
        }

        [Fact]
        public async void ListAsync_QueryUsername_And_State_And_Type_Returns_0_Items()
        {
            var dbInMemoryContext = GetDatabaseContext();
            DocumentRepository repository = new DocumentRepository(dbInMemoryContext);
            DocumentsQueryParameters documentsQueryParameters = new DocumentsQueryParameters()
            { UserName = "user1", State = "Rejected", Type = "Invoice" };
            var response = await repository.ListAsync(documentsQueryParameters);
            Assert.True(response.Count == 0);
        }

        [Fact]
        public async void ListAsync_QueryType_Returns_2_Items()
        {
            var dbInMemoryContext = GetDatabaseContext();
            DocumentRepository repository = new DocumentRepository(dbInMemoryContext);
            DocumentsQueryParameters documentsQueryParameters = new DocumentsQueryParameters()
            {Type = "Invoice" };
            var response = await repository.ListAsync(documentsQueryParameters);
            Assert.True(response.Count == 2);
        }

        [Fact]
        public async void ListAsync_QueryInvalidType_FillterDeosntAppply_Returns_All_Items()
        {
            var dbInMemoryContext = GetDatabaseContext();
            DocumentRepository repository = new DocumentRepository(dbInMemoryContext);
            DocumentsQueryParameters documentsQueryParameters = new DocumentsQueryParameters()
            { Type = "Invoices" };
            var response = await repository.ListAsync(documentsQueryParameters);
            Assert.True(response.Count == 3);
        }

        [Fact]
        public async void ListAsync_QueryType_CaseInsensitive_Returns_2_Items()
        {
            var dbInMemoryContext = GetDatabaseContext();
            DocumentRepository repository = new DocumentRepository(dbInMemoryContext);
            DocumentsQueryParameters documentsQueryParameters = new DocumentsQueryParameters()
            { Type = "inVoice" };
            var response = await repository.ListAsync(documentsQueryParameters);
            Assert.True(response.Count == 2);
        }

        [Fact]
        public async void ListAsync_QueryState_CaseInsensitive_Returns_2_Items()
        {
            var dbInMemoryContext = GetDatabaseContext();
            DocumentRepository repository = new DocumentRepository(dbInMemoryContext);
            DocumentsQueryParameters documentsQueryParameters = new DocumentsQueryParameters()
            { State = "approveD" };
            var response = await repository.ListAsync(documentsQueryParameters);
            Assert.True(response.Count == 1);
        }

        [Fact]
        public async void ListAsync_QueryType_AsNumericValue_FilterDeosntApply_Returns_All_Items()
        {
            var dbInMemoryContext = GetDatabaseContext();
            DocumentRepository repository = new DocumentRepository(dbInMemoryContext);
            DocumentsQueryParameters documentsQueryParameters = new DocumentsQueryParameters()
            { Type = "1" };
            var response = await repository.ListAsync(documentsQueryParameters);
            Assert.True(response.Count == 3);
        }

        // etc, etc....
    }
}
