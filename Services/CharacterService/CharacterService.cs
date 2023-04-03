
namespace dotnet7_rpg.Services.CharacterService;

public class CharacterService : ICharacterService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;

    public CharacterService(IMapper mapper, DataContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    
    
    public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
    {
        var dbCharacters = await _context.Characters.ToListAsync();
        var serviceResponse = new ServiceResponse<List<GetCharacterDto>>()
        {
            Data = _mapper.Map<List<GetCharacterDto>>(dbCharacters)
        };
        return serviceResponse;
    }

    public async Task<ServiceResponse<GetCharacterDto>> GetSingleCharacter(int id)
    {
      
        var serviceResponse = new ServiceResponse<GetCharacterDto>();
      
        try
        {
            var dbCharacter =  await _context.Characters.FirstOrDefaultAsync(c => c.Id == id) ?? throw  new  Exception($"Character Id {id} not found");
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(dbCharacter);
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
        _context.Characters.Add(character); // characters is a List of Character not a List of AddCharacterDto, so newCharacter should be converted in Character
        await _context.SaveChangesAsync();
        var serviceResponse = new ServiceResponse<List<GetCharacterDto>>
        {
            Data = await _context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync() // Select returns an enum so I convert it into a string
        };
        return serviceResponse;
    }

    public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
    {
        var serviceResponse = new ServiceResponse<GetCharacterDto>();
        try
        {
            var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id) ??
                            throw new Exception($"the id {updatedCharacter.Id} of character to update is not found");
            
            // copy the updateCharacter to character, it is a shortcut for do character.Name = updatedCharacter.Name and so on....
            _mapper.Map(updatedCharacter, character);

            // Save changes to the database
            await _context.SaveChangesAsync();
            
            
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
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
        var characterToRemove = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);
        var serviceResponse = new ServiceResponse<GetCharacterDto>
        {
            Data = _mapper.Map<GetCharacterDto>(characterToRemove)
        };
        try
        {
            _context.Characters.Remove(characterToRemove ?? throw new Exception($"Character Id {id} not found"));
            await _context.SaveChangesAsync();
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
