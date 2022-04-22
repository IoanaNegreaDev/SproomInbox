using SproomInbox.API.Utils.Extensions;
using SproomInbox.WebApp.Shared.Resources.Parametrization.Paging;

namespace SproomInbox.API.Utils.Paging
{ 
    public partial class PagedList<T> : List<T>
    {
        public PagedListMetadata PagedMetadata { get; private set; }
        public int TotalPages { get; private set; } = 0;
        public int TotalCount { get; private set; } = 0;
        public bool HasPrevious { get => (PagedMetadata.Current > 1); }
        public bool HasNext { get => (PagedMetadata.Current<TotalPages);}

        public PagedList(List<T> items, PagedListMetadata pagedMetadata)
        {
            PagedMetadata = pagedMetadata;
            TotalCount = items.Count;
            TotalPages = (int)Math.Ceiling(items.Count / (double)PagedMetadata.Size);
            AddRange(items);
        }

        public async static Task<PagedList<T>> Create(IQueryable<T> source, PagedListMetadata pagedMetadata)
        {
            if (source == null || pagedMetadata == null)
                return new PagedList<T>(new List<T>(), new PagedListMetadata());

            var items = await source.Skip((pagedMetadata.Current - 1) * pagedMetadata.Size).Take(pagedMetadata.Size)
                                    .ToListAsyncSafe();

            return new PagedList<T>(items, pagedMetadata);
        }
    }
}
