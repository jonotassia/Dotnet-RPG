using Dotnet_RPG.Dtos.Skill;
using Dotnet_RPG.Services.SkillService;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet_RPG.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SkillController : ControllerBase
    {
        private readonly ISkillService _skillService;

        public SkillController(ISkillService skillService)
        {
            _skillService = skillService;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<GetSkillDto>>> AddSkill(AddSkillDto newSkill)
        {
            var response = await _skillService.AddSkill(newSkill);

            if (response.Success == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
