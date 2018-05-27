using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.Entities;
using BusinessLayer.Providers.ShowProvider.Entities;
using DataLayer.Storages;

namespace BusinessLayer.Providers.ShowProvider
{
    internal class ShowProvider : IShowProvider
    {
        private readonly IShowStorage _showStorage;

        public ShowProvider(IShowStorage showStorage)
        {
            _showStorage = showStorage;
        }

        public async Task<SearchResults<Show>> GetShows(int pageNum, int pageSize)
        {
            var shows = await _showStorage.GetShows(pageNum, pageSize);

            return new SearchResults<Show>
            {
                PageNum = pageNum,
                PageSize = pageSize,
                TotalCount = await _showStorage.GetShowsCount(),
                Items = Mapper.Map<ICollection<Show>>(shows)
            };
        }

        public async Task SaveShows(ICollection<Show> newShows)
        {
            var newDomains = Mapper.Map<ICollection<DataLayer.Domains.Show>>(newShows);

            await _showStorage.AddShows(newDomains);
        }

        public async Task Delete(int showId)
        {
            await _showStorage.Delete(showId);
        }
    }
}
