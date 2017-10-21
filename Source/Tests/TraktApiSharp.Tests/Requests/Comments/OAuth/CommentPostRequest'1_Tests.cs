﻿namespace TraktApiSharp.Tests.Requests.Comments.OAuth
{
    using FluentAssertions;
    using System;
    using Traits;
    using TraktApiSharp.Objects.Post.Comments;
    using TraktApiSharp.Requests.Base;
    using TraktApiSharp.Requests.Comments.OAuth;
    using Xunit;

    [Category("Requests.Comments.OAuth")]
    public class CommentPostRequest_1_Tests
    {
        [Fact]
        public void Test_CommentPostRequest_1_Has_AuthorizationRequirement_Required()
        {
            var request = new CommentPostRequest<TraktCommentPost>();
            request.AuthorizationRequirement.Should().Be(AuthorizationRequirement.Required);
        }

        [Fact]
        public void Test_CommentPostRequest_1_Has_Valid_UriTemplate()
        {
            var request = new CommentPostRequest<TraktCommentPost>();
            request.UriTemplate.Should().Be("comments");
        }

        [Fact]
        public void Test_CommentPostRequest_1_Validate_Throws_ArgumentNullException()
        {
            var request = new CommentPostRequest<TraktCommentPost>();

            Action act = () => request.Validate();
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void Test_CommentPostRequest_1_Returns_Valid_UriPathParameters()
        {
            var request = new CommentPostRequest<TraktCommentPost>();
            var uriParams = request.GetUriPathParameters();
            uriParams.Should().NotBeNull().And.BeEmpty().And.HaveCount(0);
        }
    }
}
