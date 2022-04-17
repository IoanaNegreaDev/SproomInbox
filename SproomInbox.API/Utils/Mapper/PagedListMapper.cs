using AutoMapper;
using SproomInbox.API.Utils.Paging;

namespace SproomInbox.API.Utils.Mapper
{
    public class PagedListMapper<TSource, TDestination> : ITypeConverter<PagedList<TSource>, PagedList<TDestination>>
    {
        public PagedList<TDestination> Convert(PagedList<TSource> sourcePagedList,
                                               PagedList<TDestination> destinationPagedList,
                                               ResolutionContext context)
        {
            var mappedBasicList = context.Mapper.Map<List<TSource>, List<TDestination>>(sourcePagedList);
            var dtoPagedList = new PagedList<TDestination>(mappedBasicList,
                                                           sourcePagedList.PagedMetadata);
            return dtoPagedList;
        }
    }
}
