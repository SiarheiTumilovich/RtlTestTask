using System.Threading.Tasks;
using DataLayer.Domains;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Storages.DbContexts
{
    internal interface IMyDbContext
    {
        DbSet<Show> Shows { get; }
        DbSet<Person> People { get; }
        DbSet<ShowPersonAssoc> ShowPersonAssocs { get; }
        Task Save();
    }
}
