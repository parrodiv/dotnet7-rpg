using dotnet7_rpg.Dtos.Fight;
using Microsoft.AspNetCore.Mvc;

namespace dotnet7_rpg.Controllers;


[ApiController]
[Route("[controller]")]
public class FightController : ControllerBase
{
    private readonly IFightService _fightService;

    public FightController(IFightService fightService)
    {
        _fightService = fightService;
    }
    
    [HttpPost()]
    public async Task<ActionResult<ServiceResponse<AttackResultDto>>> WeaponAttack(WeaponAttackDto request)
    {
        return Ok(await _fightService.WeaponAttack(request));
    }
}