namespace BusinessLayer.Providers.ShowGrabbing
{
    public interface IShowGrabberFactory
    {
        IShowGrabber CreateGrabber(string dataSourceName);
    }
}
