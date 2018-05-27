using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.Providers.ShowProvider.Entities;
using BusinessLayer.Providers.TvMaze;
using Microsoft.Extensions.Logging;

namespace BusinessLayer.Providers.ShowGrabbing.Grabbers
{
    internal class TvMazeShowGrabber : IShowGrabber
    {
        private readonly ITvMazeApi _tvMazeApi;
        private readonly ILogger _logger;

        public TvMazeShowGrabber(ITvMazeApi tvMazeApi, ILoggerFactory loggerFactory)
        {
            _tvMazeApi = tvMazeApi;
            _logger = loggerFactory.CreateLogger<TvMazeShowGrabber>();
        }

        public async Task<ICollection<Show>> Grab(int fromPage, int toPage)
        {
            var shows = new List<Show>();

            for (int pageNum = fromPage; pageNum < toPage; ++pageNum)
            {
                shows.AddRange(
                    await Grab(pageNum)
                );
            }

            return shows.ToArray();
        }

        private async Task<ICollection<Show>> Grab(int pageNum)
        {
            var shows = new ConcurrentBag<Show>();

            var sync = new SemaphoreSlim(5, 5);

            var tasks = new List<Task>();

            var tvMazeShows = await _tvMazeApi.GetShows(pageNum);

            foreach (var tvMazeShow in tvMazeShows)
            {
                await sync.WaitAsync();

                tasks.Add(
                    AttemptExecution(async () =>
                        {
                            var tvMazeCasts = await _tvMazeApi.GetCasts(tvMazeShow.Id);
                            var show = Mapper.Map<Show>(tvMazeShow);
                            show.People = Mapper.Map<ICollection<Person>>(tvMazeCasts);
                            shows.Add(show);
                        }, 3, 9000)
                        .ContinueWith(t =>
                        {
                            sync.Release();
                        })
                );
            }

            await Task.WhenAll(tasks);

            return shows.ToArray();
        }

        private async Task AttemptExecution(Func<Task> task, int attemptCount, int delayBetweenAttempts)
        {
            for (int i = 0; i < attemptCount; ++i)
            {
                try
                {
                    await task();
                    return;
                }
                catch(Exception ex)
                {
                    if (i >= attemptCount - 1)
                    {
                        _logger.LogError(ex, ex.Message);
                        throw;
                    }
                    await Task.Delay(delayBetweenAttempts);
                }
            }
        }
    }
}
