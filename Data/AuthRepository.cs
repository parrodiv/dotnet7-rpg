

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;

namespace dotnet7_rpg.Data;

public class AuthRepository : IAuthRepository
{
    private readonly DataContext _context;
    private readonly IConfiguration _configuration;

    public AuthRepository(DataContext context, IConfiguration configuration)  // with configuration I can access to appsettings.json file where it is stored the Token key 
    {
        _context = context;
        _configuration = configuration;
    }
    public async Task<ServiceResponse<int>> Register(User user, string password)
    {
        var response = new ServiceResponse<int>();
        if (await UserExists(user.Username))
        {
            response.Success = false;
            response.Message = "User already exists";
            return response;
        }
        CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        
        response.Data = user.Id;
        return response;
    }

    public async Task<ServiceResponse<string>> Login(string username, string password)
    {
        var response = new ServiceResponse<string>();
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
        if (user is null)
        {
            response.Success = false;
            response.Message = "User doesn't exist";
        } else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
        {
            response.Success = false;
            response.Message = "Wrong password";
        }
        else
        {
            response.Data = CreateToken(user);
        }

        return response;
        
    }

    public async Task<bool> UserExists(string username)
    {
        if (await _context.Users.AnyAsync(u => u.Username.ToLower() == username.ToLower()))
        {
            return true;
        }

        return false;
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt);
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        // compare computedHash with the passwordHash in db
        return computedHash.SequenceEqual(passwordHash);  // it returns true or false
    }

    private string CreateToken(User user)
    {   
        // A claim is a piece of information about the user that will be included in the token
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };
        
        // Get the app settings token: is a secret key that is used to sign the token
        var appSettingsToken = _configuration.GetSection("AppSettings:Token").Value;
        if (appSettingsToken is null)
        {
            throw new Exception("AppSettings Token is null!");
        }
    
        // Create a SymmetricCredentials: This object contains the key and algorithm used to sign the token
        SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(appSettingsToken));
        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        
        // This object specifies the details of the token, such as the claims, expiration time, and signing credentials
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(1),
            SigningCredentials = creds
        };
        
        // This object is used to create and write the token
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        
        // Create the token: the token is created using the token descriptor
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        
        // the token is written as a string using the WriteToken method of the JwtSecurityTokenHandler
        return tokenHandler.WriteToken(token);
    }
}