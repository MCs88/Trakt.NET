﻿namespace TraktApiSharp.Objects.Basic.Json.Writer
{
    using Newtonsoft.Json;
    using Objects.Json;
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    internal class IdsObjectJsonWriter : IObjectJsonWriter<ITraktIds>
    {
        public Task<string> WriteObjectAsync(ITraktIds obj, CancellationToken cancellationToken = default)
        {
            using (var writer = new StringWriter())
            {
                return WriteObjectAsync(writer, obj, cancellationToken);
            }
        }

        public async Task<string> WriteObjectAsync(StringWriter writer, ITraktIds obj, CancellationToken cancellationToken = default)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            using (var jsonWriter = new JsonTextWriter(writer))
            {
                await WriteObjectAsync(jsonWriter, obj, cancellationToken);
            }

            return writer.ToString();
        }

        public async Task WriteObjectAsync(JsonTextWriter jsonWriter, ITraktIds obj, CancellationToken cancellationToken = default)
        {
            if (jsonWriter == null)
                throw new ArgumentNullException(nameof(jsonWriter));

            await jsonWriter.WriteStartObjectAsync(cancellationToken);

            await jsonWriter.WritePropertyNameAsync(JsonProperties.IDS_PROPERTY_NAME_TRAKT, cancellationToken);
            await jsonWriter.WriteValueAsync(obj.Trakt);

            if (!string.IsNullOrEmpty(obj.Slug))
            {
                await jsonWriter.WritePropertyNameAsync(JsonProperties.IDS_PROPERTY_NAME_SLUG, cancellationToken);
                await jsonWriter.WriteValueAsync(obj.Slug, cancellationToken);
            }

            if (obj.Tvdb.HasValue)
            {
                await jsonWriter.WritePropertyNameAsync(JsonProperties.IDS_PROPERTY_NAME_TVDB, cancellationToken);
                await jsonWriter.WriteValueAsync(obj.Tvdb);
            }

            if (!string.IsNullOrEmpty(obj.Imdb))
            {
                await jsonWriter.WritePropertyNameAsync(JsonProperties.IDS_PROPERTY_NAME_IMDB, cancellationToken);
                await jsonWriter.WriteValueAsync(obj.Imdb, cancellationToken);
            }

            if (obj.Tmdb.HasValue)
            {
                await jsonWriter.WritePropertyNameAsync(JsonProperties.IDS_PROPERTY_NAME_TMDB, cancellationToken);
                await jsonWriter.WriteValueAsync(obj.Tmdb);
            }

            if (obj.TvRage.HasValue)
            {
                await jsonWriter.WritePropertyNameAsync(JsonProperties.IDS_PROPERTY_NAME_TVRAGE, cancellationToken);
                await jsonWriter.WriteValueAsync(obj.TvRage);
            }

            await jsonWriter.WriteEndObjectAsync(cancellationToken);
        }
    }
}
