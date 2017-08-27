﻿namespace TraktApiSharp.Tests.Requests.Movies
{
    using FluentAssertions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Traits;
    using TraktApiSharp.Objects.Get.Movies;
    using TraktApiSharp.Requests.Movies;
    using Xunit;

    [Category("Requests.Movies")]
    public class MovieTranslationsRequest_Tests
    {
        [Fact]
        public void Test_MovieTranslationsRequest_IsNotAbstract()
        {
            typeof(MovieTranslationsRequest).IsAbstract.Should().BeFalse();
        }

        [Fact]
        public void Test_MovieTranslationsRequest_IsSealed()
        {
            typeof(MovieTranslationsRequest).IsSealed.Should().BeTrue();
        }

        [Fact]
        public void Test_MovieTranslationsRequest_Inherits_AMovieRequest_1()
        {
            typeof(MovieTranslationsRequest).IsSubclassOf(typeof(AMovieRequest<ITraktMovieTranslation>)).Should().BeTrue();
        }

        [Fact]
        public void Test_MovieTranslationsRequest_Has_Valid_UriTemplate()
        {
            var request = new MovieTranslationsRequest();
            request.UriTemplate.Should().Be("movies/{id}/translations{/language}");
        }

        [Fact]
        public void Test_MovieTranslationsRequest_Has_LanguageCode_Property()
        {
            var propertyInfo = typeof(MovieTranslationsRequest)
                    .GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.Name == "LanguageCode")
                    .FirstOrDefault();

            propertyInfo.CanRead.Should().BeTrue();
            propertyInfo.CanWrite.Should().BeTrue();
            propertyInfo.PropertyType.Should().Be(typeof(string));
        }

        [Fact]
        public void Test_MovieTranslationsRequest_Returns_Valid_UriPathParameters()
        {
            // without language code
            var request = new MovieTranslationsRequest { Id = "123" };

            request.GetUriPathParameters().Should().NotBeNull()
                                                   .And.HaveCount(1)
                                                   .And.Contain(new Dictionary<string, object>
                                                   {
                                                       ["id"] = "123"
                                                   });

            // with language code
            request = new MovieTranslationsRequest { Id = "123", LanguageCode = "en" };

            request.GetUriPathParameters().Should().NotBeNull()
                                                   .And.HaveCount(2)
                                                   .And.Contain(new Dictionary<string, object>
                                                   {
                                                       ["id"] = "123",
                                                       ["language"] = "en"
                                                   });
        }

        [Fact]
        public void Test_MovieTranslationsRequest_Validate_Throws_Exceptions()
        {
            // id is null
            var request = new MovieTranslationsRequest();

            Action act = () => request.Validate();
            act.ShouldThrow<ArgumentNullException>();

            // empty id
            request = new MovieTranslationsRequest { Id = string.Empty };

            act = () => request.Validate();
            act.ShouldThrow<ArgumentException>();

            // id with spaces
            request = new MovieTranslationsRequest { Id = "invalid id" };
            act.ShouldThrow<ArgumentException>();

            // language code with wrong length
            request = new MovieTranslationsRequest { Id = "123", LanguageCode = "eng" };

            act = () => request.Validate();
            act.ShouldThrow<ArgumentOutOfRangeException>();

            request = new MovieTranslationsRequest { Id = "123", LanguageCode = "e" };

            act = () => request.Validate();
            act.ShouldThrow<ArgumentOutOfRangeException>();
        }
    }
}
