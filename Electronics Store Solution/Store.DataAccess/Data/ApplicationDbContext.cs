using Microsoft.EntityFrameworkCore;
using Store.Entities.Models;

namespace Store.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            {

            }

        }
        public DbSet<Category>Categories { get; set; }
        public DbSet<Product>Products { get; set; }


    }
}
