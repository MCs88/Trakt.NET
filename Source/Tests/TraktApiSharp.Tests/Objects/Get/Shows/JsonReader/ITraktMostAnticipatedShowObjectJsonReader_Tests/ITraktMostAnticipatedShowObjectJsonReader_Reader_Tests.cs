﻿namespace TraktApiSharp.Tests.Objects.Get.Shows.JsonReader
{
    using FluentAssertions;
    using Newtonsoft.Json;
    using System.IO;
    using System.Threading.Tasks;
    using Traits;
    using TraktApiSharp.Objects.Get.Shows.JsonReader;
    using Xunit;

    [Category("Objects.Get.Shows.JsonReader")]
    public partial class ITraktMostAnticipatedShowObjectJsonReader_Tests
    {
        [Fact]
        public async Task Test_ITraktMostAnticipatedShowObjectJsonReader_ReadObject_From_JsonReader_Complete()
        {
            var traktJsonReader = new ITraktMostAnticipatedShowObjectJsonReader();

            using (var reader = new StringReader(JSON_COMPLETE))
            using (var jsonReader = new JsonTextReader(reader))
            {
                var traktMostAnticipatedShow = await traktJsonReader.ReadObjectAsync(jsonReader);

                traktMostAnticipatedShow.Should().NotBeNull();
                traktMostAnticipatedShow.ListCount.Should().Be(12805);
                traktMostAnticipatedShow.Show.Should().NotBeNull();
                traktMostAnticipatedShow.Show.Title.Should().Be("Game of Thrones");
                traktMostAnticipatedShow.Show.Year.Should().Be(2011);
                traktMostAnticipatedShow.Show.Ids.Should().NotBeNull();
                traktMostAnticipatedShow.Show.Ids.Trakt.Should().Be(1390U);
                traktMostAnticipatedShow.Show.Ids.Slug.Should().Be("game-of-thrones");
                traktMostAnticipatedShow.Show.Ids.Tvdb.Should().Be(121361U);
                traktMostAnticipatedShow.Show.Ids.Imdb.Should().Be("tt0944947");
                traktMostAnticipatedShow.Show.Ids.Tmdb.Should().Be(1399U);
                traktMostAnticipatedShow.Show.Ids.TvRage.Should().Be(24493U);
            }
        }

        [Fact]
        public async Task Test_ITraktMostAnticipatedShowObjectJsonReader_ReadObject_From_JsonReader_Incomplete_1()
        {
            var traktJsonReader = new ITraktMostAnticipatedShowObjectJsonReader();

            using (var reader = new StringReader(JSON_INCOMPLETE_1))
            using (var jsonReader = new JsonTextReader(reader))
            {
                var traktMostAnticipatedShow = await traktJsonReader.ReadObjectAsync(jsonReader);

                traktMostAnticipatedShow.Should().NotBeNull();
                traktMostAnticipatedShow.ListCount.Should().Be(12805);
                traktMostAnticipatedShow.Show.Should().BeNull();
            }
        }

        [Fact]
        public async Task Test_ITraktMostAnticipatedShowObjectJsonReader_ReadObject_From_JsonReader_Incomplete_2()
        {
            var traktJsonReader = new ITraktMostAnticipatedShowObjectJsonReader();

            using (var reader = new StringReader(JSON_INCOMPLETE_2))
            using (var jsonReader = new JsonTextReader(reader))
            {
                var traktMostAnticipatedShow = await traktJsonReader.ReadObjectAsync(jsonReader);

                traktMostAnticipatedShow.Should().NotBeNull();
                traktMostAnticipatedShow.ListCount.Should().BeNull();
                traktMostAnticipatedShow.Show.Should().NotBeNull();
                traktMostAnticipatedShow.Show.Title.Should().Be("Game of Thrones");
                traktMostAnticipatedShow.Show.Year.Should().Be(2011);
                traktMostAnticipatedShow.Show.Ids.Should().NotBeNull();
                traktMostAnticipatedShow.Show.Ids.Trakt.Should().Be(1390U);
                traktMostAnticipatedShow.Show.Ids.Slug.Should().Be("game-of-thrones");
                traktMostAnticipatedShow.Show.Ids.Tvdb.Should().Be(121361U);
                traktMostAnticipatedShow.Show.Ids.Imdb.Should().Be("tt0944947");
                traktMostAnticipatedShow.Show.Ids.Tmdb.Should().Be(1399U);
                traktMostAnticipatedShow.Show.Ids.TvRage.Should().Be(24493U);
            }
        }

        [Fact]
        public async Task Test_ITraktMostAnticipatedShowObjectJsonReader_ReadObject_From_JsonReader_Not_Valid_1()
        {
            var traktJsonReader = new ITraktMostAnticipatedShowObjectJsonReader();

            using (var reader = new StringReader(JSON_NOT_VALID_1))
            using (var jsonReader = new JsonTextReader(reader))
            {
                var traktMostAnticipatedShow = await traktJsonReader.ReadObjectAsync(jsonReader);

                traktMostAnticipatedShow.Should().NotBeNull();
                traktMostAnticipatedShow.ListCount.Should().BeNull();
                traktMostAnticipatedShow.Show.Should().NotBeNull();
                traktMostAnticipatedShow.Show.Title.Should().Be("Game of Thrones");
                traktMostAnticipatedShow.Show.Year.Should().Be(2011);
                traktMostAnticipatedShow.Show.Ids.Should().NotBeNull();
                traktMostAnticipatedShow.Show.Ids.Trakt.Should().Be(1390U);
                traktMostAnticipatedShow.Show.Ids.Slug.Should().Be("game-of-thrones");
                traktMostAnticipatedShow.Show.Ids.Tvdb.Should().Be(121361U);
                traktMostAnticipatedShow.Show.Ids.Imdb.Should().Be("tt0944947");
                traktMostAnticipatedShow.Show.Ids.Tmdb.Should().Be(1399U);
                traktMostAnticipatedShow.Show.Ids.TvRage.Should().Be(24493U);
            }
        }

        [Fact]
        public async Task Test_ITraktMostAnticipatedShowObjectJsonReader_ReadObject_From_JsonReader_Not_Valid_2()
        {
            var traktJsonReader = new ITraktMostAnticipatedShowObjectJsonReader();

            using (var reader = new StringReader(JSON_NOT_VALID_2))
            using (var jsonReader = new JsonTextReader(reader))
            {
                var traktMostAnticipatedShow = await traktJsonReader.ReadObjectAsync(jsonReader);

                traktMostAnticipatedShow.Should().NotBeNull();
                traktMostAnticipatedShow.ListCount.Should().Be(12805);
                traktMostAnticipatedShow.Show.Should().BeNull();
            }
        }

        [Fact]
        public async Task Test_ITraktMostAnticipatedShowObjectJsonReader_ReadObject_From_JsonReader_Not_Valid_3()
        {
            var traktJsonReader = new ITraktMostAnticipatedShowObjectJsonReader();

            using (var reader = new StringReader(JSON_NOT_VALID_3))
            using (var jsonReader = new JsonTextReader(reader))
            {
                var traktMostAnticipatedShow = await traktJsonReader.ReadObjectAsync(jsonReader);

                traktMostAnticipatedShow.Should().NotBeNull();
                traktMostAnticipatedShow.ListCount.Should().BeNull();
                traktMostAnticipatedShow.Show.Should().BeNull();
            }
        }

        [Fact]
        public async Task Test_ITraktMostAnticipatedShowObjectJsonReader_ReadObject_From_JsonReader_Null()
        {
            var traktJsonReader = new ITraktMostAnticipatedShowObjectJsonReader();

            var traktMostAnticipatedShow = await traktJsonReader.ReadObjectAsync(default(JsonTextReader));
            traktMostAnticipatedShow.Should().BeNull();
        }

        [Fact]
        public async Task Test_ITraktMostAnticipatedShowObjectJsonReader_ReadObject_From_JsonReader_Empty()
        {
            var traktJsonReader = new ITraktMostAnticipatedShowObjectJsonReader();

            using (var reader = new StringReader(string.Empty))
            using (var jsonReader = new JsonTextReader(reader))
            {
                var traktMostAnticipatedShow = await traktJsonReader.ReadObjectAsync(jsonReader);
                traktMostAnticipatedShow.Should().BeNull();
            }
        }
    }
}
