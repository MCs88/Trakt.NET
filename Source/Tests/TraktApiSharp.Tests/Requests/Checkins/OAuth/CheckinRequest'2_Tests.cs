﻿namespace TraktApiSharp.Tests.Requests.Checkins.OAuth
{
    using FluentAssertions;
    using Traits;
    using TraktApiSharp.Requests.Base;
    using TraktApiSharp.Requests.Checkins.OAuth;
    using Xunit;

    [Category("Requests.Checkins.OAuth")]
    public class CheckinRequest_2_Tests
    {
        [Fact]
        public void Test_CheckinRequest_2_Has_AuthorizationRequirement_Required()
        {
            var request = new CheckinRequest<int, float>();
            request.AuthorizationRequirement.Should().Be(AuthorizationRequirement.Required);
        }

        [Fact]
        public void Test_CheckinRequest_2_Has_Valid_UriTemplate()
        {
            var request = new CheckinRequest<int, float>();
            request.UriTemplate.Should().Be("checkin");
        }

        [Fact]
        public void Test_CheckinRequest_2_Returns_Valid_UriPathParameters()
        {
            var request = new CheckinRequest<int, float>();
            request.GetUriPathParameters().Should().NotBeNull().And.HaveCount(0);
        }
    }
}
