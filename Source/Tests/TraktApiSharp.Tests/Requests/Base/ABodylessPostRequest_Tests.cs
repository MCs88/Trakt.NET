﻿namespace TraktApiSharp.Tests.Requests.Base
{
    using FluentAssertions;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using Traits;
    using TraktApiSharp.Requests.Base;
    using Xunit;

    [Category("Requests.Base")]
    public class ABodylessPostRequest_Tests
    {
        internal class BodylessPostRequestMock : ABodylessPostRequest
        {
            public override string UriTemplate { get { throw new NotImplementedException(); } }
            public override IDictionary<string, object> GetUriPathParameters() => throw new NotImplementedException();
            public override void Validate() => throw new NotImplementedException();
        }

        [Fact]
        public void Test_ABodylessPostRequest_Returns_Valid_AuthorizationRequirement()
        {
            var requestMock = new BodylessPostRequestMock();
            requestMock.AuthorizationRequirement.Should().Be(AuthorizationRequirement.Required);
        }

        [Fact]
        public void Test_ABodylessPostRequest_Returns_Valid_Method()
        {
            var requestMock = new BodylessPostRequestMock();
            requestMock.Method.Should().Be(HttpMethod.Post);
        }
    }
}
