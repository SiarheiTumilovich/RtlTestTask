using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Entities;
using BusinessLayer.Providers.ShowProvider.Entities;

namespace BusinessLayer.Providers.ShowProvider
{
    public interface IShowProvider
    {
        Task<SearchResults<Show>> GetShows(int pageNum, int pageSize);
        Task SaveShows(ICollection<Show> newShows);
        Task Delete(int showId);
    }
}
