

namespace dotnet7_rpg.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }
    
    // with DbSet we are able to query and save RPG charcaters 
    // the name of DbSet will be the pluralized name of the Character entity (and it will be the corrisponding database table)
    // this is the Characters table
    public DbSet<Character> Characters => Set<Character>(); // Creates a DbSet<TEntity> that can be used to query and save instances of TEntity
    public DbSet<User> Users => Set<User>();
    public DbSet<Weapon> Weapons => Set<Weapon>();
}