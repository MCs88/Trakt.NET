﻿namespace TraktApiSharp.Objects.Get.Collections.JsonReader.Factories
{
    using Objects.JsonReader;

    internal class TraktCollectionShowJsonReaderFactory : ITraktJsonReaderFactory<ITraktCollectionShow>
    {
        public ITraktObjectJsonReader<ITraktCollectionShow> CreateObjectReader() => new TraktCollectionShowObjectJsonReader();

        public ITraktArrayJsonReader<ITraktCollectionShow> CreateArrayReader() => new TraktCollectionShowArrayJsonReader();
    }
}