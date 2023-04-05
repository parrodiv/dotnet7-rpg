using dotnet7_rpg.Dtos.Weapon;

namespace dotnet7_rpg.Services.WeaponService;

public interface IWeaponService
{
    Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon);
}