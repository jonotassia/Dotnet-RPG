using Dotnet_RPG.Dtos.Character;
using Dotnet_RPG.Dtos.Weapon;

namespace Dotnet_RPG.Services.WeaponService
{
    public interface IWeaponService
    {
        Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto weapon);
    }
}
