
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
        var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
        serviceResponse.Data = _mapper.Map<List<GetCharacterDto>>(characters);  
        return serviceResponse;
    }

    public async Task<ServiceResponse<GetCharacterDto>> GetSingleCharacter(int id)
    {
        var character =  characters.FirstOrDefault(c => c.Id == id);
        var serviceResponse = new ServiceResponse<GetCharacterDto>
        {
            Data = _mapper.Map<GetCharacterDto>(character)
        };
        if (character is not null)
        {
            return serviceResponse;
        }

        throw new Exception($"Character not found with id:{id}");
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
    {
        characters.Add(_mapper.Map<Character>(newCharacter)); // characters is a List of Character not a List of AddCharacterDto, so newCharacter should be converted in Character
        var serviceResponse = new ServiceResponse<List<GetCharacterDto>>
        {
            //Data = _mapper.Map<List<GetCharacterDto>>(characters) 
            // ALTERNATIVE
            Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList() // Select returns an enum so I convert it into a string
        };
        return serviceResponse;
    }
}
