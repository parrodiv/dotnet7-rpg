

namespace dotnet7_rpg.Services.CharacterService;

public interface ICharacterService
{
    Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters();
    Task<ServiceResponse<GetCharacterDto>> GetSingleCharacter(int id);
    Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter);
    Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter);
    Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id);
    Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill);
    Task<ServiceResponse<GetCharacterDto>> GetSingleCharacterNoAuth(int id);
    Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharactersNoAuth();

}