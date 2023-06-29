using AutoMapper;
using Dotnet_RPG.Data;
using Dotnet_RPG.Dtos.Character;
using Dotnet_RPG.Dtos.Fight;
using Dotnet_RPG.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;
using static System.Net.Mime.MediaTypeNames;

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
                    string skill = string.Empty;
                    resultDto.Damage = CalculateDamage(attacker, opponent, out skill, AttackDto.SkillId);
                    resultDto.SkillUsed = skill;
                }
                else
                {
                    resultDto.Damage = CalculateDamage(attacker, opponent);
                    resultDto.WeaponUsed = attacker.Weapon == null ? "Bare Hands" : attacker.Weapon.Name;
                }
                opponent.HitPoints -= resultDto.Damage > 0 ? resultDto.Damage : 0;
                resultDto.AttackerHP = attacker.HitPoints;
                resultDto.OpponentHP = opponent.HitPoints;

                // If enemy defeated, set defeated to true. Increments and decrements character stats
                if (opponent.HitPoints <= 0)
                {
                    attacker.Victories += 1;
                    opponent.Defeats += 1;

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

        public async Task<ServiceResponse<FightResultsDto>> Fight(FightRequestDto fight)
        {
            var response = new ServiceResponse<FightResultsDto>
            {
                Data = new FightResultsDto()
            };
            
            try
            {
                var characters = await _context.Characters
                    .Include(c => c.Weapon)
                    .Include(c => c.Skills)
                    .Where(c => fight.CharacterIds.Contains(c.Id))
                    .ToListAsync();

                bool defeated = false;
                while (!defeated)
                {
                    foreach (var attacker in characters)
                    {
                        var opponents = characters.Where(c => c.Id != attacker.Id).ToList();
                        var opponent = opponents[new Random().Next(opponents.Count)];

                        int damage = 0;
                        string attackUsed = string.Empty;
                        bool useWeapon = new Random().Next(2) == 0;

                        if (useWeapon)
                        {
                            damage = CalculateDamage(attacker, opponent);
                            opponent.HitPoints -= damage > 0 ? damage : 0;

                            response.Data.Log
                                .Add($"{attacker.Name} dealt {damage} to {opponent.Name} " +
                                $"using {(attacker.Weapon is null ? "Bare Hands" : attacker.Weapon.Name)}");
                        }
                        else if (!useWeapon && attacker.Skills is not null)
                        {
                            int skillId = attacker.Skills[new Random().Next(attacker.Skills.Count)].Id;
                            string skillUsed = string.Empty;
                            damage = CalculateDamage(attacker, opponent, out skillUsed, skillId);
                            opponent.HitPoints -= damage > 0 ? damage : 0;

                            response.Data.Log
                                .Add($"{attacker.Name} dealt {damage} to {opponent.Name} " +
                                $"using {skillUsed}");
                        }
                        else
                        {
                            response.Data.Log
                                .Add($"{attacker.Name} was unable to attack.");
                            continue;
                        }

                        if (opponent.HitPoints <= 0)
                        {
                            attacker.Victories += 1;
                            opponent.Defeats += 1;
                            defeated = true;
                            response.Data.Log.Add($"{opponent.Name} has been defeated!");
                            response.Data.Log.Add($"{attacker.Name} wins with {attacker.HitPoints} HP remaining!");
                            break;
                        }
                    }
                }

                characters.ForEach(c =>
                {
                    c.Fights++;
                    c.HitPoints = 100;
                });

                await _context.SaveChangesAsync();
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }

        }

        public async Task<ServiceResponse<List<HighScoreDto>>> GetHighScore()
        {
            var characters = await _context.Characters
                .Where(c => c.Fights > 0)
                .OrderByDescending(c => c.Victories)
                .ThenBy(c => c.Defeats)
                .ToListAsync();

            var response = new ServiceResponse<List<HighScoreDto>>();
            response.Data = characters.Select(c => _mapper.Map<HighScoreDto>(c)).ToList();

            return response;
        }

        private int CalculateDamage(Character attacker, Character opponent)
        {
            int damage = 0;
            
            if (attacker.Weapon != null)
            {
                
                damage = attacker.Weapon!.Damage - opponent.Defense;
            }
            else
            {
                damage = 1 - opponent.Defense;
            }

            return damage;
        }

        private int CalculateDamage(Character attacker, Character opponent, out string skillName, int skillId)
        {
            int damage = 0;
            
            if (attacker.Skills is null)
            {
                throw new Exception($"{attacker.Name} does not know any skills.");
            }
            
            var skill = attacker.Skills!.FirstOrDefault(s => s.Id == skillId);

            if (skill != null)
            {
                damage = skill.Damage + attacker.Intelligence - opponent.Defense;
                skillName = skill.Name;
            }
            else
            {
                throw new Exception($"{attacker.Name} does not know that skill.");
            }

            return damage;
        }
    }
}
