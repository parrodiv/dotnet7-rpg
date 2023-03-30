namespace dotnet7_rpg.Services.CharacterService;

public interface ICharacterService
{
    List<Character> GetAllCharacters();
    Character GetSingleCharacter(int id);
    List<Character> AddCharacter(Character newCharacter);
}