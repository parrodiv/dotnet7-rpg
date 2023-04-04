
using System.Security.Claims;

namespace dotnet7_rpg.Services.CharacterService;

public class CharacterService : ICharacterService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User
        .FindFirstValue(ClaimTypes.NameIdentifier)!);
    
    public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
    {
        // Now I'll get all the character associated to the user that makes the request
        var dbCharacters = await _context.Characters.Where(c => c.User!.Id == GetUserId() ).ToListAsync();
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
        // Set the User property of Character searching in the Users table the corrisponding id of the user that make a request
        character.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
        _context.Characters.Add(character); 
        await _context.SaveChangesAsync();
        var serviceResponse = new ServiceResponse<List<GetCharacterDto>>
        {
            Data = await _context.Characters
                .Where(c => c.User!.Id == GetUserId())
                .Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync() // Select returns an enum so I convert it into a string
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
