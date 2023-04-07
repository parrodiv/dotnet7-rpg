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
            else
            {
                response.Message = $"{attacker.Name} has used {attacker.Weapon.Name} skill!";
            }

            response.Data = new AttackResultDto()
            {
                Attacker = attacker.Name,
                Opponent = opponent.Name,
                AttackerHP = attacker.HitPoints,
                OpponentHP = opponent.HitPoints,
                WeaponName = attacker.Weapon.Name,
                SkillName = "No skill used",
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

    public async Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto request)
    {
        var response = new ServiceResponse<AttackResultDto>();
        try
        {
            var attacker = await _context.Characters
                .Include(c => c.Skills)
                .FirstOrDefaultAsync(
                    c => c.Id == request.AttackerId);

            var opponent = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == request.OpponentId);
            
            if (attacker is null || opponent is null)
            {
                throw new Exception("Something fishy is going on here...");
            }

            var skill = attacker!.Skills!.FirstOrDefault(s => s.Id == request.SkillId) ??
                        throw new Exception($"{attacker.Name} doesn't know skillId {request.SkillId}");

            int damage = skill.Damage + (new Random().Next(attacker.Intelligence + 1));
            damage -=  new Random().Next(opponent.Defense + 1);

            if (damage > 0)
            {
                opponent.HitPoints -= damage;
            }

            if (opponent.HitPoints <= 0)
            {
                opponent.HitPoints = 0;
                response.Message = $"{opponent.Name} has been defeated!";
            }
            else
            {
                response.Message = $"{attacker.Name} has used {skill.Name} skill!";
            }

            response.Data = new AttackResultDto()
            {
                Attacker = attacker.Name,
                Opponent = opponent.Name,
                AttackerHP = attacker.HitPoints,
                OpponentHP = opponent.HitPoints,
                WeaponName = "No weapon used",
                SkillName = skill.Name,
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