﻿namespace TraktNet.Objects.Post.Checkins.Json.Factories
{
    using Objects.Json;
    using Reader;
    using Writer;

    internal class EpisodeCheckinPostJsonIOFactory : IJsonIOFactory<ITraktEpisodeCheckinPost>
    {
        public IObjectJsonReader<ITraktEpisodeCheckinPost> CreateObjectReader() => new EpisodeCheckinPostObjectJsonReader();

        public IArrayJsonReader<ITraktEpisodeCheckinPost> CreateArrayReader() => new EpisodeCheckinPostArrayJsonReader();

        public IObjectJsonWriter<ITraktEpisodeCheckinPost> CreateObjectWriter() => new EpisodeCheckinPostObjectJsonWriter();
    }
}
