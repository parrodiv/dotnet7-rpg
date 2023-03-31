using Microsoft.AspNetCore.Mvc;

namespace dotnet7_rpg.Controllers;

// ControllerBase is a base class for an MVC controller without View support, that will be Controller
// Indicates that a type and all derived types are used to serve HTTP API responses.
// "api/Character" the part of the name of c# class that comes before Controller word
[ApiController]
[Route("api/[controller]")]
public class CharacterController : ControllerBase
{
    // private member for the constructor
    private readonly ICharacterService _characterService;

    // constructor
    public CharacterController(ICharacterService characterService)
    {
        _characterService = characterService;
    }
    
    

    // api will be: "api/Character/GetAll", alternative of [HttpGet("GetAll")] could be: [HttpGet] and underneath: [Route("GetAll")]
    [HttpGet("GetAll")]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> GetAll()
    {
        return Ok(await _characterService.GetAllCharacters()); // ok = 200 status code, NotFound = 404, BadRequest = 400
    }
    
    

    [HttpGet("{id:int}")] // parameter {id} has to be the same name of the parameter passed to the method
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> GetSingle(int id) // api/Character/{id}
    {
        return Ok(await _characterService.GetSingleCharacter(id));
        //FirstOrDefault method is used to find the first Character object in the characters list that has an ID equal to the one specified in the "id" variable.
        //If an object with the matching ID is found, the method will return that object,
        //otherwise it will return the default value for the Character data type (which in this case is null)
    }
    
    

    [HttpPost()]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> AddCharacter(AddCharacterDto newCharacter)
    {
        return Ok(await _characterService.AddCharacter(newCharacter));
    }
    
    
    [HttpPut()]
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
    {
        return Ok(await _characterService.UpdateCharacter(updatedCharacter));
    }
}