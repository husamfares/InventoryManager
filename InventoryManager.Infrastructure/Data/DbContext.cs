using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryManager.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Product> Products { get; set; }

    public DbSet<AppUser> AppUser { get; set; }

}
