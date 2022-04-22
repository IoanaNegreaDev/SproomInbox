﻿using Microsoft.AspNetCore.Mvc;
using SproomInbox.WebApp.Shared.Resources.Parametrization;
namespace SproomInbox.API.Utils.Paging
{
    public class PaginationUriBuilder : IPaginationUriBuilder
    {
        public object BuildPaginationMetadata<T>(IUrlHelper urlHelper, 
                                                string routeName, 
                                                DocumentsQueryParameters queryParameters,
                                                PagedList<T> pagedList)
        {

            var previousPageLink = pagedList.HasPrevious ?
                CreateDocumentsResourceUri(urlHelper,
                                        routeName,
                                        queryParameters,
                                        PageNavigation.PreviousPage)
                 : null;

            var nextPageLink = pagedList.HasNext ?
                CreateDocumentsResourceUri(urlHelper,
                                         routeName,
                                        queryParameters,
                                        PageNavigation.NextPage)
                 : null;

            var paginationMetadata = new
            {
                totalCount = pagedList.TotalCount,
                pageSize = pagedList.PagedMetadata.Size,
                currentPage = pagedList.PagedMetadata.Current,
                totalPages = pagedList.TotalPages,
                previousPageLink,
                nextPageLink
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
                            searchQuery = queryParameters.Search,
                            fields = queryParameters.Fields
                        });
                case PageNavigation.NextPage:
                    return urlHelper.Link(routeName,
                        new
                        {                           
                            pageNumber = queryParameters.Page.Current + 1,
                            pageSize = queryParameters.Page.Size,                         
                            searchQuery = queryParameters.Search,
                            fields = queryParameters.Fields
                        });
                case PageNavigation.Current:
                default:
                    return urlHelper.Link(routeName,
                        new
                        {                           
                            pageNumber = queryParameters.Page.Current,
                            pageSize = queryParameters.Page.Size,                          
                            searchQuery = queryParameters.Search,
                            fields = queryParameters.Fields
                        });
            };

        }
    }
}
