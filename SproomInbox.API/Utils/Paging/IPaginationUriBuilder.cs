using Microsoft.AspNetCore.Mvc;
using SproomInbox.WebApp.Shared.Pagination;
using SproomInbox.WebApp.Shared.Resources.Parametrization;

namespace SproomInbox.API.Utils.Paging
{
    public interface IPaginationUriBuilder
    {
        PagedListMetadata BuildPaginationMetadata<T>(IUrlHelper urlHelper, string routeName, DocumentsQueryParameters queryParameters, PagedList<T> pagedList);
    }
}