// tutti i global saranno accessibili in globale
global using dotnet7_rpg.Models;
global using dotnet7_rpg.Services.CharacterService;
global using dotnet7_rpg.Dtos.Character;
global using AutoMapper;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// now the web API knows that it has to use the CharacterService Class whenever a controller wants to inject the ICharacterService
// in essence it pass to the constructor of the controller CharacterService class to the parameter characterService
builder.Services.AddScoped<ICharacterService, CharacterService>();
// AddScoped = means that a single instance of the service is created within each scope.
// AddTransient = provides a new instance to every controller and to every service even within the same request
// AddSingleton = this one creates only one istance that is used for every request

//Register AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// middleware for https
app.UseHttpsRedirection();

//middleware for authorization
app.UseAuthorization();

app.MapControllers();

app.Run();


