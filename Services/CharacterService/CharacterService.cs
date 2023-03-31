
namespace dotnet7_rpg.Services.CharacterService;

public class CharacterService : ICharacterService
{

    private static List<Character> characters = new List<Character>
    {
        new Character(),
        new Character { Id = 1, Name = "Sam" }
    };
    
    private readonly IMapper _mapper;

    public CharacterService(IMapper mapper)
    {
        _mapper = mapper;
    }

    
    
    public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
    {
        var serviceResponse = new ServiceResponse<List<GetCharacterDto>>
        {
            Data = _mapper.Map<List<GetCharacterDto>>(characters)
        };
        return serviceResponse;
    }

    public async Task<ServiceResponse<GetCharacterDto>> GetSingleCharacter(int id)
    {
      
        var serviceResponse = new ServiceResponse<GetCharacterDto>();
      
        try
        {
            var character =  characters.FirstOrDefault(c => c.Id == id) ?? throw  new  Exception($"Character Id {id} not found");
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
        }
        catch (Exception ex)
        {
            serviceResponse.Message = ex.Message;
            serviceResponse.Success = false;
        }

        return serviceResponse;

    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
    {
        var character = _mapper.Map<Character>(newCharacter);
        character.Id = characters.Max(c => c.Id) + 1; // Max() search the maximum number between the c.Id props
        characters.Add(character); // characters is a List of Character not a List of AddCharacterDto, so newCharacter should be converted in Character
        var serviceResponse = new ServiceResponse<List<GetCharacterDto>>
        {
            //Data = _mapper.Map<List<GetCharacterDto>>(characters) 
            // ALTERNATIVE
            Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList(), // Select returns an enum so I convert it into a string
        };
        return serviceResponse;
    }

    public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
    {
        var serviceResponse = new ServiceResponse<GetCharacterDto>();
        try
        {
            var character = characters.FirstOrDefault(c => c.Id == updatedCharacter.Id);
            int indexToChange = characters.IndexOf(character ?? throw new InvalidOperationException($"the id {updatedCharacter.Id} of character to update is not found"));
            characters[indexToChange] = _mapper.Map<Character>(updatedCharacter);
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(updatedCharacter);
        }
        catch(Exception ex)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = ex.Message;
        }
        return serviceResponse;
        
    }

    public async Task<ServiceResponse<GetCharacterDto>> DeleteCharacter(int id)
    {
        var characterToRemove = characters.FirstOrDefault(c => c.Id == id);
        var serviceResponse = new ServiceResponse<GetCharacterDto>
        {
            Data = _mapper.Map<GetCharacterDto>(characterToRemove)
        };
        try
        {
            characters.Remove(characterToRemove ?? throw new Exception($"Character Id {id} not found"));
            serviceResponse.Message = $"Character with id {id} is removed successfully";
        }
        catch (Exception ex)
        {
            serviceResponse.Message = ex.Message;
            serviceResponse.Success = false;
        }

        return serviceResponse;
    }
}
