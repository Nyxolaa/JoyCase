using Microsoft.EntityFrameworkCore;

namespace JoyCase.Data;

public interface IJoyDbContext
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
}
