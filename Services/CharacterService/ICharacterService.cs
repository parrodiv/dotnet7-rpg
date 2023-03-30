namespace dotnet7_rpg.Services.CharacterService;

public interface ICharacterService
{
    Task<ServiceResponse<List<Character>>> GetAllCharacters();
    Task<ServiceResponse<Character>> GetSingleCharacter(int id);
    Task<ServiceResponse<List<Character>>> AddCharacter(Character newCharacter);
}