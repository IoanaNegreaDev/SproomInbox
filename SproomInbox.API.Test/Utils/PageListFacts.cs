using SproomInbox.API.Utils.Pagination;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SproomInbox.API.Test.Utils
{
    public class MockEntity
    {
        public int Id;
    }
    // TO DO split the tests for a better localization of errors + ADD PagedListMetadata tests
    public class PageListFacts
    {
        [Fact]
        public void Create_ValidInput_Returns_PageList()
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
            var returnedPagedList =  PagedList<MockEntity>.Create(testList, pagedListDataTest);
            Assert.NotNull(returnedPagedList);
            Assert.True(returnedPagedList.Count == 2);
            Assert.True(returnedPagedList[0].Id == 1 && returnedPagedList[1].Id == 2);

            pagedListDataTest = new PagedListMetadata() { Current = 2, Size = 2 };
            returnedPagedList =  PagedList<MockEntity>.Create(testList, pagedListDataTest);
            Assert.NotNull(returnedPagedList);
            Assert.True(returnedPagedList.Count == 2);
            Assert.True(returnedPagedList[0].Id == 3 && returnedPagedList[1].Id == 4);

            pagedListDataTest = new PagedListMetadata() { Current = 2, Size = 4 };
            returnedPagedList =  PagedList<MockEntity>.Create(testList, pagedListDataTest);
            Assert.NotNull(returnedPagedList);
            Assert.True(returnedPagedList.Count == 1);
            Assert.True(returnedPagedList[0].Id == 5);
        }

        [Fact]
        public void Create_InvalidPageMetadataValues_Returns_PageList()
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
            var returnedPagedList =  PagedList<MockEntity>.Create(testList, pagedListDataTest);
            Assert.NotNull(returnedPagedList);
            Assert.True(returnedPagedList.Count == 2);
            Assert.True(returnedPagedList[0].Id == 1 && returnedPagedList[1].Id == 2);

            pagedListDataTest = new PagedListMetadata() { Current = 4, Size = 6 };
            returnedPagedList =  PagedList<MockEntity>.Create(testList, pagedListDataTest);
            Assert.NotNull(returnedPagedList);
            Assert.True(returnedPagedList.Count == 0);

            pagedListDataTest = new PagedListMetadata() { Current = 9, Size = 9 };
            returnedPagedList =  PagedList<MockEntity>.Create(testList, pagedListDataTest);
            Assert.NotNull(returnedPagedList);
            Assert.True(returnedPagedList.Count == 0);

            pagedListDataTest = new PagedListMetadata() { Current = 9, Size = 0 };
            returnedPagedList =  PagedList<MockEntity>.Create(testList, pagedListDataTest);
            Assert.NotNull(returnedPagedList);
            Assert.True(returnedPagedList.Count == 0);
        }

        [Fact]
        public void Create_InputList_Null_Or_0_Returns_EmptyPageList()
        {
            var emptyTestList = new List<MockEntity>()
            {          
            }.AsQueryable(); 

            PagedListMetadata pagedListDataTest = new PagedListMetadata() { Current = 0, Size = 2 }; 
             var returnedPagedList =  PagedList<MockEntity>.Create(null, pagedListDataTest);
            Assert.NotNull(returnedPagedList);
      
            pagedListDataTest = new PagedListMetadata() { Current = 4, Size = 6 };
            returnedPagedList =  PagedList<MockEntity>.Create(emptyTestList, pagedListDataTest);
            Assert.NotNull(returnedPagedList);
            Assert.True(returnedPagedList.Count == 0);          
        }
    }

    // etc etc....
}
