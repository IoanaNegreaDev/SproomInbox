using Microsoft.AspNetCore.Mvc;
using SproomInbox.API.Parametrization;

namespace SproomInbox.API.Utils.Pagination
{
    public interface IPaginationUriBuilder
    {
        PagedListMetadata BuildPaginationMetadata<T>(IUrlHelper urlHelper, string routeName, DocumentsQueryParameters queryParameters, PagedList<T> pagedList);
    }
}