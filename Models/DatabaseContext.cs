using Microsoft.EntityFrameworkCore;

namespace BackendSpicy.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }
        public DbSet<Account> Account { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<CategoryProduct> CategoryProduct { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<OrderAccount> OrderAccount { get; set; }
        public DbSet<ProductList> ProductList { get; set; }
        public DbSet<ProductDescription> ProductDescription { get; set; }
    }
}

