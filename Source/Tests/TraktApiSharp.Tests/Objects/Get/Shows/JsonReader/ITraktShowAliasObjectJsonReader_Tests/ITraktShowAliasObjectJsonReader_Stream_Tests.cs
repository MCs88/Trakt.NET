﻿namespace TraktApiSharp.Tests.Objects.Get.Shows.JsonReader
{
    using FluentAssertions;
    using System.IO;
    using System.Threading.Tasks;
    using TestUtils;
    using Traits;
    using TraktApiSharp.Objects.Get.Shows.JsonReader;
    using Xunit;

    [Category("Objects.Get.Shows.JsonReader")]
    public partial class ITraktShowAliasObjectJsonReader_Tests
    {
        [Fact]
        public async Task Test_ITraktShowAliasObjectJsonReader_ReadObject_From_Stream_Complete()
        {
            var traktJsonReader = new ITraktShowAliasObjectJsonReader();

            using (var stream = JSON_COMPLETE.ToStream())
            {
                var traktShowAlias = await traktJsonReader.ReadObjectAsync(stream);

                traktShowAlias.Should().NotBeNull();
                traktShowAlias.Title.Should().Be("Game of Thrones- Das Lied von Eis und Feuer");
                traktShowAlias.CountryCode.Should().Be("de");
            }
        }

        [Fact]
        public async Task Test_ITraktShowAliasObjectJsonReader_ReadObject_From_Stream_Incomplete_1()
        {
            var traktJsonReader = new ITraktShowAliasObjectJsonReader();

            using (var stream = JSON_INCOMPLETE_1.ToStream())
            {
                var traktShowAlias = await traktJsonReader.ReadObjectAsync(stream);

                traktShowAlias.Should().NotBeNull();
                traktShowAlias.Title.Should().Be("Game of Thrones- Das Lied von Eis und Feuer");
                traktShowAlias.CountryCode.Should().BeNull();
            }
        }

        [Fact]
        public async Task Test_ITraktShowAliasObjectJsonReader_ReadObject_From_Stream_Incomplete_2()
        {
            var traktJsonReader = new ITraktShowAliasObjectJsonReader();

            using (var stream = JSON_INCOMPLETE_2.ToStream())
            {
                var traktShowAlias = await traktJsonReader.ReadObjectAsync(stream);

                traktShowAlias.Should().NotBeNull();
                traktShowAlias.Title.Should().BeNull();
                traktShowAlias.CountryCode.Should().Be("de");
            }
        }

        [Fact]
        public async Task Test_ITraktShowAliasObjectJsonReader_ReadObject_From_Stream_Not_Valid_1()
        {
            var traktJsonReader = new ITraktShowAliasObjectJsonReader();

            using (var stream = JSON_NOT_VALID_1.ToStream())
            {
                var traktShowAlias = await traktJsonReader.ReadObjectAsync(stream);

                traktShowAlias.Should().NotBeNull();
                traktShowAlias.Title.Should().BeNull();
                traktShowAlias.CountryCode.Should().Be("de");
            }
        }

        [Fact]
        public async Task Test_ITraktShowAliasObjectJsonReader_ReadObject_From_Stream_Not_Valid_2()
        {
            var traktJsonReader = new ITraktShowAliasObjectJsonReader();

            using (var stream = JSON_NOT_VALID_2.ToStream())
            {
                var traktShowAlias = await traktJsonReader.ReadObjectAsync(stream);

                traktShowAlias.Should().NotBeNull();
                traktShowAlias.Title.Should().Be("Game of Thrones- Das Lied von Eis und Feuer");
                traktShowAlias.CountryCode.Should().BeNull();
            }
        }

        [Fact]
        public async Task Test_ITraktShowAliasObjectJsonReader_ReadObject_From_Stream_Not_Valid_3()
        {
            var traktJsonReader = new ITraktShowAliasObjectJsonReader();

            using (var stream = JSON_NOT_VALID_3.ToStream())
            {
                var traktShowAlias = await traktJsonReader.ReadObjectAsync(stream);

                traktShowAlias.Should().NotBeNull();
                traktShowAlias.Title.Should().BeNull();
                traktShowAlias.CountryCode.Should().BeNull();
            }
        }

        [Fact]
        public async Task Test_ITraktShowAliasObjectJsonReader_ReadObject_From_Stream_Null()
        {
            var traktJsonReader = new ITraktShowAliasObjectJsonReader();

            var traktShowAlias = await traktJsonReader.ReadObjectAsync(default(Stream));
            traktShowAlias.Should().BeNull();
        }

        [Fact]
        public async Task Test_ITraktShowAliasObjectJsonReader_ReadObject_From_Stream_Empty()
        {
            var traktJsonReader = new ITraktShowAliasObjectJsonReader();

            using (var stream = string.Empty.ToStream())
            {
                var traktShowAlias = await traktJsonReader.ReadObjectAsync(stream);
                traktShowAlias.Should().BeNull();
            }
        }
    }
}
