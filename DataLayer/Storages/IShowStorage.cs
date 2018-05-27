using System.Collections.Generic;
using System.Threading.Tasks;
using DataLayer.Domains;

namespace DataLayer.Storages
{
    public interface IShowStorage
    {
        Task AddShows(ICollection<Show> shows);
        Task<ICollection<Show>> GetShows(int pageNum, int pageSize);
        Task<int> GetShowsCount();
        Task Delete(int showId);
    }
}
