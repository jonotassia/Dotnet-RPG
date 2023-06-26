using Microsoft.EntityFrameworkCore;

namespace Dotnet_RPG.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
        // Each DbSet corresponds to the name of the database table you would like to create
        public DbSet<Character> Characters => Set<Character>();

        public DbSet<User> Users => Set<User>();
    }
}
