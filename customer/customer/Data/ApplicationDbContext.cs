using customer.Models;
using Microsoft.EntityFrameworkCore;

namespace customer.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Offer> offer { get; set; }

    }
}
