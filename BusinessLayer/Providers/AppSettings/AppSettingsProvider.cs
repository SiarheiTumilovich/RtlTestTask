using Microsoft.Extensions.Configuration;

namespace BusinessLayer.Providers.AppSettings
{
    internal class AppSettingsProvider : IAppSettingsProvider
    {
        private readonly IConfigurationSection _appSettingsSection;

        public AppSettingsProvider(IConfiguration configuration)
        {
            _appSettingsSection = configuration.GetSection("AppSettings");
        }

        public string TvMazeApiBaseEndpoint => _appSettingsSection[nameof(TvMazeApiBaseEndpoint)];
    }
}
