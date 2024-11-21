using Microsoft.EntityFrameworkCore;
using nagyv_projekt.Entities;

namespace nagyv_projekt.Contexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<Books> Books { get; set; }
    
    public DbSet<Readers> Readers { get; set; }
    
    public DbSet<Lending> Lending { get; set; }
}