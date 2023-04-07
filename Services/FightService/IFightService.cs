using dotnet7_rpg.Dtos.Fight;

namespace dotnet7_rpg.Services.FightService;

public interface IFightService
{
    Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request);
}