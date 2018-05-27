using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Providers.TvMaze.Entities;

namespace BusinessLayer.Providers.TvMaze
{
    internal interface ITvMazeApi
    {
        Task<ICollection<TvMazeShow>> GetShows(int pageNum);
        Task<ICollection<TvMazeCast>> GetCasts(int showId);
    }
}
