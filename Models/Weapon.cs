namespace dotnet7_rpg.Models;

public class Weapon
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Damage { get; set; }
    public Character? Character { get; set; }
    public int CharacterId { get; set; }
    // with the help of that convention using the C# class name Character + Id (CharacterId)
    // EF knows that this is the corresponding foreign key for the Character property
}