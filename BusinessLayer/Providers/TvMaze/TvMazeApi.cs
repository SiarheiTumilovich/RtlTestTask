using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using BusinessLayer.Providers.AppSettings;
using BusinessLayer.Providers.TvMaze.Entities;
using Newtonsoft.Json;

namespace BusinessLayer.Providers.TvMaze
{
    internal class TvMazeApi : ITvMazeApi
    {
        private readonly IAppSettingsProvider _appSettingsProvider;

        public TvMazeApi(IAppSettingsProvider appSettingsProvider)
        {
            _appSettingsProvider = appSettingsProvider;
        }

        public async Task<ICollection<TvMazeShow>> GetShows(int pageNum)
        {
            var uri = $"{_appSettingsProvider.TvMazeApiBaseEndpoint}/shows?page={pageNum}";

            return await GetResponse<TvMazeShow[]>(uri);
        }

        public async Task<ICollection<TvMazeCast>> GetCasts(int showId)
        {
            var uri = $"{_appSettingsProvider.TvMazeApiBaseEndpoint}/shows/{showId}/cast";

            return await GetResponse<TvMazeCast[]>(uri);
        }

        private async Task<TResponse> GetResponse<TResponse>(string uri)
        {
            var httpClient = WebRequest.Create(uri);

            using (var req = await httpClient.GetResponseAsync())
            {
                var responseStream = req.GetResponseStream();
                if (responseStream == null)
                    throw new InvalidOperationException();

                return ReadFromStream<TResponse>(responseStream);
            }
        }

        private T ReadFromStream<T>(Stream stream)
        {
            var jsonSerializer = new JsonSerializer();
            jsonSerializer.DateFormatString = "yyyy-MM-dd";

            using (var streamReader = new StreamReader(stream))
            {
                using (var jsonReader = new JsonTextReader(streamReader))
                {
                    return jsonSerializer.Deserialize<T>(jsonReader);
                }
            }
        }
    }
}
