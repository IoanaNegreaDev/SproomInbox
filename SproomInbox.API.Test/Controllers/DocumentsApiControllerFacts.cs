using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SproomInbox.API.Domain.Models;
using SproomInbox.API.Domain.Services;
using SproomInbox.API.Utils.ErrorHandling;
using SproomInbox.API.Utils.Paging;
using SproomInbox.WebApp.Shared.Resources;
using SproomInbox.WebApp.Shared.Resources.Parametrization;
using SproomInbox.WebApp.Shared.Resources.Parametrization.Paging;
using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;

namespace SproomInbox.API.Test
{
    public class DocumentsApiControllerFacts
    {
        [Fact]
        public void Constructor_OnNullParameters_ThrowsException()
        {
            Mock<ILogger<DocumentsApiController>> loggerMock = new Mock<ILogger<DocumentsApiController>>();
            Mock<IDocumentService> documentServiceMock = new Mock<IDocumentService>();
            Mock<IPaginationUriBuilder> paginationUriBuilderMock = new Mock<IPaginationUriBuilder>();
            Mock<IMapper> mapperMock = new Mock<IMapper>();

            Assert.Throws<ArgumentNullException>(() => new DocumentsApiController(null,
                                                                paginationUriBuilderMock.Object,
                                                                mapperMock.Object,
                                                                loggerMock.Object));

            Assert.Throws<ArgumentNullException>(() => new DocumentsApiController(documentServiceMock.Object,
                                                             null,
                                                             mapperMock.Object,
                                                             loggerMock.Object));

            Assert.Throws<ArgumentNullException>(() => new DocumentsApiController(documentServiceMock.Object,
                                                             paginationUriBuilderMock.Object,
                                                             null,
                                                             loggerMock.Object));

            Assert.Throws<ArgumentNullException>(() => new DocumentsApiController(documentServiceMock.Object,
                                                             paginationUriBuilderMock.Object,
                                                             mapperMock.Object,
                                                             null));         
        }

        [Fact]
        public async void GetDocuments_ValidParameters_ReturnsPagedList_And_StatusOK()
        {
            #region setup
            Mock<ILogger<DocumentsApiController>> loggerMock = new Mock<ILogger<DocumentsApiController>>();
            Mock<IDocumentService> documentServiceMock = new Mock<IDocumentService>();
            Mock<IPaginationUriBuilder> paginationUriBuilderMock = new Mock<IPaginationUriBuilder>();
            Mock<IMapper> mapperMock = new Mock<IMapper>();
            HttpContext httpContext = new DefaultHttpContext();
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();

            List<Document> mockList = new List<Document>()
            {
                new Document {Id = id1},
                new Document {Id = id2}
            };

            List<DocumentDto> mockListDto = new List<DocumentDto>()
            { 
                new DocumentDto { Id= id1 },
                new DocumentDto { Id= id1 },
            };

            PagedList<Document> mockPagedList = new PagedList<Document>(mockList, new PagedListMetadata());
            PagedList<DocumentDto> mockPagedListDto = new PagedList<DocumentDto>(mockListDto, new PagedListMetadata());
            var mocServiceReturnStatusOk = new Status<PagedList<Document>>(mockPagedList);
            var mockDtoReturnStatusOk = new Status<PagedList<DocumentDto>>(mockPagedListDto);

            var dummyQueryParameters = new DocumentsQueryParameters();
            var dummyPagedListMetadata = new PagedListMetadata();

            documentServiceMock.Setup(method => method.ListDocumentsAsync(dummyQueryParameters))
                               .ReturnsAsync(mocServiceReturnStatusOk);
                                  
            mapperMock.Setup(m => m.Map<PagedList<Document>, PagedList<DocumentDto>>(
                             It.IsAny<PagedList<Document>>())).Returns(new PagedList<DocumentDto>(mockListDto, dummyPagedListMetadata));
            #endregion
         
            #region act
            DocumentsApiController documentController = new DocumentsApiController(documentServiceMock.Object,
                                                                                   paginationUriBuilderMock.Object,
                                                                                   mapperMock.Object,
                                                                                   loggerMock.Object)
            {
                ControllerContext = controllerContext
            };

            var response = await documentController.GetDocumentsAsync(dummyQueryParameters);
            #endregion

            #region assert
            var actionResult = (OkObjectResult)response.Result; 
            var okResultValue = actionResult.Value;

            Assert.IsType<OkObjectResult>(response.Result);
            var returnedValue = Assert.IsAssignableFrom<PagedList<DocumentDto>>(okResultValue);
            Assert.Equal(2, returnedValue.Count());
            Assert.True(returnedValue[0] == mockListDto[0] && returnedValue[1] == mockListDto[1]);
            Assert.True(documentController.Response.Headers["X-Pagination"].Any());
            #endregion
        }

        [Fact]
        public async void GetDocuments_ValidParameters_NullReturnList_StatusNoContent()
        {
            #region setup
            Mock<ILogger<DocumentsApiController>> loggerMock = new Mock<ILogger<DocumentsApiController>>();
            Mock<IDocumentService> documentServiceMock = new Mock<IDocumentService>();
            Mock<IPaginationUriBuilder> paginationUriBuilderMock = new Mock<IPaginationUriBuilder>();
            Mock<IMapper> mapperMock = new Mock<IMapper>();
            HttpContext httpContext = new DefaultHttpContext();
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            List<Document> mockList = new List<Document>();
            List<DocumentDto> mockListDto = new List<DocumentDto>();
           
            PagedList<Document> mockPagedList = new PagedList<Document>(mockList, new PagedListMetadata());
            PagedList<DocumentDto> mockPagedListDto = new PagedList<DocumentDto>(mockListDto, new PagedListMetadata());

            var mocServiceReturnStatusOk = new Status<PagedList<Document>>(mockPagedList);
            var mockDtoReturnStatusOk = new Status<PagedList<DocumentDto>>(mockPagedListDto);

            var dummyQueryParameters = new DocumentsQueryParameters();
            var dummyPagedListMetadata = new PagedListMetadata();

            documentServiceMock.Setup(method => method.ListDocumentsAsync(dummyQueryParameters))
                               .ReturnsAsync(mocServiceReturnStatusOk);

            mapperMock.Setup(m => m.Map<PagedList<Document>, PagedList<DocumentDto>>(
                             It.IsAny<PagedList<Document>>())).Returns(new PagedList<DocumentDto>(mockListDto, dummyPagedListMetadata));
            #endregion

            #region act
            DocumentsApiController documentController = new DocumentsApiController(documentServiceMock.Object,
                                                                                   paginationUriBuilderMock.Object,
                                                                                   mapperMock.Object,
                                                                                   loggerMock.Object)
            {
                ControllerContext = controllerContext
            };

            var response = await documentController.GetDocumentsAsync(dummyQueryParameters);
            #endregion

            #region assert
            Assert.IsType<NoContentResult>(response.Result);
            #endregion  
        }

        [Fact]
        public async void GetDocuments_ValidParameters_NullReturnList_XPagination_Header_Ok()
        {
            #region setup
            Mock<ILogger<DocumentsApiController>> loggerMock = new Mock<ILogger<DocumentsApiController>>();
            Mock<IDocumentService> documentServiceMock = new Mock<IDocumentService>();
            Mock<IPaginationUriBuilder> paginationUriBuilderMock = new Mock<IPaginationUriBuilder>();
            Mock<IMapper> mapperMock = new Mock<IMapper>();
            HttpContext httpContext = new DefaultHttpContext();
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();

            List<Document> mockList = new List<Document>()
            {
                new Document {Id = id1},
                new Document {Id = id2}
            };

            List<DocumentDto> mockListDto = new List<DocumentDto>()
            {
                new DocumentDto { Id= id1 },
                new DocumentDto { Id= id1 },
            };

            PagedList<Document> mockPagedList = new PagedList<Document>(mockList, new PagedListMetadata());
            PagedList<DocumentDto> mockPagedListDto = new PagedList<DocumentDto>(mockListDto, new PagedListMetadata());
            var mocServiceReturnStatusOk = new Status<PagedList<Document>>(mockPagedList);
            var mockDtoReturnStatusOk = new Status<PagedList<DocumentDto>>(mockPagedListDto);

            var dummyQueryParameters = new DocumentsQueryParameters();
            var dummyPagedListMetadata = new PagedListMetadata();

            documentServiceMock.Setup(method => method.ListDocumentsAsync(dummyQueryParameters))
                               .ReturnsAsync(mocServiceReturnStatusOk);

            mapperMock.Setup(m => m.Map<PagedList<Document>, PagedList<DocumentDto>>(
                             It.IsAny<PagedList<Document>>())).Returns(new PagedList<DocumentDto>(mockListDto, dummyPagedListMetadata));
            #endregion

            #region act
            DocumentsApiController documentController = new DocumentsApiController(documentServiceMock.Object,
                                                                                   paginationUriBuilderMock.Object,
                                                                                   mapperMock.Object,
                                                                                   loggerMock.Object)
            {
                ControllerContext = controllerContext
            };

            var response = await documentController.GetDocumentsAsync(dummyQueryParameters);
            #endregion

            #region assert
            Assert.True(documentController.Response.Headers["X-Pagination"].Any());
            #endregion
        }

        [Fact]
        public async void GetDocuments_ValidParameters_NullReturnList_XPagination_Header_Missing()
        {
            #region setup
            Mock<ILogger<DocumentsApiController>> loggerMock = new Mock<ILogger<DocumentsApiController>>();
            Mock<IDocumentService> documentServiceMock = new Mock<IDocumentService>();
            Mock<IPaginationUriBuilder> paginationUriBuilderMock = new Mock<IPaginationUriBuilder>();
            Mock<IMapper> mapperMock = new Mock<IMapper>();
            HttpContext httpContext = new DefaultHttpContext();
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            List<Document> mockList = new List<Document>();
            List<DocumentDto> mockListDto = new List<DocumentDto>();

            PagedList<Document> mockPagedList = new PagedList<Document>(mockList, new PagedListMetadata());
            PagedList<DocumentDto> mockPagedListDto = new PagedList<DocumentDto>(mockListDto, new PagedListMetadata());
            var mocServiceReturnStatusOk = new Status<PagedList<Document>>(mockPagedList);
            var mockDtoReturnStatusOk = new Status<PagedList<DocumentDto>>(mockPagedListDto);

            var dummyQueryParameters = new DocumentsQueryParameters();
            var dummyPagedListMetadata = new PagedListMetadata();

            documentServiceMock.Setup(method => method.ListDocumentsAsync(dummyQueryParameters))
                               .ReturnsAsync(mocServiceReturnStatusOk);

            mapperMock.Setup(m => m.Map<PagedList<Document>, PagedList<DocumentDto>>(
                             It.IsAny<PagedList<Document>>())).Returns(new PagedList<DocumentDto>(mockListDto, dummyPagedListMetadata));
            #endregion

            #region act
            DocumentsApiController documentController = new DocumentsApiController(documentServiceMock.Object,
                                                                                   paginationUriBuilderMock.Object,
                                                                                   mapperMock.Object,
                                                                                   loggerMock.Object)
            {
                ControllerContext = controllerContext
            };

            var response = await documentController.GetDocumentsAsync(dummyQueryParameters);
            #endregion

            #region assert
            Assert.IsType<NoContentResult>(response.Result);
            Assert.True(documentController.Response.Headers["X-Pagination"].Count == 0);
            #endregion  
        }    }  
}