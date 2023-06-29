using Dotnet_RPG.Dtos.Fight;

namespace Dotnet_RPG.Services.Fights
{
    public interface IFightsService
    {
        Task<ServiceResponse<AttackResultDto>> Attack(AttackDto AttackDto);
    }
}
