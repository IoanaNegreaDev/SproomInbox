using Microsoft.AspNetCore.Mvc;
using SproomInbox.API.Utils.Paging;
using SproomInbox.WebApp.Shared.Resources.Parametrization;

namespace SproomInbox.API.Utils.Paging
{
    public interface IPaginationUriBuilder
    {
        object BuildPaginationMetadata<T>(IUrlHelper urlHelper, string routeName, DocumentListQueryParameters queryParameters, PagedList<T> pagedList);
    }
}