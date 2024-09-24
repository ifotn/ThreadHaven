using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ThreadHaven.Models;

namespace ThreadHaven.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

       // add 1 DbSet object for each model which can perform CRUD operations
       public DbSet<Product> Products { get; set; }
       public DbSet<Category> Categories { get; set; }
       public DbSet<CartItem> CartItems { get; set; }
       public DbSet<OrderDetail> OrderDetails { get; set; }    
       public DbSet<Order> Orders { get; set; }
    }
}
