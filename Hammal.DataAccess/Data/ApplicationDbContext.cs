

using Hammal.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hammal.DataAccess.Data
{
    public class ApplicationDbContext: IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options):base(options)
        {
            
        }

        public DbSet<Category> Categories{ get; set; }
        public DbSet<AltCategory> AltCategories{ get; set; }
        public DbSet<Advertisement> Advertisements{ get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Address> Addresses{ get; set; }
        public DbSet<UserAbility> UserAbilities{ get; set; }
        public DbSet<SystemUser> SystemUsers{ get; set; }
        public DbSet<Order> Orders{ get; set; }
        
    }
}
