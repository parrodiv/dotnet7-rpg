using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet7_rpg.Controllers;

// ControllerBase is a base class for an MVC controller without View support, that will be Controller
// Indicates that a type and all derived types are used to serve HTTP API responses.
// "api/Character" the part of the name of c# class that comes before Controller word
[Authorize] // this means that we have to add the bearer token to the header of our request
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
        var response = await _characterService.GetAllCharacters();
        if (response.Data == null)
        {
            return NotFound(response);
        }
        return Ok(response); 
    }
    
    
    [HttpGet("GetAllNoAuth")]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> GetAllNoAuth()
    {
        var response = await _characterService.GetAllCharactersNoAuth();
        if (response.Data == null)
        {
            return NotFound(response);
        }
        return Ok(response); 
    }
    
    

    [HttpGet("{id:int}")] // parameter {id} has to be the same name of the parameter passed to the method
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> GetSingle(int id) // api/Character/{id}
    {
        var response = await _characterService.GetSingleCharacter(id);
        if (response.Data == null)
        {
            return NotFound(response);
        }
        return Ok(response); 
        
    }
    
    

    [HttpPost()]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> AddCharacter(AddCharacterDto newCharacter)
    {
        var response = await _characterService.AddCharacter(newCharacter);
        if (response.Data == null)
        {
            return NotFound(response);
        }
        return Ok(response); 
        
    }
    
    
    [HttpPut()]
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
    {
        var response = await _characterService.UpdateCharacter(updatedCharacter);
        if (response.Data == null)
        {
            return NotFound(response);
        }
        return Ok(response);
    }
    
    
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> DeleteCharacter(int id)
    {
        var response = await _characterService.DeleteCharacter(id);
        if (response.Data == null)
        {
            return NotFound(response);
        }
        return Ok(response); 
    }


    [HttpPost("Skill")]
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill)
    {
        var response = await _characterService.AddCharacterSkill(newCharacterSkill);
        if (response.Data == null)
        {
            return NotFound(response);
        }
        return Ok(response); 
    }
}