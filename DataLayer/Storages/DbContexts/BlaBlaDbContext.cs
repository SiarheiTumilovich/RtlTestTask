using System.Threading.Tasks;
using DataLayer.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataLayer.Storages.DbContexts
{
    internal class MyDbContext : DbContext, IMyDbContext
    {
        private readonly string _connectionString;

        public MyDbContext(DbContextOptions options, IConfiguration configuration)
            : base(options)
        {
            _connectionString = configuration.GetSection("AppSettings")["BlaBlaDbConnectionString"];
        }

        public DbSet<Show> Shows => Set<Show>();
        public DbSet<Person> People => Set<Person>();
        public DbSet<ShowPersonAssoc> ShowPersonAssocs => Set<ShowPersonAssoc>();

        public async Task Save()
        {
            await SaveChangesAsync();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShowPersonAssoc>()
                .HasKey(x => new {x.ShowId, x.PersonId});
        }
    }
}
