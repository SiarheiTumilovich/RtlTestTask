using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Providers.ShowProvider.Entities;

namespace BusinessLayer.Providers.ShowGrabbing
{
    public interface IShowGrabber
    {
        Task<ICollection<Show>> Grab(int fromPage, int toPage);
    }
}
