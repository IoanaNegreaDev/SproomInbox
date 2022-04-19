
using Microsoft.EntityFrameworkCore;
using SproomInbox.WebApp.Shared.Resources.Parametrization.Paging;

namespace SproomInbox.API.Utils.Paging
{ 
    public partial class PagedList<T> : List<T>
    {
        public PagedListMetadata PagedMetadata { get; private set; }
        public int TotalPages { get; private set; } = 0;
        public int TotalCount { get; private set; } = 0;
        public bool HasPrevious { get => (PagedMetadata.CurrentPage > 1); }
        public bool HasNext { get => (PagedMetadata.CurrentPage<TotalPages);}

        public PagedList(List<T> items, PagedListMetadata pagedMetadata)
        {
            PagedMetadata = pagedMetadata;
            TotalCount = items.Count;
            TotalPages = (int)Math.Ceiling(items.Count / (double)PagedMetadata.PageSize);
            AddRange(items);
        }

        public async static Task<PagedList<T>> Create(IQueryable<T> source, PagedListMetadata pagedMetadata)
        {
            var items = await source.Skip((pagedMetadata.CurrentPage - 1) * pagedMetadata.PageSize).Take(pagedMetadata.PageSize)
                                    .ToListAsync();

            return new PagedList<T>(items, pagedMetadata);
        }
    }
}
