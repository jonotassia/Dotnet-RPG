using AutoMapper;
using Dotnet_RPG.Dtos.Character;
using Dotnet_RPG.Dtos.Fight;
using Dotnet_RPG.Dtos.Skill;
using Dotnet_RPG.Dtos.Weapon;

namespace Dotnet_RPG
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<Character, GetCharacterDto>();
            CreateMap<AddCharacterDto, Character>();
            CreateMap<AddCharacterSkillDto, Character>();
            CreateMap<AddWeaponDto, Weapon>();
            CreateMap<Weapon, GetWeaponDto>();
            CreateMap<AddSkillDto, Skill>();
            CreateMap<Skill, GetSkillDto>();
            CreateMap<Character, HighScoreDto>();
        }
    }
}
