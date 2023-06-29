using Dotnet_RPG.Dtos.Fight;
using Dotnet_RPG.Services.Fights;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet_RPG.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FightController : Controller
    {
        private readonly IFightsService _fightService;

        public FightController(IFightsService fightService)
        {
            _fightService = fightService;
        }

        [HttpPost("Attack")]
        public async Task<ActionResult<ServiceResponse<AttackResultDto>>> Attack(AttackDto Attack)
        {
            var response = await _fightService.Attack(Attack);
            
            if (response.Success == false)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<FightResultsDto>>> Fight(FightRequestDto fight)
        {
            var response = await _fightService.Fight(fight);

            if (response.Success == false)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<HighScoreDto>>>> GetHighScore()
        {
            var response = await _fightService.GetHighScore();

            if (response.Success == false)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
