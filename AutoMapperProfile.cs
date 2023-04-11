using dotnet7_rpg.Dtos.Fight;
using dotnet7_rpg.Dtos.Skill;
using dotnet7_rpg.Dtos.Weapon;

namespace dotnet7_rpg;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Character, GetCharacterDto>(); // Character to GetCharacterDto
        CreateMap<AddCharacterDto, Character>(); // AddCharacterDto to Character
        CreateMap<UpdateCharacterDto, Character>();
        CreateMap<UpdateCharacterDto, GetCharacterDto>();
        CreateMap<AddWeaponDto, Weapon>();
        CreateMap<Weapon, GetWeaponDto>();
        CreateMap<Skill, GetSkillDto>();
        CreateMap<GetCharacterDto, Character>();
        CreateMap<GetWeaponDto, Weapon>();
        CreateMap<GetSkillDto, Skill>();
        CreateMap<Character, HighScoreDto>();
    }
}