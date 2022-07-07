using AutoMapper;
using SproomInbox.API.Utils.Pagination;


namespace SproomInbox.API.Utils.DtoMapper
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
