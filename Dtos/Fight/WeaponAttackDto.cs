namespace dotnet7_rpg.Dtos.Fight;

public class WeaponAttackDto
{
    // since the attacker can have only one Weapon, we only need to know who the attacker is
    public int AttackerId { get; set; }
    public int OpponentId { get; set; }
}