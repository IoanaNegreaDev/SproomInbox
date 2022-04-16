using System.Collections.Generic;

namespace SproomInbox.API.Utils.Paging
{
    public interface IPagedList<out T> : IPagedList, IEnumerable<T>
    {
        IPagedList GetMetaData();
        T this[int index] { get; }
    }

    public interface IPagedList
    {
        int CurrentPage { get; }
        bool HasNext { get; }
        bool HasPrevious { get; }
        int PageSize { get; }
        int TotalCount { get; }
        int TotalPages { get; }
    }
}