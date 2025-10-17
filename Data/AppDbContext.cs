using Microsoft.EntityFrameworkCore;
using KuroApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace KuroApi.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Customer> Customers { get; set; }
    }
}
