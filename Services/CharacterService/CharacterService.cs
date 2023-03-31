namespace dotnet7_rpg.Services.CharacterService;

public class CharacterService : ICharacterService
{
    private static List<Character> characters = new List<Character>
    {
        new Character(),
        new Character { Id = 1, Name = "Sam" }
    };
    
    public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
    {
        var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
        serviceResponse.Data = characters;  // now it gives an error (ServiceResponse<T> --> T became   List<GetCharacterDto> so Data is List<GetCharacterDto>, while characters is List<Character> )
        return serviceResponse;
    }

    public async Task<ServiceResponse<GetCharacterDto>> GetSingleCharacter(int id)
    {
        var character =  characters.FirstOrDefault(c => c.Id == id);
        var serviceResponse = new ServiceResponse<GetCharacterDto>
        {
            Data = character // now it gives an error ( same as line 14)
        };
        if (character is not null)
        {
            return serviceResponse;
        }

        throw new Exception($"Character not found with id:{id}");
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
    {
        characters.Add(newCharacter);
        var serviceResponse = new ServiceResponse<List<GetCharacterDto>>
        {
            Data = characters // now it gives an error ( same as line 14)
        };
        return serviceResponse;
    }
}
