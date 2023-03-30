namespace dotnet7_rpg.Services.CharacterService;

public class CharacterService : ICharacterService
{
    private static List<Character> characters = new List<Character>
    {
        new Character(),
        new Character { Id = 1, Name = "Sam" }
    };
    
    public async Task<ServiceResponse<List<Character>>> GetAllCharacters()
    {
        var serviceResponse = new ServiceResponse<List<Character>>();
        serviceResponse.Data = characters;
        return serviceResponse;
    }

    public async Task<ServiceResponse<Character>> GetSingleCharacter(int id)
    {
        var character =  characters.FirstOrDefault(c => c.Id == id);
        var serviceResponse = new ServiceResponse<Character>
        {
            Data = character
        };
        if (character is not null)
        {
            return serviceResponse;
        }

        throw new Exception($"Character not found with id:{id}");
    }

    public async Task<ServiceResponse<List<Character>>> AddCharacter(Character newCharacter)
    {
        characters.Add(newCharacter);
        var serviceResponse = new ServiceResponse<List<Character>>
        {
            Data = characters
        };
        return serviceResponse;
    }
}
