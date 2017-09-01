﻿namespace TraktApiSharp.Objects.Get.Movies.JsonReader.Factories
{
    using Objects.JsonReader;

    internal class MovieJsonReaderFactory : IJsonReaderFactory<ITraktMovie>
    {
        public IObjectJsonReader<ITraktMovie> CreateObjectReader() => new TraktMovieObjectJsonReader();

        public IArrayJsonReader<ITraktMovie> CreateArrayReader() => new MovieArrayJsonReader();
    }
}
