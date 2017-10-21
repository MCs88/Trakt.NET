﻿namespace TraktApiSharp.Tests.Requests.Scrobbles.OAuth
{
    using FluentAssertions;
    using Traits;
    using TraktApiSharp.Requests.Base;
    using TraktApiSharp.Requests.Scrobbles.OAuth;
    using Xunit;

    [Category("Requests.Scrobbles.OAuth")]
    public class ScrobblePauseRequest_2_Tests
    {
        [Fact]
        public void Test_ScrobblePauseRequest_2_Has_AuthorizationRequirement_Required()
        {
            var request = new ScrobblePauseRequest<int, float>();
            request.AuthorizationRequirement.Should().Be(AuthorizationRequirement.Required);
        }

        [Fact]
        public void Test_ScrobblePauseRequest_2_Has_Valid_UriTemplate()
        {
            var request = new ScrobblePauseRequest<int, float>();
            request.UriTemplate.Should().Be("scrobble/pause");
        }

        [Fact]
        public void Test_ScrobblePauseRequest_2_Returns_Valid_UriPathParameters()
        {
            var request = new ScrobblePauseRequest<int, float>();
            request.GetUriPathParameters().Should().NotBeNull().And.HaveCount(0);
        }
    }
}
