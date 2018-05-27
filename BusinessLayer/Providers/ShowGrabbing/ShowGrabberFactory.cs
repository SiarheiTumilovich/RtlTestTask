using System;
using BusinessLayer.Providers.ShowGrabbing.Grabbers;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLayer.Providers.ShowGrabbing
{
    internal class ShowGrabberFactory : IShowGrabberFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ShowGrabberFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IShowGrabber CreateGrabber(string dataSourceName)
        {
            if (string.Equals(dataSourceName, "tvMaze"))
                return _serviceProvider.GetService<TvMazeShowGrabber>();
            throw new NotSupportedException(dataSourceName);
        }
    }
}
