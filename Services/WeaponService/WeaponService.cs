using System.ComponentModel.Design;
using System.Security.Claims;
using dotnet7_rpg.Dtos.Weapon;

namespace dotnet7_rpg.Services.WeaponService;

public class WeaponService : IWeaponService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WeaponService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }
    
    
    private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User
        .FindFirstValue(ClaimTypes.NameIdentifier)!);
    
    
    public async Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon)
    {
        var response = new ServiceResponse<GetCharacterDto>();
        try
        {   
            // check if the character belongs to the current authenticated user
            var character = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == newWeapon.CharacterId &&
                                          c.User!.Id == GetUserId());
            
            if (character is null) throw new Exception("Character not found");

                var weapon = _mapper.Map<Weapon>(newWeapon); // it will contains Name, Damage, CharcaterId
            // add the Character object to the Character property of weapon
            weapon.Character = character;

            _context.Weapons.Add(weapon);
            await _context.SaveChangesAsync();

            response.Data = _mapper.Map<GetCharacterDto>(character);
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
        }

        return response;
    }

}