using System.Security.Cryptography.X509Certificates;
using Azure;
using dotnet7_rpg.Dtos.Fight;
using dotnet7_rpg.Dtos.Skill;

namespace dotnet7_rpg.Services.FightService;

public class FightService : IFightService
{
    private readonly DataContext _context;
    private readonly ICharacterService _characterService;
    private readonly IMapper _mapper;
    private readonly ILogger<FightService> _logger;

    public FightService(DataContext context, ICharacterService characterService, IMapper mapper, ILogger<FightService> logger)
    {
        _context = context;
        _characterService = characterService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request)
    {
        var response = new ServiceResponse<AttackResultDto>();
        try
        {
            // who make the attack request has to be authenticated adn he has to be the owner of the attacker character
            // (see GetSingleCharacter in CharacterService where it is made the check for both cases)
            var attackerDto = _characterService.GetSingleCharacter(request.AttackerId).Result.Data;
            
            var opponentDto = _characterService.GetSingleCharacterNoAuth(request.OpponentId).Result.Data;
            
            var attacker = _mapper.Map<Character>(attackerDto);
            var opponent = _mapper.Map<Character>(opponentDto);

            if (attacker is null || opponent is null || attacker.Weapon is null)
            {
                throw new Exception("Something fishy is going on here...");
            }

            
            
            
            // the formula takes the damage of the Weapon and adds a random value between zero and the strength of the attacker
            var damage = DoWeaponDamage(attacker, opponent);

            if (opponent.HitPoints <= 0)
            {
                opponent.HitPoints = 0;
                response.Message = $"{opponent.Name} has been defeated!";
            }
            else
            {
                response.Message = $"{attacker.Name} has used {attacker.Weapon!.Name} skill!";
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

    private static int DoWeaponDamage(Character attacker, Character opponent)
    {
        if (attacker.Weapon == null) throw new Exception($"Attacker {attacker.Name} has no weapon!");

        int damage = attacker.Weapon.Damage + (new Random().Next(attacker.Strength + 1));
        damage = damage - new Random().Next(opponent.Defense + 1);

        if (damage > 0)
        {
            opponent.HitPoints -= damage;
        }

        return damage;
    }

    public async Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto request)
    {
        var response = new ServiceResponse<AttackResultDto>();
        try
        {
            var attackerDto = _characterService.GetSingleCharacter(request.AttackerId).Result.Data;
            
            var opponentDto = _characterService.GetSingleCharacterNoAuth(request.OpponentId).Result.Data;
            
            var attacker = _mapper.Map<Character>(attackerDto);
            var opponent = _mapper.Map<Character>(opponentDto);

            if (attacker is null || opponent is null)
            {
                throw new Exception("Something fishy is going on here...");
            }

            var skill = attacker.Skills!.FirstOrDefault(s => s.Id == request.SkillId)
                        ?? throw new Exception($"{attacker.Name} doesn't know skillId {request.SkillId}");

            var damage = DoSkillDamage(skill, attacker, opponent);

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

    private static int DoSkillDamage(Skill? skill, Character attacker, Character opponent)
    {
        int damage = skill.Damage + (new Random().Next(attacker.Intelligence + 1));
        damage -= new Random().Next(opponent.Defense + 1);

        if (damage > 0)
        {
            opponent.HitPoints -= damage;
        }

        return damage;
    }

    public async Task<ServiceResponse<FightResultDto>> Fight(FightRequestDto request)
    {
        var response = new ServiceResponse<FightResultDto>()
        {
            Data = new FightResultDto()
        };
        
        _logger.LogInformation("Starting the fight...");
        
        try
        {
            var characters = await _context.Characters
                .Include(c => c.Weapon)
                .Include(c => c.Skills)
                .Where(c => request.CharacterIds.Contains(c.Id)).ToListAsync();
            
            _logger.LogInformation($"Found {characters.Count} characters.");

            if (characters is null) throw new Exception("Characters not found");

            bool defeated = false;
            while (!defeated)
            {
                foreach (var attacker in characters )
                {
                    _logger.LogInformation($"The attacker is {attacker.Name}");

                    var opponents = characters.Where(c => c.Id != attacker.Id).ToList();  // there will be a list of opponents based on the actual attacker in the loop
                    var opponent = opponents[new Random().Next(opponents.Count)];
                    
                    _logger.LogInformation($"The opponent is {opponent.Name}");


                    int damage = 0;
                    string attackUsed = string.Empty;

                    bool useWeapon = new Random().Next(2) == 0;   // the result of new Random can be 0 or 1, so if it will be 0 useWeapon will be true
                    
                    _logger.LogInformation($"{attacker.Name} uses {(useWeapon  ? "a weapon attack" : "a skill attack")}");


                    if (useWeapon && attacker.Weapon is not null)
                    {
                        attackUsed = attacker.Weapon.Name;
                        damage = DoWeaponDamage(attacker, opponent);
                        _logger.LogInformation($"{attacker.Weapon.Name} did {damage} damage");

                    }
                    else if (!useWeapon && attacker.Skills is not null)
                    {
                        var skill = attacker.Skills[new Random().Next(attacker.Skills.Count)];
                        attackUsed = skill.Name;
                        damage = DoSkillDamage(attacker.Skills.Find(s => s == skill), attacker, opponent);
                        _logger.LogInformation($"{attackUsed} did {damage} damage");
                    }
                    else
                    {
                        response.Data.Log
                            .Add($"{attacker.Name} wasn't able to attack");
                        continue; // means that skips the remaining code and move to the next iteration 
                    }
                    
                    response.Data.Log
                        .Add($"{attacker.Name} attacks {opponent.Name} using {attackUsed} with {(damage >= 0 ? damage : 0)} damage");

                    if (opponent.HitPoints <= 0)
                    {

                        defeated = true;
                        attacker.Victories++;
                        opponent.Defeats++;
                        response.Data.Log.Add($"{opponent.Name} has been defeated!");
                        response.Data.Log.Add($"{attacker.Name} wins with {attacker.HitPoints} HP left");
                        break;  //break the foreach loop
                    }
                }
            }
            
            await _context.Characters
                .Where(c => request.CharacterIds.Contains(c.Id))
                .ForEachAsync(c =>
                    {
                        c.Fights++;
                        c.HitPoints = 100;
                    });

            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Fight completed successfully");

        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
            
            _logger.LogInformation("Failed to complete the fight");

        }

        return response;
    }

    public async Task<ServiceResponse<List<HighScoreDto>>> GetHighScore()
    {
        var characters = await _context.Characters
            .Where(c => c.Fights > 0)
            .OrderByDescending(c => c.Victories)
            .ThenBy(c => c.Defeats).ToListAsync();  // if victories are the same number of another character, ThenBy sort Defeats in ascending order

        var response = new ServiceResponse<List<HighScoreDto>>()
        {
            // map the type of each character that match the above query
            Data = characters.Select(c => _mapper.Map<HighScoreDto>(c)).ToList(),
            Message = "The HighScore filter has been executed successfully!"
        };

        return response;
    }
}