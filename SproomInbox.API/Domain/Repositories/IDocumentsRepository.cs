﻿using SproomInbox.API.Domain.Models;
using SproomInbox.API.Utils.Paging;
using SproomInbox.API.Utils.Parametrization;

namespace SproomInbox.API.Domain.Repositories
{
    public interface IDocumentsRepository
    {
        Task<PagedList<Document>> ListAsync();
        Task<PagedList<Document>> ListAsync(QueryParameters queryParameters);
    }
}