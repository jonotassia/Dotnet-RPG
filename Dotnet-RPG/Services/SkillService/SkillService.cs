using AutoMapper;
using Dotnet_RPG.Data;
using Dotnet_RPG.Dtos.Character;
using Dotnet_RPG.Dtos.Skill;
using Microsoft.EntityFrameworkCore;

namespace Dotnet_RPG.Services.SkillService
{
    public class SkillService : ISkillService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public SkillService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ServiceResponse<GetSkillDto>> AddSkill(AddSkillDto newSkill)
        {
            var serviceResponse = new ServiceResponse<GetSkillDto>();

            try
            {
                var skill = await _context.Skills.FirstOrDefaultAsync(s => s.Name == newSkill.Name);
                
                if (skill != null)
                {
                    serviceResponse.Message = "Skill already exists.";
                    serviceResponse.Success = false;
                    return serviceResponse;
                }

                skill = _mapper.Map(newSkill, skill);
                _context.Skills.Add(skill!);
                await _context.SaveChangesAsync();

                serviceResponse.Success = true;
                serviceResponse.Data = _mapper.Map<GetSkillDto>(skill);
                return serviceResponse;
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.Message;
                serviceResponse.Success = false;
                return serviceResponse;
            }
        }
    }
}
