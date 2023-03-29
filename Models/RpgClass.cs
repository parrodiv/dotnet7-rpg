using System.Text.Json.Serialization;

namespace dotnet7_rpg.Models;

// Converts enumeration values to and from strings. without this in Swagger UI we can't see the structure of RpgClass and in the response
// without Converter {..., class:1} with converter {..., class: "Knight"} 
[JsonConverter(typeof(JsonStringEnumConverter))] 
public enum RpgClass
{
    Knight = 1,
    Mage = 2,
    Cleric = 3
}