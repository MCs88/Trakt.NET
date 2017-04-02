﻿namespace TraktApiSharp.Objects.Get.Episodes.JsonReader
{
    using Newtonsoft.Json;
    using Objects.JsonReader;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    internal class ITraktEpisodeArrayJsonReader : ITraktArrayJsonReader<ITraktEpisode>
    {
        public Task<IEnumerable<ITraktEpisode>> ReadArrayAsync(string json, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(json))
                return null;

            using (var reader = new StringReader(json))
            using (var jsonReader = new JsonTextReader(reader))
            {
                return ReadArrayAsync(jsonReader, cancellationToken);
            }
        }

        public async Task<IEnumerable<ITraktEpisode>> ReadArrayAsync(JsonTextReader jsonReader, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (jsonReader == null)
                return null;

            if (await jsonReader.ReadAsync(cancellationToken) && jsonReader.TokenType == JsonToken.StartArray)
            {
                var episodeReader = new ITraktEpisodeObjectJsonReader();
                //var traktEpisodeReadingTasks = new List<Task<ITraktEpisode>>();
                var traktEpisodes = new List<ITraktEpisode>();

                //traktEpisodeReadingTasks.Add(episodeReader.ReadObjectAsync(jsonReader, cancellationToken));
                ITraktEpisode traktEpisode = await episodeReader.ReadObjectAsync(jsonReader, cancellationToken);

                while (traktEpisode != null)
                {
                    traktEpisodes.Add(traktEpisode);
                    //traktEpisodeReadingTasks.Add(episodeReader.ReadObjectAsync(jsonReader, cancellationToken));
                    traktEpisode = await episodeReader.ReadObjectAsync(jsonReader, cancellationToken);
                }

                //var readEpisodes = await Task.WhenAll(traktEpisodeReadingTasks);
                //return (IEnumerable<ITraktEpisode>)readEpisodes.GetEnumerator();
                return traktEpisodes;
            }

            return null;
        }
    }
}
