using Microsoft.EntityFrameworkCore;
using DoppelgangstersOnline.Database.Models;

namespace DoppelgangstersOnline.Database
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Content> Contents { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }
    }
}