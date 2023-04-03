using Microsoft.AspNetCore.Mvc;

namespace dotnet7_rpg.Dtos.User;

public class UserRegisterDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

}