
using Microsoft.AspNetCore.Mvc;

namespace dotnet7_rpg.Controllers;

// Controllerbase is a base class for an MVC controller without View support, that will be Controller
// Indicates that a type and all derived types are used to serve HTTP API responses.
// "api/Character" the part of the name of c# class that comes before Controller word
[ApiController]      
[Route("api/[controller]")] 
public class CharacterController : ControllerBase
{
    private static List<Character> characters = new List<Character>
    {
        new Character(),
        new Character { Name = "Sam" }
    };
    
    // api will be: "api/Character/GetAll", alternative of [HttpGet("GetAll")] could be: [HttpGet] and underneath: [Route("GetAll")]
    [HttpGet("GetAll")] 
    public ActionResult<List<Character>> Get() 
    {
        return Ok(characters); // ok = 200 status code, NotFound = 404, BadRequest = 400
    }
    
    [HttpGet]
    public ActionResult<Character> GetSingle() 
    {
        return Ok(characters[0]); 
    }
}
