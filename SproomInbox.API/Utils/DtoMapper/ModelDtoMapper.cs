using AutoMapper;
using SproomInbox.API.Domain.Models;
using SproomInbox.API.Utils.Paging;
using SproomInbox.WebApp.Shared.Resources;

namespace SproomInbox.API.Utils.DtoMapper
{
    public class ModelDtoMapper : Profile
    {
        public ModelDtoMapper()
        {
            CreateMap<User, UserDto>();

            CreateMap<Document, DocumentDto>()
                  .ForMember(dest => dest.State,
                             opt => opt.MapFrom(src => Enum.GetName(typeof(State), src.StateId)))
                  .ForMember(dest => dest.Type,
                             opt => opt.MapFrom(src => src.TypeId.ToString()));

            CreateMap<DocumentState, DocumentStateDto>()
                  .ForMember(dest => dest.State,
                             opt => opt.MapFrom(src => Enum.GetName(typeof(State), src.StateId)));

            CreateMap(typeof(PagedList<Document>), typeof(PagedList<DocumentDto>)).ConvertUsing(typeof(PagedListMapper<Document, DocumentDto>));
        }  
    }
}
