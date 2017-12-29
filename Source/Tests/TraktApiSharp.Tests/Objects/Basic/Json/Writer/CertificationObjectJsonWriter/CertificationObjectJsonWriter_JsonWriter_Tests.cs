﻿namespace TraktApiSharp.Tests.Objects.Basic.Json.Writer
{
    using FluentAssertions;
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Traits;
    using TraktApiSharp.Objects.Basic;
    using TraktApiSharp.Objects.Basic.Implementations;
    using TraktApiSharp.Objects.Basic.Json.Writer;
    using Xunit;

    [Category("Objects.Basic.JsonWriter")]
    public partial class CertificationObjectJsonWriter_Tests
    {
        [Fact]
        public void Test_CertificationObjectJsonWriter_WriteObject_JsonWriter_Exceptions()
        {
            var traktJsonWriter = new CertificationObjectJsonWriter();
            ITraktCertification traktCertification = new TraktCertification();
            Func<Task> action = () => traktJsonWriter.WriteObjectAsync(default(JsonTextWriter), traktCertification);
            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public async Task Test_CertificationObjectJsonWriter_WriteObject_JsonWriter_Empty()
        {
            ITraktCertification traktCertification = new TraktCertification();

            using (var stringWriter = new StringWriter())
            using (var jsonWriter = new JsonTextWriter(stringWriter))
            {
                var traktJsonWriter = new CertificationObjectJsonWriter();
                await traktJsonWriter.WriteObjectAsync(jsonWriter, traktCertification);
                stringWriter.ToString().Should().Be("{}");
            }
        }

        [Fact]
        public async Task Test_CertificationObjectJsonWriter_WriteObject_JsonWriter_Only_Name_Property()
        {
            ITraktCertification traktCertification = new TraktCertification
            {
                Name = "certification name"
            };

            using (var stringWriter = new StringWriter())
            using (var jsonWriter = new JsonTextWriter(stringWriter))
            {
                var traktJsonWriter = new CertificationObjectJsonWriter();
                await traktJsonWriter.WriteObjectAsync(jsonWriter, traktCertification);
                stringWriter.ToString().Should().Be(@"{""name"":""certification name""}");
            }
        }

        [Fact]
        public async Task Test_CertificationObjectJsonWriter_WriteObject_JsonWriter_Only_Slug_Property()
        {
            ITraktCertification traktCertification = new TraktCertification
            {
                Slug = "certification slug"
            };

            using (var stringWriter = new StringWriter())
            using (var jsonWriter = new JsonTextWriter(stringWriter))
            {
                var traktJsonWriter = new CertificationObjectJsonWriter();
                await traktJsonWriter.WriteObjectAsync(jsonWriter, traktCertification);
                stringWriter.ToString().Should().Be(@"{""slug"":""certification slug""}");
            }
        }

        [Fact]
        public async Task Test_CertificationObjectJsonWriter_WriteObject_JsonWriter_Only_Description_Property()
        {
            ITraktCertification traktCertification = new TraktCertification
            {
                Description = "certification description"
            };

            using (var stringWriter = new StringWriter())
            using (var jsonWriter = new JsonTextWriter(stringWriter))
            {
                var traktJsonWriter = new CertificationObjectJsonWriter();
                await traktJsonWriter.WriteObjectAsync(jsonWriter, traktCertification);
                stringWriter.ToString().Should().Be(@"{""description"":""certification description""}");
            }
        }

        [Fact]
        public async Task Test_CertificationObjectJsonWriter_WriteObject_JsonWriter_Complete()
        {
            ITraktCertification traktCertification = new TraktCertification
            {
                Name = "certification name",
                Slug = "certification slug",
                Description = "certification description"
            };

            using (var stringWriter = new StringWriter())
            using (var jsonWriter = new JsonTextWriter(stringWriter))
            {
                var traktJsonWriter = new CertificationObjectJsonWriter();
                await traktJsonWriter.WriteObjectAsync(jsonWriter, traktCertification);
                stringWriter.ToString().Should().Be(@"{""name"":""certification name"",""slug"":""certification slug"",""description"":""certification description""}");
            }
        }
    }
}
