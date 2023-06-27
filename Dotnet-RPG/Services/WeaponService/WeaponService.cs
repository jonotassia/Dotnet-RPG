using AutoMapper;
using Dotnet_RPG.Data;
using Dotnet_RPG.Dtos.Character;
using Dotnet_RPG.Dtos.Weapon;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Dotnet_RPG.Services.WeaponService
{
    public class WeaponService : IWeaponService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public WeaponService(IMapper mapper, DataContext context, IHttpContextAccessor httpContext)
        {
            _mapper = mapper;
            _context = context;
            _httpContext = httpContext;
        }

        public async Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();

            try
            {
                // Get first character based on requested character ID in weapon request
                // and correct user logged in
                var character = await _context.Characters
                    .FirstOrDefaultAsync(c => c.Id == newWeapon.CharacterId && 
                        c.User!.Id == int.Parse(_httpContext.HttpContext!
                            .User.FindFirstValue(ClaimTypes.NameIdentifier)!));

                if (character == null)
                {
                    throw new NullReferenceException("This character does not exist.");
                }

                // Map values of new weapon to a new weapon, then add to DB and save
                var weapon = _mapper.Map<Weapon>(newWeapon);
                _context.Weapons.Add(weapon);
                await _context.SaveChangesAsync();

                serviceResponse.Success = true;
                serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);

            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                return serviceResponse;
            }

            return serviceResponse;
        }
    }
}
