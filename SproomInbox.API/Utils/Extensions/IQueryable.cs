using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace SproomInbox.API.Utils.Extensions
{
    public static class IQueryableExtensions
    {
        // temporary HACK!!!!!!!!!!!!!!! - until I figure out how to call IQueryable ToListAsync during unit testing
        public static Task<List<TSource>> ToListAsyncSafe<TSource>(this IQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (!(source is IDbAsyncEnumerable<TSource>))
                return Task.FromResult(source.ToList());
            return source.ToListAsync();
        }
    }
}
