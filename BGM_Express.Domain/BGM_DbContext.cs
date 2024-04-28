using BGM_Express.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BGM_Express.Domain
{
    public class BGM_DbContext : DbContext
    {
        public BGM_DbContext(DbContextOptions<BGM_DbContext> options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Buyer> Buyers { get; set; }
    }
}
