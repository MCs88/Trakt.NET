﻿namespace TraktApiSharp.Tests.Objects.Basic.Json.Writer
{
    using FluentAssertions;
    using System;
    using System.Threading.Tasks;
    using Traits;
    using TraktApiSharp.Objects.Basic;
    using TraktApiSharp.Objects.Basic.Implementations;
    using TraktApiSharp.Objects.Basic.Json.Writer;
    using Xunit;

    [Category("Objects.Basic.JsonWriter")]
    public partial class ImageObjectJsonWriter_Tests
    {
        [Fact]
        public void Test_ImageObjectJsonWriter_WriteObject_Object_Exceptions()
        {
            var traktJsonWriter = new ImageObjectJsonWriter();
            Func<Task<string>> action = () => traktJsonWriter.WriteObjectAsync(default(ITraktImage));
            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public async Task Test_ImageObjectJsonWriter_WriteObject_Object_Complete()
        {
            ITraktImage traktImage = new TraktImage
            {
                Full = "fullPath"
            };

            var traktJsonWriter = new ImageObjectJsonWriter();
            string json = await traktJsonWriter.WriteObjectAsync(traktImage);
            json.Should().Be(@"{""full"":""fullPath""}");
        }
    }
}
