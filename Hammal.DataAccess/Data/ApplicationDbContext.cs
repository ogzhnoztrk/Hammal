

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
        public DbSet<Advertisement> Advertisements{ get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}
