// tutti i global saranno accessibili in globale
global using dotnet7_rpg.Models;
global using dotnet7_rpg.Services.CharacterService;
global using dotnet7_rpg.Dtos.Character;
global using AutoMapper;
global using Microsoft.EntityFrameworkCore;
global using dotnet7_rpg.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Adding the DbContext
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // enable the option in the swagger UI to enter the Bearer Token
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = """ Standard Authorization header using the Bearer scheme. Example: "bearer {token}" """,
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    
    c.OperationFilter<SecurityRequirementsOperationFilter>();
});

// Register HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// now the web API knows that it has to use the CharacterService Class whenever a controller wants to inject the ICharacterService
// in essence it pass to the constructor of the controller CharacterService class to the parameter characterService
//At runtime, the container will resolve the dependency and create an instance of CharacterService to be injected into the controller.
builder.Services.AddScoped<ICharacterService, CharacterService>();

// Register the AuthRepository instance
builder.Services.AddScoped<IAuthRepository, AuthRepository>();



//Register AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// AddAuthentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // initialize a new instance of token validation parameters and then set these parameters
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token")
                    .Value!)), // "!" null-forgiving operator, this means that I'm sure that this won't be null, but in case it will, it returns null as well
            ValidateIssuer = false,
            ValidateAudience = false

        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// middleware for https
app.UseHttpsRedirection();

// important to insert above UseAuthorization();
app.UseAuthentication();

//middleware for authorization
app.UseAuthorization();

app.MapControllers();

app.Run();


