using Microsoft.EntityFrameworkCore;

namespace Mango.Services.Coupon.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) 
        {
            
        }
        public DbSet<Coupon> Coupons { get; set; }
    }
}
