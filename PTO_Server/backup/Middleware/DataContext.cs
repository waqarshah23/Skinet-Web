using Microsoft.EntityFrameworkCore;
using PTO_Server.Models;

namespace PTO_Server.Middleware
{
    public class DataContext: DbContext
    {
        public DataContext(): base()
        {
                
        }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Users> Users { get; set; }

        public DbSet<PTO_Types> PTO_Types { get; set; }

        public DbSet<PTO_History> PTO_History { get; set; }

        public DbSet<Products> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                optionsBuilder.UseSqlServer(config.GetConnectionString("mysqlconnection"));
            }
        }
    }
}
