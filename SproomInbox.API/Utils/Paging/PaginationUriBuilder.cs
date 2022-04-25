using Microsoft.AspNetCore.Mvc;
using SproomInbox.WebApp.Shared.Pagination;
using SproomInbox.WebApp.Shared.Resources.Parametrization;
namespace SproomInbox.API.Utils.Paging
{
    public class PaginationUriBuilder : IPaginationUriBuilder
    {
        public PagedListMetadata BuildPaginationMetadata<T>(IUrlHelper urlHelper, 
                                                string routeName, 
                                                DocumentsQueryParameters queryParameters,
                                                PagedList<T> pagedList)
        {
            var previousPageLink = string.Empty;
            var nextPageLink = string.Empty;

            previousPageLink = pagedList.PagedMetadata.HasPrevious ?
                CreateDocumentsResourceUri(urlHelper,
                                        routeName,
                                        queryParameters,
                                        PageNavigation.PreviousPage)
                 : string.Empty;

            nextPageLink = pagedList.PagedMetadata.HasNext ?
                CreateDocumentsResourceUri(urlHelper,
                                         routeName,
                                        queryParameters,
                                        PageNavigation.NextPage)
                 : string.Empty;

            var paginationMetadata = new PagedListMetadata
            {
                TotalCount = pagedList.PagedMetadata.TotalCount,
                Size = pagedList.PagedMetadata.Size,
                Current = pagedList.PagedMetadata.Current,
                TotalPages = pagedList.PagedMetadata.TotalPages,
                PreviousPageLink = previousPageLink??string.Empty,
                NextPageLink = nextPageLink??string.Empty
            };

            return paginationMetadata;
        }

        private string? CreateDocumentsResourceUri(IUrlHelper urlHelper, 
                                                  string routeName,
                                                  DocumentsQueryParameters queryParameters,
                                                  PageNavigation pageNavigationType)
        { 

            switch (pageNavigationType)
            {
                case PageNavigation.PreviousPage:
                    return urlHelper.Link(routeName,
                        new
                        {
                            pageNumber = queryParameters.CurrentPage - 1,
                            pageSize = queryParameters.PageSize,
                            searchQuery = queryParameters.Search
                         //  fields = queryParameters.Fields
                        });
                case PageNavigation.NextPage:
                    return urlHelper.Link(routeName,
                        new
                        {                           
                            pageNumber = queryParameters.CurrentPage + 1,
                            pageSize = queryParameters.PageSize,                         
                            searchQuery = queryParameters.Search
                        //    fields = queryParameters.Fields
                        });
                case PageNavigation.Current:
                default:
                    return urlHelper.Link(routeName,
                        new
                        {                           
                            pageNumber = queryParameters.CurrentPage,
                            pageSize = queryParameters.PageSize,                          
                            searchQuery = queryParameters.Search,
                       //     fields = queryParameters.Fields
                        });
            };

        }
    }
}
