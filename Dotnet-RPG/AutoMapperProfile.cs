using AutoMapper;
using Dotnet_RPG.Dtos.Character;

namespace Dotnet_RPG
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<Character, GetCharacterDto>(); 
            CreateMap<AddCharacterDto, Character>();
            CreateMap<UpdateCharacterDto, Character>();
        }
    }
}
