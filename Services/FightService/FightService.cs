using dotnet7_rpg.Dtos.Fight;

namespace dotnet7_rpg.Services.FightService;

public class FightService : IFightService
{
    private readonly DataContext _context;

    public FightService(DataContext context)
    {
        _context = context;
    }

    public async Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request)
    {
        var response = new ServiceResponse<AttackResultDto>();
        try
        {
            var attacker = await _context.Characters
                .Include(c => c.Weapon)
                .FirstOrDefaultAsync(c => c.Id == request.AttackerId);
            
            var opponent = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == request.OpponentId);

            if (attacker is null || opponent is null || attacker.Weapon is null)
            {
                throw new Exception("Something fishy is going on here...");
            }
            
            // the formula takes the damage of the Weapon and adds a random value between zero and the strength of the attacker
            int damage = attacker.Weapon.Damage + (new Random().Next(attacker.Strength + 1));
            damage = damage - new Random().Next(opponent.Defense + 1);

            if (damage > 0)
            {
                opponent.HitPoints -= damage;
            }

            if (opponent.HitPoints <= 0)
            {
                opponent.HitPoints = 0;
                response.Message = $"{opponent.Name} has been defeated!";
            }

            response.Data = new AttackResultDto()
            {
                Attacker = attacker.Name,
                Opponent = opponent.Name,
                AttackerHP = attacker.HitPoints,
                OpponentHP = opponent.HitPoints,
                Damage = damage
            };

            await _context.SaveChangesAsync();
            
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
            return response;
        }

        return response;
    }
}