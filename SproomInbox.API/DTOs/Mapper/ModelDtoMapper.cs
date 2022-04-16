using AutoMapper;
using SproomInbox.API.Domain.Models;
using SproomInbox.API.Utils.Extensions;
using SproomInbox.API.Utils.Paging;

namespace SproomInbox.API.DTOs.Mapper
{
    public class ModelDtoMapper : Profile
    {
        public ModelDtoMapper()
        {
            CreateMap<User, UserDto>();
   
            CreateMap<Document, DocumentDto>()
                  .ForMember(dest => dest.State,
                             opt => opt.MapFrom(src => src.StateId.ToDescriptionString()))
                  .ForMember(dest => dest.Type,
                             opt => opt.MapFrom(src => src.TypeId.ToDescriptionString()));

            CreateMap<DocumentState, DocumentStateDto>()
                  .ForMember(dest => dest.State,
                             opt => opt.MapFrom(src => src.StateId.ToDescriptionString()));
   
            CreateMap(typeof(PagedList<Document>), typeof(PagedList<DocumentDto>)).ConvertUsing(typeof(PagedListMapper<Document, DocumentDto>));  
        }  
    }
}
