namespace dotnet7_rpg;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Character, GetCharacterDto>(); // Character to GetCharacterDto
        CreateMap<AddCharacterDto, Character>(); // AddCharacterDto to Character
        CreateMap<UpdateCharacterDto, Character>();
        CreateMap<UpdateCharacterDto, GetCharacterDto>();
    }
}