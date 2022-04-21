using Moq;
using SproomInbox.API.Domain.Models;
using SproomInbox.API.Domain.Repositories;
using SproomInbox.API.Domain.Services;
using SproomInbox.WebApp.Shared.Resources.Parametrization;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SproomInbox.API.Test.Domanin.Services
{
    public class DocumentServiceFacts
    {
        [Fact]
        public void Constructor_OnNullParameters_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => new DocumentService(null));
        }

        [Fact]
        public async  void UpdateAsync_ValidParameters_Returns_List_StatusOk()
        {
            #region setup
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IDocumentRepository> documentRepositoryMock = new Mock<IDocumentRepository>();
            Mock<IDocumentStateRepository> documentStateRepositoryMock = new Mock<IDocumentStateRepository>();

            State newState = State.Approved;
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();

            List<Document> mockList = new List<Document>()
            {
                new Document {Id = id1, StateId = State.Received},
                new Document {Id = id2, StateId = State.Received}
            };

            DocumentsUpdateStatusParameters updateParameters = new DocumentsUpdateStatusParameters()
            {
                NewState = Enum.GetName(typeof(State), newState),
                DocumentIds = new List<string> { id1.ToString(), id2.ToString() }
            };

            DocumentState mockDocumentState = new DocumentState();

            documentRepositoryMock.SetupSequence(method => method.FindByIdAsync(It.IsAny<DocumentsFindByIdParameters>()))
                     .ReturnsAsync(mockList[0])
                     .ReturnsAsync(mockList[1]);
         
            documentStateRepositoryMock.SetupSequence(method => method.AddAsync(It.IsAny<DocumentState>()))
                     .ReturnsAsync(mockDocumentState)
                     .ReturnsAsync(mockDocumentState);

            unitOfWorkMock.Setup(method => method.DocumentRepository)
                     .Returns(documentRepositoryMock.Object);

            unitOfWorkMock.Setup(method => method.DocumentStateRepository)
                 .Returns(documentStateRepositoryMock.Object);

            DocumentService documentService = new DocumentService(unitOfWorkMock.Object);
            #endregion

            #region act
            var response = await documentService.UpdateAsync(updateParameters);
            #endregion

            #region assert
            Assert.True(response.Success);
            Assert.NotNull(response._entity);
            Assert.True(response._entity.Count() == 2);
            Assert.True(response._entity.ToList()[0].Id == mockList[0].Id && response._entity.ToList()[1].Id == mockList[1].Id);
            Assert.True(response._entity.ToList()[0].StateId == newState && response._entity.ToList()[1].StateId == newState);
            #endregion
        }


        [Fact]
        public async void UpdateAsync_ValidParameters_DbDocumentStatusNotUpdatable_Returns_BadRequest()
        {
            #region setup
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IDocumentRepository> documentRepositoryMock = new Mock<IDocumentRepository>();
            Mock<IDocumentStateRepository> documentStateRepositoryMock = new Mock<IDocumentStateRepository>();

            State newState = State.Approved;
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();

            List<Document> mockList = new List<Document>()
            {
                new Document {Id = id1, StateId = State.Received},
                new Document {Id = id2, StateId = State.Rejected}
            };

            DocumentsUpdateStatusParameters updateParameters = new DocumentsUpdateStatusParameters()
            {
                NewState = Enum.GetName(typeof(State), newState),
                DocumentIds = new List<string> { id1.ToString(), id2.ToString() }
            };

            DocumentState mockDocumentState = new DocumentState();

            documentRepositoryMock.SetupSequence(method => method.FindByIdAsync(It.IsAny<DocumentsFindByIdParameters>()))
                     .ReturnsAsync(mockList[0])
                     .ReturnsAsync(mockList[1]);

            documentStateRepositoryMock.SetupSequence(method => method.AddAsync(It.IsAny<DocumentState>()))
                     .ReturnsAsync(mockDocumentState)
                     .ReturnsAsync(mockDocumentState);

            unitOfWorkMock.Setup(method => method.DocumentRepository)
                     .Returns(documentRepositoryMock.Object);

            unitOfWorkMock.Setup(method => method.DocumentStateRepository)
                 .Returns(documentStateRepositoryMock.Object);

            DocumentService documentService = new DocumentService(unitOfWorkMock.Object);
            #endregion

            #region act
            var response = await documentService.UpdateAsync(updateParameters);
            #endregion

            #region assert
            Assert.False(response.Success);
            Assert.Null(response._entity);
            #endregion
        }


        [Fact]
        public async void UpdateAsync_ValidParameters_DocumentIdNotFound_Returns_BadRequest()
        {
            #region setup
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IDocumentRepository> documentRepositoryMock = new Mock<IDocumentRepository>();
            Mock<IDocumentStateRepository> documentStateRepositoryMock = new Mock<IDocumentStateRepository>();

            State newState = State.Rejected;
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();

            List<Document> mockList = new List<Document>()
            {
                new Document {Id = id1, StateId = State.Received},
                new Document {Id = id2, StateId = State.Received}
            };

            DocumentsUpdateStatusParameters updateParameters = new DocumentsUpdateStatusParameters()
            {
                NewState = Enum.GetName(typeof(State), newState),
                DocumentIds = new List<string> { id1.ToString(), id2.ToString() }
            };

            DocumentState mockDocumentState = new DocumentState();

            documentRepositoryMock.SetupSequence(method => method.FindByIdAsync(It.IsAny<DocumentsFindByIdParameters>()))
                     .ReturnsAsync(mockList[0])
                     .ReturnsAsync((Document)null); 

            documentStateRepositoryMock.SetupSequence(method => method.AddAsync(It.IsAny<DocumentState>()))
                     .ReturnsAsync(mockDocumentState)
                     .ReturnsAsync(mockDocumentState);

            unitOfWorkMock.Setup(method => method.DocumentRepository)
                     .Returns(documentRepositoryMock.Object);

            unitOfWorkMock.Setup(method => method.DocumentStateRepository)
                 .Returns(documentStateRepositoryMock.Object);

            DocumentService documentService = new DocumentService(unitOfWorkMock.Object);
            #endregion

            #region act
            var response = await documentService.UpdateAsync(updateParameters);
            #endregion

            #region assert
            Assert.False(response.Success);
            Assert.Null(response._entity);
            #endregion
        }
    }
}
