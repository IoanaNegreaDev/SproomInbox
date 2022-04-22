using Microsoft.AspNetCore.Mvc;
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

            var previousPageLink = pagedList.PagedMetadata.HasPrevious ?
                CreateDocumentsResourceUri(urlHelper,
                                        routeName,
                                        queryParameters,
                                        PageNavigation.PreviousPage)
                 : string.Empty;

            var nextPageLink = pagedList.PagedMetadata.HasNext ?
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
                PreviousPageLink = previousPageLink,
                NextPageLink = nextPageLink
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
                            pageNumber = queryParameters.Page.Current - 1,
                            pageSize = queryParameters.Page.Size,
                            searchQuery = queryParameters.Search
                         //  fields = queryParameters.Fields
                        });
                case PageNavigation.NextPage:
                    return urlHelper.Link(routeName,
                        new
                        {                           
                            pageNumber = queryParameters.Page.Current + 1,
                            pageSize = queryParameters.Page.Size,                         
                            searchQuery = queryParameters.Search
                        //    fields = queryParameters.Fields
                        });
                case PageNavigation.Current:
                default:
                    return urlHelper.Link(routeName,
                        new
                        {                           
                            pageNumber = queryParameters.Page.Current,
                            pageSize = queryParameters.Page.Size,                          
                            searchQuery = queryParameters.Search,
                       //     fields = queryParameters.Fields
                        });
            };

        }
    }
}
