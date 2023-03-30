namespace dotnet7_rpg.Models;

public class ServiceResponse<T>
{   
    // "?" because it can be nullable
    public T? Data { get; set; }
    public bool Success { get; set; } = true;
    public string Message { get; set; } = string.Empty;
}