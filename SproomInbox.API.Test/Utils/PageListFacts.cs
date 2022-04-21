using SproomInbox.API.Utils.Paging;
using SproomInbox.WebApp.Shared.Resources.Parametrization.Paging;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SproomInbox.API.Test.Utils
{
    public class MockEntity
    {
        public int Id;
    }
    public class PageListFacts
    {
        [Fact]
        public async void Create_Returns_PageList_From_InputList_ValidInput()
        {
           var testList =new List<MockEntity>()
            {
                new MockEntity() { Id = 1 },
                new MockEntity() { Id = 2 },
                new MockEntity() { Id = 3 },
                new MockEntity() { Id = 4 },
                new MockEntity() { Id = 5 },
           }.AsQueryable();
       
            PagedListMetadata pagedListDataTest = new PagedListMetadata() { Current = 1, Size = 2 };
            var returnedPagedList = await PagedList<MockEntity>.Create(testList, pagedListDataTest);
            Assert.NotNull(returnedPagedList);
            Assert.True(returnedPagedList.Count == 2);
            Assert.True(returnedPagedList[0].Id == 1 && returnedPagedList[1].Id == 2);

            pagedListDataTest = new PagedListMetadata() { Current = 2, Size = 2 };
            returnedPagedList = await PagedList<MockEntity>.Create(testList, pagedListDataTest);
            Assert.NotNull(returnedPagedList);
            Assert.True(returnedPagedList.Count == 2);
            Assert.True(returnedPagedList[0].Id == 3 && returnedPagedList[1].Id == 4);

            pagedListDataTest = new PagedListMetadata() { Current = 2, Size = 4 };
            returnedPagedList = await PagedList<MockEntity>.Create(testList, pagedListDataTest);
            Assert.NotNull(returnedPagedList);
            Assert.True(returnedPagedList.Count == 1);
            Assert.True(returnedPagedList[0].Id == 5);
        }

        [Fact]
        public async void Create_Returns_PageList_From_InputList_InvalidPageInfo()
        {
            var testList = new List<MockEntity>()
            {
                new MockEntity() { Id = 1 },
                new MockEntity() { Id = 2 },
                new MockEntity() { Id = 3 },
                new MockEntity() { Id = 4 },
                new MockEntity() { Id = 5 },
           }.AsQueryable();

            PagedListMetadata pagedListDataTest = new PagedListMetadata() { Current = 0, Size = 2 };
            var returnedPagedList = await PagedList<MockEntity>.Create(testList, pagedListDataTest);
            Assert.NotNull(returnedPagedList);
            Assert.True(returnedPagedList.Count == 2);
            Assert.True(returnedPagedList[0].Id == 1 && returnedPagedList[1].Id == 2);

            pagedListDataTest = new PagedListMetadata() { Current = 4, Size = 6 };
            returnedPagedList = await PagedList<MockEntity>.Create(testList, pagedListDataTest);
            Assert.NotNull(returnedPagedList);
            Assert.True(returnedPagedList.Count == 0);

            pagedListDataTest = new PagedListMetadata() { Current = 9, Size = 9 };
            returnedPagedList = await PagedList<MockEntity>.Create(testList, pagedListDataTest);
            Assert.NotNull(returnedPagedList);
            Assert.True(returnedPagedList.Count == 0);

            pagedListDataTest = new PagedListMetadata() { Current = 9, Size = 0 };
            returnedPagedList = await PagedList<MockEntity>.Create(testList, pagedListDataTest);
            Assert.NotNull(returnedPagedList);
            Assert.True(returnedPagedList.Count == 0);
        }

        [Fact]
        public async void Create_Returns_PageList_From_InputList_NUllOr0QueryableList()
        {
            var emptyTestList = new List<MockEntity>()
            {          
            }.AsQueryable(); 

            PagedListMetadata pagedListDataTest = new PagedListMetadata() { Current = 0, Size = 2 };
            var returnedPagedList = await PagedList<MockEntity>.Create(null, pagedListDataTest);
            Assert.Null(returnedPagedList);
      
            pagedListDataTest = new PagedListMetadata() { Current = 4, Size = 6 };
            returnedPagedList = await PagedList<MockEntity>.Create(emptyTestList, pagedListDataTest);
            Assert.NotNull(returnedPagedList);
            Assert.True(returnedPagedList.Count == 0);          
        }
    }
}
