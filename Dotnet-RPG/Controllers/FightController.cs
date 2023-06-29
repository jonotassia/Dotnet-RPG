﻿using Dotnet_RPG.Dtos.Fight;
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

        [HttpPost("Weapon")]
        public async Task<ActionResult<ServiceResponse<AttackResultDto>>> WeaponAttack(WeaponAttackDto weaponAttack)
        {
            var response = await _fightService.WeaponAttack(weaponAttack);
            
            if (response.Success == false)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
