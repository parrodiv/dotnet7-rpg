
using Microsoft.AspNetCore.Mvc;

namespace dotnet7_rpg.Controllers;

// Controllerbase is a base class for an MVC controller without View support, that will be Controller
[ApiController]      // Indicates that a type and all derived types are used to serve HTTP API responses.
[Route("api/[controller]")] // "api/Character" the part of the name of c# class that comes before Controller word
public class CharacterController : ControllerBase
{
    private static List<Character> characters = new List<Character>
    {
        new Character(),
        new Character { Name = "Sam" }
    };
    
    [HttpGet]
    public ActionResult<List<Character>> Get() // ActionResult<Character> means that this action returns a Character type data
    {
        return Ok(characters); // ok = 200 status code, NotFound = 404, BadRequest = 400
    }
}
