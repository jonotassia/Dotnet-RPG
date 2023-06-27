using Dotnet_RPG.Dtos.Character;
using Dotnet_RPG.Dtos.Skill;

namespace Dotnet_RPG.Services.SkillService
{
    public interface ISkillService
    {
        Task<ServiceResponse<GetSkillDto>> AddSkill(AddSkillDto newSkill);
    }
}
