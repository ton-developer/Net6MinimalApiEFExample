using Microsoft.EntityFrameworkCore;

namespace MinimalWebApplication;

public class CustomDbContext : DbContext
{
    public CustomDbContext(DbContextOptions<CustomDbContext> contextOptions) : base(contextOptions)
    {
    }
    
    public DbSet<DbObject> Objects { get; set; }
    
}