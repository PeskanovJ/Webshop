using Microsoft.EntityFrameworkCore;
using Projekat.DAL.Model;

namespace Projekat.DAL
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option)
        {

        }
        //public DbSet<Category> Categories { get; set; }
        public DbSet<Item> Products { get; set; }
        public DbSet<User> Users { get; set; }


    }
}
