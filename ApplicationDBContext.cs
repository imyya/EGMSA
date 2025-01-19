using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using EMGMSA.Models;
using Microsoft.AspNetCore.Identity;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    // Table des voitures
    public DbSet<Car> Cars { get; set; }

}
