namespace dotnet7_rpg.Services.CharacterService;

public interface ICharacterService
{
    Task<List<Character>> GetAllCharacters();
    Task<Character> GetSingleCharacter(int id);
    Task<List<Character>> AddCharacter(Character newCharacter);
}