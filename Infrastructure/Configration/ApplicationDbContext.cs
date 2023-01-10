using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Configration;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    //public virtual DbSet<Movie> Movie { get; set; }
}