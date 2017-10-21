﻿namespace TraktApiSharp.Tests.Requests.Syncs.OAuth
{
    using FluentAssertions;
    using System;
    using System.Collections.Generic;
    using Traits;
    using TraktApiSharp.Requests.Base;
    using TraktApiSharp.Requests.Syncs.OAuth;
    using Xunit;

    [Category("Requests.Syncs.OAuth")]
    public class SyncPlaybackDeleteRequest_Tests
    {
        [Fact]
        public void Test_SyncPlaybackDeleteRequest_Has_AuthorizationRequirement_Required()
        {
            var request = new SyncPlaybackDeleteRequest();
            request.AuthorizationRequirement.Should().Be(AuthorizationRequirement.Required);
        }

        [Fact]
        public void Test_SyncPlaybackDeleteRequest_Returns_Valid_RequestObjectType()
        {
            var requestMock = new SyncPlaybackDeleteRequest();
            requestMock.RequestObjectType.Should().Be(RequestObjectType.Unspecified);
        }

        [Fact]
        public void Test_SyncPlaybackDeleteRequest_Has_Valid_UriTemplate()
        {
            var request = new SyncPlaybackDeleteRequest();
            request.UriTemplate.Should().Be("sync/playback/{id}");
        }

        [Fact]
        public void Test_SyncPlaybackDeleteRequest_Returns_Valid_UriPathParameters()
        {
            var requestMock = new SyncPlaybackDeleteRequest { Id = "123" };

            requestMock.GetUriPathParameters().Should().NotBeNull()
                                                       .And.HaveCount(1)
                                                       .And.Contain(new Dictionary<string, object>
                                                       {
                                                           ["id"] = "123"
                                                       });
        }

        [Fact]
        public void Test_SyncPlaybackDeleteRequest_Validate_Throws_Exceptions()
        {
            // id is null
            var requestMock = new SyncPlaybackDeleteRequest();

            Action act = () => requestMock.Validate();
            act.ShouldThrow<ArgumentNullException>();

            // empty id
            requestMock = new SyncPlaybackDeleteRequest { Id = string.Empty };

            act = () => requestMock.Validate();
            act.ShouldThrow<ArgumentException>();

            // id with spaces
            requestMock = new SyncPlaybackDeleteRequest { Id = "invalid id" };

            act = () => requestMock.Validate();
            act.ShouldThrow<ArgumentException>();
        }
    }
}
