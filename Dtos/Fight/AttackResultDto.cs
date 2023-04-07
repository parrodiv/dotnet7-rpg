namespace dotnet7_rpg.Dtos.Fight;

public class AttackResultDto
{
    public string? Attacker { get; set; }
    public string? Opponent { get; set; }
    public int AttackerHP { get; set; }
    public int OpponentHP { get; set; }
    public string? SkillName { get; set; }
    public string? WeaponName { get; set; }
    public int Damage { get; set; }
}