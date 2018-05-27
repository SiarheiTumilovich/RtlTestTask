using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.Providers.ShowGrabbing;
using BusinessLayer.Providers.ShowProvider;
using Microsoft.AspNetCore.Mvc;
using WebApi.Entities;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/shows")]
    public class ShowController : Controller
    {
        private readonly IShowProvider _showProvider;
        private readonly IShowGrabberFactory _showGrabberFactory;

        public ShowController(IShowProvider showProvider, IShowGrabberFactory showGrabberFactory)
        {
            _showProvider = showProvider;
            _showGrabberFactory = showGrabberFactory;
        }

        [HttpGet("all")]
        public async Task<ICollection<Show>> GetShows(int pageNum = 0, int pageSize = 10)
        {
            if (pageNum < 0)
                throw new ArgumentException(nameof(pageNum));

            if (pageSize < 0)
                throw new ArgumentException(nameof(pageSize));

            var searchResults = await _showProvider.GetShows(pageNum, pageSize);

            return Mapper.Map<ICollection<Show>>(searchResults.Items);
        }

        [HttpPost]
        public async Task GrabData(string grabberName = "tvMaze", int fromPage = 0, int? toPage = null)
        {
            if (fromPage < 0)
                throw new ArgumentException(nameof(fromPage));

            if (toPage == null)
                toPage = fromPage + 1;
            else if (toPage <= fromPage)
                throw new ArgumentException(nameof(toPage));

            var grabber = _showGrabberFactory.CreateGrabber(grabberName);

            var newShows = await grabber.Grab(fromPage, toPage.Value);

            await _showProvider.SaveShows(newShows);
        }

        [HttpDelete("{showId}")]
        public async Task DeleteAll(int showId)
        {
            if (showId <= 0)
                throw new ArgumentException(nameof(showId));

            await _showProvider.Delete(showId);
        }
    }
}