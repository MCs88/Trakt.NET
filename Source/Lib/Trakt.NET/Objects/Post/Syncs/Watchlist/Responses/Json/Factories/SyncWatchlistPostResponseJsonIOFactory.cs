﻿namespace TraktNet.Objects.Post.Syncs.Watchlist.Responses.Json.Factories
{
    using Objects.Json;
    using Reader;
    using Writer;

    internal class SyncWatchlistPostResponseJsonIOFactory : IJsonIOFactory<ITraktSyncWatchlistPostResponse>
    {
        public IObjectJsonReader<ITraktSyncWatchlistPostResponse> CreateObjectReader() => new SyncWatchlistPostResponseObjectJsonReader();

        public IArrayJsonReader<ITraktSyncWatchlistPostResponse> CreateArrayReader() => new SyncWatchlistPostResponseArrayJsonReader();

        public IObjectJsonWriter<ITraktSyncWatchlistPostResponse> CreateObjectWriter() => new SyncWatchlistPostResponseObjectJsonWriter();
    }
}
