

namespace dotnet7_rpg.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Skill>().HasData(
            new Skill {Id = 1, Name="Fireball", Damage = 30},
            new Skill {Id = 2, Name="Frenzy", Damage = 20},
            new Skill {Id = 3, Name="Blizzard", Damage = 50}
        );
    }

    // with DbSet we are able to query and save RPG charcaters 
    // the name of DbSet will be the pluralized name of the Character entity (and it will be the corrisponding database table)
    // this is the Characters table
    public DbSet<Character> Characters => Set<Character>(); // Creates a DbSet<TEntity> that can be used to query and save instances of TEntity
    public DbSet<User> Users => Set<User>();
    public DbSet<Weapon> Weapons => Set<Weapon>();
    public DbSet<Skill> Skills => Set<Skill>();
}