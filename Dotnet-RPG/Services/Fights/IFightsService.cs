using Dotnet_RPG.Dtos.Fight;

namespace Dotnet_RPG.Services.Fights
{
    public interface IFightsService
    {
        Task<ServiceResponse<AttackResultDto>> Attack(AttackDto AttackDto);
        Task<ServiceResponse<FightResultsDto>> Fight(FightRequestDto fight);
        Task<ServiceResponse<List<HighScoreDto>>> GetHighScore();
    }
}
