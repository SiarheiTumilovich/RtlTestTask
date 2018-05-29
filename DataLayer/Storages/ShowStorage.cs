using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Domains;
using DataLayer.Storages.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Storages
{
    internal class ShowStorage : IShowStorage
    {
        private readonly IMyDbContext _myDbContext;

        public ShowStorage(IMyDbContext myDbContext)
        {
            _myDbContext = myDbContext;
        }

        public async Task AddShows(ICollection<Show> shows)
        {
            if (shows == null || !shows.Any())
                return;

            await AddAbsentShows(shows);
            await AddAbsentPeople(shows.SelectMany(s => s.People.Select(p => p.Person)).ToList());
            await AddAbsentAssocs(shows.SelectMany(s => s.People).ToList());

            await _myDbContext.Save();
        }

        private async Task AddAbsentShows(ICollection<Show> shows)
        {
            var showIds = shows.Select(s => s.ShowId);
            var existingShowIds = await _myDbContext.Shows
                .Where(s => showIds.Contains(s.ShowId))
                .Select(s => s.ShowId)
                .ToListAsync();

            var newShows = shows
                .Where(s => !existingShowIds.Contains(s.ShowId))
                .GroupBy(s => s.ShowId)
                .Select(g => g.First());
            foreach (var newShow in newShows)
                _myDbContext.Shows.Add(new Show { ShowId = newShow.ShowId, Name = newShow.Name });
        }

        private async Task AddAbsentPeople(ICollection<Person> people)
        {
            var personIds = people.Select(p => p.PersonId);
            var existingPeopleIds = await _myDbContext.People
                .Where(p => personIds.Contains(p.PersonId))
                .Select(p => p.PersonId)
                .ToListAsync();

            var newPeople = people
                .Where(p => !existingPeopleIds.Contains(p.PersonId))
                .GroupBy(p => p.PersonId)
                .Select(g => g.First());
            foreach (var newPerson in newPeople)
                _myDbContext.People.Add(new Person { PersonId = newPerson.PersonId, Name = newPerson.Name, Birthday = newPerson.Birthday });
        }

        private async Task AddAbsentAssocs(ICollection<ShowPersonAssoc> accocs)
        {
            var assocIds = accocs.Select(a => a.PersonId * 10000 + a.ShowId);
            var existingAssocs = await _myDbContext.ShowPersonAssocs
                .Where(a => assocIds.Contains(a.PersonId * 10000 + a.ShowId))
                .Select(a => new { a.ShowId, a.PersonId })
                .ToListAsync();

            var newAssocs = accocs
                .Where(a => !existingAssocs.Contains(new { a.ShowId, a.PersonId }))
                .GroupBy(a => new { a.ShowId, a.PersonId })
                .Select(g => g.First());
            foreach (var newAssoc in newAssocs)
                _myDbContext.ShowPersonAssocs.Add(new ShowPersonAssoc { ShowId = newAssoc.ShowId, PersonId = newAssoc.PersonId });
        }

        public async Task<ICollection<Show>> GetShows(int pageNum, int pageSize)
        {
            return await _myDbContext.Shows
                .Include(x => x.People)
                .ThenInclude(x => x.Person)
                .OrderBy(x => x.ShowId)
                .Skip(pageNum * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetShowsCount()
        {
            return await _myDbContext.Shows
                .CountAsync();
        }

        public async Task Delete(int showId)
        {
            var show = await _myDbContext.Shows
                .Include(s => s.People)
                .FirstOrDefaultAsync(s => s.ShowId == showId);

            _myDbContext.Shows.Remove(show);

            await _myDbContext.Save();
        }
    }
}
