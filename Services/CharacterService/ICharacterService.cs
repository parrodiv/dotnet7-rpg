

namespace dotnet7_rpg.Services.CharacterService;

public interface ICharacterService
{
    Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters(int userId);
    Task<ServiceResponse<GetCharacterDto>> GetSingleCharacter(int id);
    Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter);
    Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter);
    Task<ServiceResponse<GetCharacterDto>> DeleteCharacter(int id);
}