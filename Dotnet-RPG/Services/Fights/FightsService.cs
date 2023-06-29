using AutoMapper;
using Dotnet_RPG.Data;
using Dotnet_RPG.Dtos.Character;
using Dotnet_RPG.Dtos.Fight;
using Dotnet_RPG.Models;
using Microsoft.EntityFrameworkCore;

namespace Dotnet_RPG.Services.Fights
{
    public class FightsService : IFightsService
    {
        private readonly DataContext _context;
        private readonly ICharacterService _characterService;
        private readonly IMapper _mapper;

        public FightsService(IMapper mapper, DataContext context, ICharacterService characterService)
        {
            _context = context;
            _characterService = characterService;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<AttackResultDto>> Attack(AttackDto AttackDto)
        {
            var serviceResponse = new ServiceResponse<AttackResultDto>();

            try
            {
                // Get attacker and opponent, then convert to Character object rather than GetCharacterDto
                var attacker = await _context.Characters
                    .Include(c => c.Weapon)
                    .Include(c => c.Skills)
                    .FirstOrDefaultAsync(c => c.Id == AttackDto.AttackerId); 
                var opponent = await _context.Characters
                    .Include(c => c.Weapon)
                    .Include(c => c.Skills)
                    .FirstOrDefaultAsync(c => c.Id == AttackDto.OpponentId);

                if (attacker is null || opponent is null)
                {
                    throw new Exception("Character does not exist.");
                }

                // Increment fights counter
                attacker.Fights += 1;
                opponent.Fights += 1;

                // Create result Dto
                var resultDto = new AttackResultDto();
                resultDto.Attacker = attacker.Name;
                resultDto.Opponent = opponent.Name;

                // Calculate damage and hitpoints
                if (AttackDto.SkillId != 0)
                {
                    CalculateDamage(ref attacker, ref opponent, ref resultDto, AttackDto.SkillId);
                }
                else
                {
                    CalculateDamage(ref attacker, ref opponent, ref resultDto);
                }
                

                // If enemy defeated, set defeated to true. Increments and decrements character stats
                if (EnemyDefeated(ref attacker, ref opponent))
                {
                    resultDto.Defeated = true;
                    serviceResponse.Message = $"{opponent.Name} has been defeated!";
                }

                // Save changes back to database
                await _context.SaveChangesAsync();

                serviceResponse.Data = resultDto;
                serviceResponse.Success = true;
                return serviceResponse;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                return serviceResponse;
            }
        }

        private bool EnemyDefeated(ref Character attacker, ref Character opponent)
        {
            if (opponent.HitPoints <= 0)
            {
                attacker.Victories += 1;
                opponent.Defeats += 1;
                return true;
            }
            return false;
        }

        private void CalculateDamage(ref Character attacker, ref Character opponent, ref AttackResultDto resultDto)
        {
            if (attacker.Weapon != null)
            {
                resultDto.WeaponUsed = attacker.Weapon.Name;
                resultDto.Damage = attacker.Weapon!.Damage - opponent.Defense;
                opponent.HitPoints -= resultDto.Damage > 0 ? resultDto.Damage : 0;
            }
            else
            {
                resultDto.WeaponUsed = "Bare Hands";
                resultDto.Damage = 1 - opponent.Defense;
                opponent.HitPoints -= resultDto.Damage > 0 ? resultDto.Damage : 0;
            }
            resultDto.AttackerHP = attacker.HitPoints;
            resultDto.OpponentHP = opponent.HitPoints;
        }

        private void CalculateDamage(ref Character attacker, ref Character opponent, ref AttackResultDto resultDto, int skillId)
        {
            if (attacker.Skills is null)
            {
                throw new Exception($"{attacker.Name} does not know any skills.");
            }
            
            var skill = attacker.Skills!.FirstOrDefault(s => s.Id == skillId);

            if (skill != null)
            {
                resultDto.SkillUsed = skill.Name;
                resultDto.Damage = skill.Damage + attacker.Intelligence - opponent.Defense;
                opponent.HitPoints -= resultDto.Damage > 0 ? resultDto.Damage : 0;
            }
            else
            {
                throw new Exception($"{attacker.Name} does not know that skill.");
            }

            resultDto.AttackerHP = attacker.HitPoints;
            resultDto.OpponentHP = opponent.HitPoints;
        }

    }
}
