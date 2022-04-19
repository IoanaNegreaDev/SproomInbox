﻿using SproomInbox.WebApp.Shared.Resources.Parametrization;

namespace SproomInbox.WebApp.Server.Services
{
    public interface IDocumentsFromApiService
    {
        Task<HttpResponseMessage> FetchDocumentsAsync(DocumentListQueryParameters queryParameters);
        Task<HttpResponseMessage> UpdateDocumentsAsync(DocumentListStatusUpdateParameters updateParameters);
    }
}