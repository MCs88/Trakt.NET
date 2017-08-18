﻿namespace TraktApiSharp.Tests.Objects.Post.Checkins.Responses.JsonReader
{
    using FluentAssertions;
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using TestUtils;
    using Traits;
    using TraktApiSharp.Objects.Post.Checkins.Responses.JsonReader;
    using Xunit;

    [Category("Objects.Post.Checkins.Responses.JsonReader")]
    public partial class TraktCheckinPostErrorResponseObjectJsonReader_Tests
    {
        [Fact]
        public async Task Test_TraktCheckinPostErrorResponseObjectJsonReader_ReadObject_From_Stream_Complete()
        {
            var jsonReader = new TraktCheckinPostErrorResponseObjectJsonReader();

            using (var stream = JSON_COMPLETE.ToStream())
            {
                var checkinErrorResponse = await jsonReader.ReadObjectAsync(stream);

                checkinErrorResponse.Should().NotBeNull();
                checkinErrorResponse.ExpiresAt.Should().Be(DateTime.Parse("2016-04-01T12:44:40Z").ToUniversalTime());
            }
        }

        [Fact]
        public async Task Test_TraktCheckinPostErrorResponseObjectJsonReader_ReadObject_From_Stream_Not_Valid()
        {
            var jsonReader = new TraktCheckinPostErrorResponseObjectJsonReader();

            using (var stream = JSON_NOT_VALID.ToStream())
            {
                var checkinErrorResponse = await jsonReader.ReadObjectAsync(stream);

                checkinErrorResponse.Should().NotBeNull();
                checkinErrorResponse.ExpiresAt.Should().BeNull();
            }
        }

        [Fact]
        public async Task Test_TraktCheckinPostErrorResponseObjectJsonReader_ReadObject_From_Stream_Null()
        {
            var jsonReader = new TraktCheckinPostErrorResponseObjectJsonReader();

            var checkinErrorResponse = await jsonReader.ReadObjectAsync(default(Stream));
            checkinErrorResponse.Should().BeNull();
        }

        [Fact]
        public async Task Test_TraktCheckinPostErrorResponseObjectJsonReader_ReadObject_From_Stream_Empty()
        {
            var jsonReader = new TraktCheckinPostErrorResponseObjectJsonReader();

            using (var stream = string.Empty.ToStream())
            {
                var checkinErrorResponse = await jsonReader.ReadObjectAsync(stream);
                checkinErrorResponse.Should().BeNull();
            }
        }
    }
}