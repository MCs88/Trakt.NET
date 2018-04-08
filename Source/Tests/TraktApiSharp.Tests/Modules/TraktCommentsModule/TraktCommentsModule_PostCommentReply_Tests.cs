﻿namespace TraktApiSharp.Tests.Modules.TraktCommentsModule
{
    using FluentAssertions;
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using TestUtils;
    using Traits;
    using TraktApiSharp.Exceptions;
    using TraktApiSharp.Objects.Post.Comments;
    using TraktApiSharp.Objects.Post.Comments.Implementations;
    using TraktApiSharp.Objects.Post.Comments.Responses;
    using TraktApiSharp.Responses;
    using Xunit;

    [Category("Modules.Comments")]
    public partial class TraktCommentsModule_Tests
    {
        [Fact]
        public async Task Test_TraktCommentsModule_PostCommentReply()
        {
            const uint commentId = 190U;
            const string comment = "one two three four five reply";

            var commentReplyPost = new TraktCommentReplyPost
            {
                Comment = comment
            };

            var postJson = await TestUtility.SerializeObject<ITraktCommentReplyPost>(commentReplyPost);
            postJson.Should().NotBeNullOrEmpty();

            TestUtility.SetupMockResponseWithOAuth($"comments/{commentId}/replies", postJson, COMMENT_POST_RESPONSE_JSON);

            var response = TestUtility.MOCK_TEST_CLIENT.Comments.PostCommentReplyAsync(commentId, comment).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull();

            var responseValue = response.Value;

            responseValue.Id.Should().Be(190U);
            responseValue.ParentId.Should().Be(0U);
            responseValue.CreatedAt.Should().Be(DateTime.Parse("2014-08-04T06:46:01.996Z").ToUniversalTime());
            responseValue.Comment.Should().Be("Oh, I wasn't really listening.");
            responseValue.Spoiler.Should().BeFalse();
            responseValue.Review.Should().BeFalse();
            responseValue.Replies.Should().Be(0);
            responseValue.Likes.Should().Be(0);
            responseValue.UserRating.Should().NotHaveValue();
            responseValue.User.Should().NotBeNull();
            responseValue.User.Username.Should().Be("sean");
            responseValue.User.IsPrivate.Should().BeFalse();
            responseValue.User.Name.Should().Be("Sean Rudford");
            responseValue.User.IsVIP.Should().BeTrue();
            responseValue.User.IsVIP_EP.Should().BeFalse();
            responseValue.Sharing.Should().NotBeNull();
            responseValue.Sharing.Facebook.Should().BeTrue();
            responseValue.Sharing.Twitter.Should().BeTrue();
            responseValue.Sharing.Tumblr.Should().BeFalse();
            responseValue.Sharing.Medium.Should().BeTrue();
        }

        [Fact]
        public async Task Test_TraktCommentsModule_PostCommentReplyWithSpoiler()
        {
            const uint commentId = 190U;
            const string comment = "one two three four five reply";
            const bool spoiler = false;

            var commentReplyPost = new TraktCommentReplyPost
            {
                Comment = comment,
                Spoiler = spoiler
            };

            var postJson = await TestUtility.SerializeObject<ITraktCommentReplyPost>(commentReplyPost);
            postJson.Should().NotBeNullOrEmpty();

            TestUtility.SetupMockResponseWithOAuth($"comments/{commentId}/replies", postJson, COMMENT_POST_RESPONSE_JSON);

            var response = TestUtility.MOCK_TEST_CLIENT.Comments.PostCommentReplyAsync(commentId, comment, spoiler).Result;

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.HasValue.Should().BeTrue();
            response.Value.Should().NotBeNull();

            var responseValue = response.Value;

            responseValue.Id.Should().Be(190U);
            responseValue.ParentId.Should().Be(0U);
            responseValue.CreatedAt.Should().Be(DateTime.Parse("2014-08-04T06:46:01.996Z").ToUniversalTime());
            responseValue.Comment.Should().Be("Oh, I wasn't really listening.");
            responseValue.Spoiler.Should().BeFalse();
            responseValue.Review.Should().BeFalse();
            responseValue.Replies.Should().Be(0);
            responseValue.Likes.Should().Be(0);
            responseValue.UserRating.Should().NotHaveValue();
            responseValue.User.Should().NotBeNull();
            responseValue.User.Username.Should().Be("sean");
            responseValue.User.IsPrivate.Should().BeFalse();
            responseValue.User.Name.Should().Be("Sean Rudford");
            responseValue.User.IsVIP.Should().BeTrue();
            responseValue.User.IsVIP_EP.Should().BeFalse();
            responseValue.Sharing.Should().NotBeNull();
            responseValue.Sharing.Facebook.Should().BeTrue();
            responseValue.Sharing.Twitter.Should().BeTrue();
            responseValue.Sharing.Tumblr.Should().BeFalse();
            responseValue.Sharing.Medium.Should().BeTrue();
        }

        [Fact]
        public void Test_TraktCommentsModule_PostCommentReplyExceptions()
        {
            const uint commentId = 190U;
            const string comment = "one two three four five reply";

            var commentReplyPost = new TraktCommentReplyPost
            {
                Comment = comment
            };

            var uri = $"comments/{commentId}/replies";

            TestUtility.SetupMockResponseWithoutOAuth(uri, HttpStatusCode.Unauthorized);

            Func<Task<TraktResponse<ITraktCommentPostResponse>>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Comments.PostCommentReplyAsync(commentId, comment);
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, HttpStatusCode.NotFound);
            act.Should().Throw<TraktCommentNotFoundException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, HttpStatusCode.BadRequest);
            act.Should().Throw<TraktBadRequestException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, HttpStatusCode.Forbidden);
            act.Should().Throw<TraktForbiddenException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, HttpStatusCode.MethodNotAllowed);
            act.Should().Throw<TraktMethodNotFoundException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, HttpStatusCode.Conflict);
            act.Should().Throw<TraktConflictException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, HttpStatusCode.InternalServerError);
            act.Should().Throw<TraktServerException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, HttpStatusCode.BadGateway);
            act.Should().Throw<TraktBadGatewayException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)412);
            act.Should().Throw<TraktPreconditionFailedException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)422);
            act.Should().Throw<TraktValidationException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)429);
            act.Should().Throw<TraktRateLimitException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)503);
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)504);
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)520);
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)521);
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockResponseWithOAuth(uri, (HttpStatusCode)522);
            act.Should().Throw<TraktServerUnavailableException>();
        }

        [Fact]
        public async Task Test_TraktCommentsModule_PostCommentReplyArgumentExceptions()
        {
            const uint commentId = 190U;
            string comment = "one two three four five reply";

            var commentReplyPost = new TraktCommentReplyPost
            {
                Comment = comment
            };

            var postJson = await TestUtility.SerializeObject<ITraktCommentReplyPost>(commentReplyPost);
            postJson.Should().NotBeNullOrEmpty();

            TestUtility.SetupMockResponseWithOAuth($"comments/{commentId}/replies", postJson, COMMENT_POST_RESPONSE_JSON);

            Func<Task<TraktResponse<ITraktCommentPostResponse>>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Comments.PostCommentReplyAsync(0, comment);

            act.Should().Throw<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.Comments.PostCommentReplyAsync(commentId, null);
            act.Should().Throw<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.Comments.PostCommentReplyAsync(commentId, string.Empty);
            act.Should().Throw<ArgumentException>();

            comment = "one two three four";

            act = async () => await TestUtility.MOCK_TEST_CLIENT.Comments.PostCommentReplyAsync(commentId, comment);
            act.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}
