using dotnet7_rpg.Dtos.Weapon;
using dotnet7_rpg.Services.WeaponService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet7_rpg.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class WeaponController : ControllerBase
{
    private readonly IWeaponService _weaponService;

    public WeaponController(IWeaponService weaponService)
    {
        _weaponService = weaponService;
    }


    [HttpPost("AddWeapon")]
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> AddWeapon(AddWeaponDto newWeapon)
    {
        var response = await _weaponService.AddWeapon(newWeapon);
        if (response.Data == null)
        {
            return NotFound(response);
        }
        return Ok(response); 
    }
    
}