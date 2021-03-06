﻿namespace TraktNet.Objects.Post.Syncs.Watchlist.Json.Reader
{
    using Newtonsoft.Json;
    using Objects.Json;
    using System.Threading;
    using System.Threading.Tasks;

    internal class SyncWatchlistPostObjectJsonReader : AObjectJsonReader<ITraktSyncWatchlistPost>
    {
        public override async Task<ITraktSyncWatchlistPost> ReadObjectAsync(JsonTextReader jsonReader, CancellationToken cancellationToken = default)
        {
            if (jsonReader == null)
                return await Task.FromResult(default(ITraktSyncWatchlistPost));

            if (await jsonReader.ReadAsync(cancellationToken) && jsonReader.TokenType == JsonToken.StartObject)
            {
                var movieArrayJsonReader = new SyncWatchlistPostMovieArrayJsonReader();
                var showArrayJsonReader = new SyncWatchlistPostShowArrayJsonReader();
                var episodeArrayJsonReader = new SyncWatchlistPostEpisodeArrayJsonReader();
                ITraktSyncWatchlistPost syncWatchlistPost = new TraktSyncWatchlistPost();

                while (await jsonReader.ReadAsync(cancellationToken) && jsonReader.TokenType == JsonToken.PropertyName)
                {
                    var propertyName = jsonReader.Value.ToString();

                    switch (propertyName)
                    {
                        case JsonProperties.SYNC_WATCHLIST_POST_PROPERTY_NAME_MOVIES:
                            syncWatchlistPost.Movies = await movieArrayJsonReader.ReadArrayAsync(jsonReader, cancellationToken);
                            break;
                        case JsonProperties.SYNC_WATCHLIST_POST_PROPERTY_NAME_SHOWS:
                            syncWatchlistPost.Shows = await showArrayJsonReader.ReadArrayAsync(jsonReader, cancellationToken);
                            break;
                        case JsonProperties.SYNC_WATCHLIST_POST_PROPERTY_NAME_EPISODES:
                            syncWatchlistPost.Episodes = await episodeArrayJsonReader.ReadArrayAsync(jsonReader, cancellationToken);
                            break;
                        default:
                            await JsonReaderHelper.ReadAndIgnoreInvalidContentAsync(jsonReader, cancellationToken);
                            break;
                    }
                }

                return syncWatchlistPost;
            }

            return await Task.FromResult(default(ITraktSyncWatchlistPost));
        }
    }
}
