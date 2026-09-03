using LiveProject_WebAPI_Demo.Models;
using Microsoft.EntityFrameworkCore;

namespace LiveProject_WebAPI_Demo.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Book> Books { get; set; }
    
    public DbSet<User> Users { get; set; }
}