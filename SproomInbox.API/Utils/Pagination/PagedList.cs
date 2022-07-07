
namespace SproomInbox.API.Utils.Pagination
{
    public class PagedList<T> : List<T>
    {
        public PagedListMetadata PagedMetadata { get; private set; }

        public PagedList(List<T> items, PagedListMetadata pagedMetadata)
        {
            PagedMetadata = pagedMetadata;
            AddRange(items);
        }

         public static PagedList<T> Create(IQueryable<T> source, PagedListMetadata pagedMetadata)
         {      
              if (source == null || pagedMetadata == null)
                  return new PagedList<T>(new List<T>(), new PagedListMetadata());

              var items = source.Skip((pagedMetadata.Current - 1) * pagedMetadata.Size).Take(pagedMetadata.Size)
                                      .ToList();

              pagedMetadata.TotalCount = source.Count();
              pagedMetadata.TotalPages = (int)Math.Ceiling(pagedMetadata.TotalCount / (double)pagedMetadata.Size);
              return new PagedList<T>(items, pagedMetadata);
          }
    }
}
   