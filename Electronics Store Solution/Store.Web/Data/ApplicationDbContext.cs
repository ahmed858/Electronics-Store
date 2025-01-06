using Microsoft.EntityFrameworkCore;
using Store.Web.Models;

namespace Store.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            {

            }

        }
        public DbSet<Category>Categories { get; set; }

    }
}
