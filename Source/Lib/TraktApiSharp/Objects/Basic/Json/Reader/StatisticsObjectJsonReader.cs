﻿namespace TraktApiSharp.Objects.Basic.Json.Reader
{
    using Implementations;
    using Newtonsoft.Json;
    using Objects.Json;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    internal class StatisticsObjectJsonReader : IObjectJsonReader<ITraktStatistics>
    {
        public Task<ITraktStatistics> ReadObjectAsync(string json, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(json))
                return Task.FromResult(default(ITraktStatistics));

            using (var reader = new StringReader(json))
            using (var jsonReader = new JsonTextReader(reader))
            {
                return ReadObjectAsync(jsonReader, cancellationToken);
            }
        }

        public Task<ITraktStatistics> ReadObjectAsync(Stream stream, CancellationToken cancellationToken = default)
        {
            if (stream == null)
                return Task.FromResult(default(ITraktStatistics));

            using (var streamReader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                return ReadObjectAsync(jsonReader, cancellationToken);
            }
        }

        public async Task<ITraktStatistics> ReadObjectAsync(JsonTextReader jsonReader, CancellationToken cancellationToken = default)
        {
            if (jsonReader == null)
                return await Task.FromResult(default(ITraktStatistics));

            if (await jsonReader.ReadAsync(cancellationToken) && jsonReader.TokenType == JsonToken.StartObject)
            {
                ITraktStatistics traktStatistics = new TraktStatistics();

                while (await jsonReader.ReadAsync(cancellationToken) && jsonReader.TokenType == JsonToken.PropertyName)
                {
                    var propertyName = jsonReader.Value.ToString();

                    switch (propertyName)
                    {
                        case JsonProperties.STATISTICS_PROPERTY_NAME_WATCHERS:
                            traktStatistics.Watchers = await jsonReader.ReadAsInt32Async(cancellationToken);
                            break;
                        case JsonProperties.STATISTICS_PROPERTY_NAME_PLAYS:
                            traktStatistics.Plays = await jsonReader.ReadAsInt32Async(cancellationToken);
                            break;
                        case JsonProperties.STATISTICS_PROPERTY_NAME_COLLECTORS:
                            traktStatistics.Collectors = await jsonReader.ReadAsInt32Async(cancellationToken);
                            break;
                        case JsonProperties.STATISTICS_PROPERTY_NAME_COLLECTED_EPISODES:
                            traktStatistics.CollectedEpisodes = await jsonReader.ReadAsInt32Async(cancellationToken);
                            break;
                        case JsonProperties.STATISTICS_PROPERTY_NAME_COMMENTS:
                            traktStatistics.Comments = await jsonReader.ReadAsInt32Async(cancellationToken);
                            break;
                        case JsonProperties.STATISTICS_PROPERTY_NAME_LISTS:
                            traktStatistics.Lists = await jsonReader.ReadAsInt32Async(cancellationToken);
                            break;
                        case JsonProperties.STATISTICS_PROPERTY_NAME_VOTES:
                            traktStatistics.Votes = await jsonReader.ReadAsInt32Async(cancellationToken);
                            break;
                        default:
                            await JsonReaderHelper.ReadAndIgnoreInvalidContentAsync(jsonReader, cancellationToken);
                            break;
                    }
                }

                return traktStatistics;
            }

            return await Task.FromResult(default(ITraktStatistics));
        }
    }
}
