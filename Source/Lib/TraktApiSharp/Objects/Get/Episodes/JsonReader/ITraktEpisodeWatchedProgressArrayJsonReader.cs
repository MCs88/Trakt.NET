﻿namespace TraktApiSharp.Objects.Get.Episodes.JsonReader
{
    using Newtonsoft.Json;
    using Objects.JsonReader;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    internal class ITraktEpisodeWatchedProgressArrayJsonReader : ITraktArrayJsonReader<ITraktEpisodeWatchedProgress>
    {
        public Task<IEnumerable<ITraktEpisodeWatchedProgress>> ReadArrayAsync(string json, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(json))
                return null;

            using (var reader = new StringReader(json))
            using (var jsonReader = new JsonTextReader(reader))
            {
                return ReadArrayAsync(jsonReader, cancellationToken);
            }
        }

        public async Task<IEnumerable<ITraktEpisodeWatchedProgress>> ReadArrayAsync(JsonTextReader jsonReader, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (jsonReader == null)
                return null;

            if (await jsonReader.ReadAsync(cancellationToken) && jsonReader.TokenType == JsonToken.StartArray)
            {
                var episodeWatchedProgressReader = new ITraktEpisodeWatchedProgressObjectJsonReader();
                //var episodeWatchedProgressReadingTasks = new List<Task<ITraktEpisodeWatchedProgress>>();
                var traktEpisodeWatchedProgresses = new List<ITraktEpisodeWatchedProgress>();

                //episodeWatchedProgressReadingTasks.Add(episodeWatchedProgressReader.ReadObjectAsync(jsonReader, cancellationToken));
                ITraktEpisodeWatchedProgress traktEpisodeWatchedProgress = await episodeWatchedProgressReader.ReadObjectAsync(jsonReader, cancellationToken);

                while (traktEpisodeWatchedProgress != null)
                {
                    traktEpisodeWatchedProgresses.Add(traktEpisodeWatchedProgress);
                    //episodeWatchedProgressReadingTasks.Add(episodeWatchedProgressReader.ReadObjectAsync(jsonReader, cancellationToken));
                    traktEpisodeWatchedProgress = await episodeWatchedProgressReader.ReadObjectAsync(jsonReader, cancellationToken);
                }

                //var readEpisodeWatchedProgresses = await Task.WhenAll(episodeWatchedProgressReadingTasks);
                //return (IEnumerable<ITraktEpisodeWatchedProgress>)readEpisodeWatchedProgresses.GetEnumerator();
                return traktEpisodeWatchedProgresses;
            }

            return null;
        }
    }
}
