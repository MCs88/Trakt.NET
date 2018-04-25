﻿namespace TraktApiSharp.Tests.Authentication
{
    using Core;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using TraktApiSharp.Enums;
    using TraktApiSharp.Exceptions;
    using TraktApiSharp.Objects.Authentication.Implementations;
    using TraktApiSharp.Objects.Basic.Implementations;
    using Utils;

    [TestClass]
    public class TraktOAuthTests
    {
        [TestMethod]
        public void TestTraktOAuthDefaultConstructor()
        {
            var client = new TraktClient();

            client.OAuth.Client.Should().Be(client);
        }

        // -----------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------

        [ClassInitialize]
        public static void InitializeTests(TestContext context)
        {
            TestUtility.SetupMockAuthenticationHttpClient();
        }

        [ClassCleanup]
        public static void CleanupTests()
        {
            TestUtility.ResetMockHttpClient();
        }

        [TestCleanup]
        public void CleanupSingleTest()
        {
            TestUtility.ClearMockHttpClient();
            TestUtility.SetDefaultClientValues();
        }

        // -----------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------

        #region CreateAuthorizationUrl

        [TestMethod]
        public void TestTraktOAuthCreateAuthorizationUrlDefault()
        {
            var clientId = TestUtility.MOCK_TEST_CLIENT.ClientId;
            var redirectUri = TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri;

            var encodedUrl = BuildEncodedAuthorizeUrl(false, clientId, redirectUri);

            var createdUrl = TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrl();
            createdUrl.Should().NotBeNullOrEmpty().And.Be(encodedUrl);
        }

        [TestMethod]
        public void TestTraktOAuthCreateAuthorizationStagingUrlDefault()
        {
            var clientId = TestUtility.MOCK_TEST_CLIENT.ClientId;
            var redirectUri = TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri;

            var encodedStagingUrl = BuildEncodedAuthorizeUrl(true, clientId, redirectUri);

            TestUtility.MOCK_TEST_CLIENT.Configuration.UseSandboxEnvironment = true;
            var createdUrl = TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrl();
            createdUrl.Should().NotBeNullOrEmpty().And.Be(encodedStagingUrl);

            TestUtility.MOCK_TEST_CLIENT.Configuration.UseSandboxEnvironment = false;
        }

        [TestMethod]
        public void TestTraktOAuthCreateAuthorizationUrlDefaultArgumentExceptions()
        {
            TestUtility.MOCK_TEST_CLIENT.ClientId = null;

            Action act = () => TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrl();
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.ClientId = string.Empty;

            act = () => TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrl();
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.ClientId = "client id";

            act = () => TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrl();
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.ClientId = "clientId";
            TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri = null;

            act = () => TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrl();
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri = string.Empty;

            act = () => TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrl();
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri = "redirect uri";

            act = () => TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrl();
            act.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void TestTraktOAuthCreateAuthorizationUrlWithClientId()
        {
            var clientId = "clientId";
            var redirectUri = TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri;

            var encodedUrl = BuildEncodedAuthorizeUrl(false, clientId, redirectUri);

            var createdUrl = TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrl(clientId);
            createdUrl.Should().NotBeNullOrEmpty().And.Be(encodedUrl);
        }

        [TestMethod]
        public void TestTraktOAuthCreateAuthorizationStagingUrlWithClientId()
        {
            var clientId = "clientId";
            var redirectUri = TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri;

            var encodedStagingUrl = BuildEncodedAuthorizeUrl(true, clientId, redirectUri);

            TestUtility.MOCK_TEST_CLIENT.Configuration.UseSandboxEnvironment = true;
            var createdUrl = TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrl(clientId);
            createdUrl.Should().NotBeNullOrEmpty().And.Be(encodedStagingUrl);

            TestUtility.MOCK_TEST_CLIENT.Configuration.UseSandboxEnvironment = false;
        }

        [TestMethod]
        public void TestTraktOAuthCreateAuthorizationUrlWithClientIdArgumentExceptions()
        {
            var clientId = "clientId";

            Action act = () => TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrl(null);
            act.Should().Throw<ArgumentException>();

            act = () => TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrl(string.Empty);
            act.Should().Throw<ArgumentException>();

            clientId = "client id";

            act = () => TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrl(clientId);
            act.Should().Throw<ArgumentException>();

            clientId = "clientId";
            TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri = null;

            act = () => TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrl(clientId);
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri = string.Empty;

            act = () => TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrl(clientId);
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri = "redirect uri";

            act = () => TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrl(clientId);
            act.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void TestTraktOAuthCreateAuthorizationUrlWithClientIdAndRedirectUri()
        {
            var clientId = "clientId";
            var redirectUri = "redirectUri";

            var encodedUrl = BuildEncodedAuthorizeUrl(false, clientId, redirectUri);

            var createdUrl = TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrl(clientId, redirectUri);
            createdUrl.Should().NotBeNullOrEmpty().And.Be(encodedUrl);
        }

        [TestMethod]
        public void TestTraktOAuthCreateAuthorizationStagingUrlWithClientIdAndRedirectUri()
        {
            var clientId = "clientId";
            var redirectUri = "redirectUri";

            var encodedStagingUrl = BuildEncodedAuthorizeUrl(true, clientId, redirectUri);

            TestUtility.MOCK_TEST_CLIENT.Configuration.UseSandboxEnvironment = true;
            var createdUrl = TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrl(clientId, redirectUri);
            createdUrl.Should().NotBeNullOrEmpty().And.Be(encodedStagingUrl);

            TestUtility.MOCK_TEST_CLIENT.Configuration.UseSandboxEnvironment = false;
        }

        [TestMethod]
        public void TestTraktOAuthCreateAuthorizationUrlWithClientIdAndRedirectUriArgumentExceptions()
        {
            var clientId = "clientId";
            var redirectUri = "redirectUri";

            Action act = () => TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrl(null, redirectUri);
            act.Should().Throw<ArgumentException>();

            act = () => TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrl(string.Empty, redirectUri);
            act.Should().Throw<ArgumentException>();

            clientId = "client id";

            act = () => TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrl(clientId, redirectUri);
            act.Should().Throw<ArgumentException>();

            clientId = "clientId";

            act = () => TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrl(clientId, null);
            act.Should().Throw<ArgumentException>();

            act = () => TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrl(clientId, string.Empty);
            act.Should().Throw<ArgumentException>();

            redirectUri = "redirect uri";

            act = () => TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrl(clientId, redirectUri);
            act.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void TestTraktOAuthCreateAuthorizationUrlWithClientIdAndRedirectUriAndState()
        {
            var clientId = "clientId";
            var redirectUri = "redirectUri";
            var state = "customState";

            var encodedUrl = BuildEncodedAuthorizeUrl(false, clientId, redirectUri, state);

            var createdUrl = TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrl(clientId, redirectUri, state);
            createdUrl.Should().NotBeNullOrEmpty().And.Be(encodedUrl);
        }

        [TestMethod]
        public void TestTraktOAuthCreateAuthorizationStagingUrlWithClientIdAndRedirectUriAndState()
        {
            var clientId = "clientId";
            var redirectUri = "redirectUri";
            var state = "customState";

            var encodedStagingUrl = BuildEncodedAuthorizeUrl(true, clientId, redirectUri, state);

            TestUtility.MOCK_TEST_CLIENT.Configuration.UseSandboxEnvironment = true;
            var createdUrl = TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrl(clientId, redirectUri, state);
            createdUrl.Should().NotBeNullOrEmpty().And.Be(encodedStagingUrl);

            TestUtility.MOCK_TEST_CLIENT.Configuration.UseSandboxEnvironment = false;
        }

        [TestMethod]
        public void TestTraktOAuthCreateAuthorizationUrlWithClientIdAndRedirectUriAndStateArgumentExceptions()
        {
            var clientId = "clientId";
            var redirectUri = "redirectUri";
            var state = "customState";

            Action act = () => TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrl(null, redirectUri, state);
            act.Should().Throw<ArgumentException>();

            act = () => TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrl(string.Empty, redirectUri, state);
            act.Should().Throw<ArgumentException>();

            clientId = "client id";

            act = () => TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrl(clientId, redirectUri, state);
            act.Should().Throw<ArgumentException>();

            clientId = "clientId";

            act = () => TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrl(clientId, null, state);
            act.Should().Throw<ArgumentException>();

            act = () => TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrl(clientId, string.Empty, state);
            act.Should().Throw<ArgumentException>();

            redirectUri = "redirect uri";

            act = () => TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrl(clientId, redirectUri, state);
            act.Should().Throw<ArgumentException>();

            redirectUri = "redirectUri";

            act = () => TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrl(clientId, redirectUri, null);
            act.Should().Throw<ArgumentException>();

            act = () => TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrl(clientId, redirectUri, string.Empty);
            act.Should().Throw<ArgumentException>();

            state = "custom state";

            act = () => TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrl(clientId, redirectUri, state);
            act.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void TestTraktOAuthCreateAuthorizationUrlWithDefaultStateDefault()
        {
            var clientId = TestUtility.MOCK_TEST_CLIENT.ClientId;
            var redirectUri = TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri;
            var state = TestUtility.MOCK_TEST_CLIENT.Authentication.AntiForgeryToken;

            var encodedUrl = BuildEncodedAuthorizeUrl(false, clientId, redirectUri, state);

            var createdUrl = TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrlWithDefaultState();
            createdUrl.Should().NotBeNullOrEmpty().And.Be(encodedUrl);
        }

        [TestMethod]
        public void TestTraktOAuthCreateAuthorizationStagingUrlWithDefaultStateDefault()
        {
            var clientId = TestUtility.MOCK_TEST_CLIENT.ClientId;
            var redirectUri = TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri;
            var state = TestUtility.MOCK_TEST_CLIENT.Authentication.AntiForgeryToken;

            var encodedStagingUrl = BuildEncodedAuthorizeUrl(true, clientId, redirectUri, state);

            TestUtility.MOCK_TEST_CLIENT.Configuration.UseSandboxEnvironment = true;
            var createdUrl = TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrlWithDefaultState();
            createdUrl.Should().NotBeNullOrEmpty().And.Be(encodedStagingUrl);

            TestUtility.MOCK_TEST_CLIENT.Configuration.UseSandboxEnvironment = false;
        }

        [TestMethod]
        public void TestTraktOAuthCreateAuthorizationUrlWithDefaultStateDefaultArgumentExceptions()
        {
            TestUtility.MOCK_TEST_CLIENT.ClientId = null;

            Action act = () => TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrlWithDefaultState();
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.ClientId = string.Empty;

            act = () => TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrlWithDefaultState();
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.ClientId = "client id";

            act = () => TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrlWithDefaultState();
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.ClientId = "clientId";
            TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri = null;

            act = () => TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrlWithDefaultState();
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri = string.Empty;

            act = () => TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrlWithDefaultState();
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri = "redirect uri";

            act = () => TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrlWithDefaultState();
            act.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void TestTraktOAuthCreateAuthorizationUrlWithDefaultStateAndClientId()
        {
            var clientId = "clientId";
            var redirectUri = TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri;
            var state = TestUtility.MOCK_TEST_CLIENT.Authentication.AntiForgeryToken;

            var encodedUrl = BuildEncodedAuthorizeUrl(false, clientId, redirectUri, state);

            var createdUrl = TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrlWithDefaultState(clientId);
            createdUrl.Should().NotBeNullOrEmpty().And.Be(encodedUrl);
        }

        [TestMethod]
        public void TestTraktOAuthCreateAuthorizationStagingUrlWithDefaultStateAndClientId()
        {
            var clientId = "clientId";
            var redirectUri = TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri;
            var state = TestUtility.MOCK_TEST_CLIENT.Authentication.AntiForgeryToken;

            var encodedStagingUrl = BuildEncodedAuthorizeUrl(true, clientId, redirectUri, state);

            TestUtility.MOCK_TEST_CLIENT.Configuration.UseSandboxEnvironment = true;
            var createdUrl = TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrlWithDefaultState(clientId);
            createdUrl.Should().NotBeNullOrEmpty().And.Be(encodedStagingUrl);

            TestUtility.MOCK_TEST_CLIENT.Configuration.UseSandboxEnvironment = false;
        }

        [TestMethod]
        public void TestTraktOAuthCreateAuthorizationUrlWithDefaultStateAndClientIdArgumentExceptions()
        {
            var clientId = "clientId";

            Action act = () => TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrlWithDefaultState(null);
            act.Should().Throw<ArgumentException>();

            act = () => TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrlWithDefaultState(string.Empty);
            act.Should().Throw<ArgumentException>();

            clientId = "client id";

            act = () => TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrlWithDefaultState(clientId);
            act.Should().Throw<ArgumentException>();

            clientId = "clientId";
            TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri = null;

            act = () => TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrlWithDefaultState(clientId);
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri = string.Empty;

            act = () => TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrlWithDefaultState(clientId);
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri = "redirect uri";

            act = () => TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrlWithDefaultState(clientId);
            act.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void TestTraktOAuthCreateAuthorizationUrlWithDefaultStateAndClientIdAndRedirectUri()
        {
            var clientId = "clientId";
            var redirectUri = "redirectUri";
            var state = TestUtility.MOCK_TEST_CLIENT.Authentication.AntiForgeryToken;

            var encodedUrl = BuildEncodedAuthorizeUrl(false, clientId, redirectUri, state);

            var createdUrl = TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrlWithDefaultState(clientId, redirectUri);
            createdUrl.Should().NotBeNullOrEmpty().And.Be(encodedUrl);
        }

        [TestMethod]
        public void TestTraktOAuthCreateAuthorizationStagingUrlWithDefaultStateAndClientIdAndRedirectUri()
        {
            var clientId = "clientId";
            var redirectUri = "redirectUri";
            var state = TestUtility.MOCK_TEST_CLIENT.Authentication.AntiForgeryToken;

            var encodedStagingUrl = BuildEncodedAuthorizeUrl(true, clientId, redirectUri, state);

            TestUtility.MOCK_TEST_CLIENT.Configuration.UseSandboxEnvironment = true;
            var createdUrl = TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrlWithDefaultState(clientId, redirectUri);
            createdUrl.Should().NotBeNullOrEmpty().And.Be(encodedStagingUrl);

            TestUtility.MOCK_TEST_CLIENT.Configuration.UseSandboxEnvironment = false;
        }

        [TestMethod]
        public void TestTraktOAuthCreateAuthorizationUrlWithDefaultStateAndClientIdAndRedirectUriArgumentExceptions()
        {
            var clientId = "clientId";
            var redirectUri = "redirectUri";

            Action act = () => TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrlWithDefaultState(null, redirectUri);
            act.Should().Throw<ArgumentException>();

            act = () => TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrlWithDefaultState(string.Empty, redirectUri);
            act.Should().Throw<ArgumentException>();

            clientId = "client id";

            act = () => TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrlWithDefaultState(clientId, redirectUri);
            act.Should().Throw<ArgumentException>();

            clientId = "clientId";

            act = () => TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrlWithDefaultState(clientId, null);
            act.Should().Throw<ArgumentException>();

            act = () => TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrlWithDefaultState(clientId, string.Empty);
            act.Should().Throw<ArgumentException>();

            redirectUri = "redirect uri";

            act = () => TestUtility.MOCK_TEST_CLIENT.OAuth.CreateAuthorizationUrlWithDefaultState(clientId, redirectUri);
            act.Should().Throw<ArgumentException>();
        }

        private string BuildEncodedAuthorizeUrl(bool staging, string clientId, string redirectUri, string state = null)
        {
            var baseUrl = staging ? Constants.OAuthBaseAuthorizeStagingUrl : Constants.OAuthBaseAuthorizeUrl;
            var oauthAuthorizeUri = Constants.OAuthAuthorizeUri;

            var uriParams = new Dictionary<string, string>();

            uriParams["response_type"] = "code";
            uriParams["client_id"] = clientId;
            uriParams["redirect_uri"] = redirectUri;

            if (!string.IsNullOrEmpty(state))
                uriParams["state"] = state;

            var encodedUriContent = new FormUrlEncodedContent(uriParams);
            var encodedUri = encodedUriContent.ReadAsStringAsync().Result;

            return $"{baseUrl}/{oauthAuthorizeUri}?{encodedUri}";
        }

        #endregion

        // -----------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------

        #region GetAuthorization

        [TestMethod]
        public void TestTraktOAuthGetAuthorization()
        {
            var mockAuthCode = "mockAuthCode";
            TestUtility.MOCK_TEST_CLIENT.Authentication.OAuthAuthorizationCode = mockAuthCode;

            var clientId = TestUtility.MOCK_TEST_CLIENT.ClientId;
            var clientSecret = TestUtility.MOCK_TEST_CLIENT.ClientSecret;
            var redirectUri = TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri;
            var grantType = TraktAccessTokenGrantType.AuthorizationCode.ObjectName;

            var accessToken = new TraktAuthorization
            {
                AccessToken = "mockAccessToken",
                TokenType = TraktAccessTokenType.Bearer,
                ExpiresInSeconds = 7200,
                RefreshToken = "mockRefreshToken",
                Scope = TraktAccessScope.Public
            };

            var accessTokenJson = JsonConvert.SerializeObject(accessToken);
            accessTokenJson.Should().NotBeNullOrEmpty();

            var postContent = $"{{ \"code\": \"{mockAuthCode}\", \"client_id\": \"{clientId}\", " +
                              $"\"client_secret\": \"{clientSecret}\", \"redirect_uri\": " +
                              $"\"{redirectUri}\", \"grant_type\": \"{grantType}\" }}";

            TestUtility.SetupMockAuthenticationResponse(Constants.OAuthTokenUri, postContent, accessTokenJson);

            var response = TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync().Result;

            response.Should().NotBeNull();
            response.AccessToken.Should().Be(accessToken.AccessToken);
            response.TokenType.Should().Be(accessToken.TokenType);
            response.ExpiresInSeconds.Should().Be(accessToken.ExpiresInSeconds);
            response.RefreshToken.Should().Be(accessToken.RefreshToken);
            response.Scope.Should().Be(accessToken.Scope);
            response.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, 1800 * 1000);
            response.IsExpired.Should().BeFalse();

            var clientAccessToken = TestUtility.MOCK_TEST_CLIENT.Authorization;

            clientAccessToken.Should().NotBeNull();
            clientAccessToken.AccessToken.Should().Be(response.AccessToken);
            clientAccessToken.TokenType.Should().Be(response.TokenType);
            clientAccessToken.ExpiresInSeconds.Should().Be(response.ExpiresInSeconds);
            clientAccessToken.RefreshToken.Should().Be(response.RefreshToken);
            clientAccessToken.Scope.Should().Be(response.Scope);
            clientAccessToken.CreatedAt.Should().Be(response.CreatedAt);
            clientAccessToken.IsExpired.Should().BeFalse();
        }

        [TestMethod]
        public void TestTraktOAuthGetAuthorizationExceptions()
        {
            var mockAuthCode = "mockAuthCode";
            TestUtility.MOCK_TEST_CLIENT.Authentication.OAuthAuthorizationCode = mockAuthCode;

            var clientId = TestUtility.MOCK_TEST_CLIENT.ClientId;
            var clientSecret = TestUtility.MOCK_TEST_CLIENT.ClientSecret;
            var redirectUri = TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri;
            var grantType = TraktAccessTokenGrantType.AuthorizationCode.ObjectName;

            var error = new TraktError
            {
                Error = "invalid_grant",
                Description = "The provided authorization grant is invalid, expired, revoked, does not match the redirection URI used in the authorization request, or was issued to another client."
            };

            var errorMessage = $"error on retrieving oauth access token\nerror: {error.Error}\n" +
                               $"description: {error.Description}";

            var errorJson = JsonConvert.SerializeObject(error);
            errorJson.Should().NotBeNullOrEmpty();

            var postContent = $"{{ \"code\": \"{mockAuthCode}\", \"client_id\": \"{clientId}\", " +
                              $"\"client_secret\": \"{clientSecret}\", \"redirect_uri\": " +
                              $"\"{redirectUri}\", \"grant_type\": \"{grantType}\" }}";

            var uri = Constants.OAuthTokenUri;

            TestUtility.SetupMockAuthenticationErrorResponse(uri, postContent, errorJson, HttpStatusCode.Unauthorized);

            Func<Task<TraktAuthorization>> act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync();
            act.Should().Throw<TraktAuthenticationOAuthException>().WithMessage(errorMessage);

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.NotFound);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync();
            act.Should().Throw<TraktNotFoundException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.BadRequest);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync();
            act.Should().Throw<TraktBadRequestException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.Conflict);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync();
            act.Should().Throw<TraktConflictException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.Forbidden);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync();
            act.Should().Throw<TraktForbiddenException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.MethodNotAllowed);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync();
            act.Should().Throw<TraktMethodNotFoundException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.InternalServerError);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync();
            act.Should().Throw<TraktServerException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.BadGateway);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync();
            act.Should().Throw<TraktBadGatewayException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)412);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync();
            act.Should().Throw<TraktPreconditionFailedException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)422);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync();
            act.Should().Throw<TraktValidationException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)429);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync();
            act.Should().Throw<TraktRateLimitException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)503);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync();
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)504);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync();
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)520);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync();
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)521);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync();
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)522);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync();
            act.Should().Throw<TraktServerUnavailableException>();
        }

        [TestMethod]
        public void TestTraktOAuthGetAuthorizationArgumentExceptions()
        {
            var mockAuthCode = "mockAuthCode";

            TestUtility.MOCK_TEST_CLIENT.Authentication.OAuthAuthorizationCode = null;

            Func<Task<TraktAuthorization>> act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync();
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.Authentication.OAuthAuthorizationCode = string.Empty;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync();
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.Authentication.OAuthAuthorizationCode = "mock auth code";

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync();
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.Authentication.OAuthAuthorizationCode = mockAuthCode;
            TestUtility.MOCK_TEST_CLIENT.ClientId = null;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync();
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.ClientId = string.Empty;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync();
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.ClientId = "client id";

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync();
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.ClientId = "clientId";
            TestUtility.MOCK_TEST_CLIENT.ClientSecret = null;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync();
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.ClientSecret = string.Empty;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync();
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.ClientSecret = "client secret";

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync();
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.ClientSecret = "clientSecret";
            TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri = null;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync();
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri = string.Empty;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync();
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri = "redirect uri";

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync();
            act.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void TestTraktOAuthGetAuthorizationWithCode()
        {
            var mockAuthCode = "mockAuthCode";

            var clientId = TestUtility.MOCK_TEST_CLIENT.ClientId;
            var clientSecret = TestUtility.MOCK_TEST_CLIENT.ClientSecret;
            var redirectUri = TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri;
            var grantType = TraktAccessTokenGrantType.AuthorizationCode.ObjectName;

            var accessToken = new TraktAuthorization
            {
                AccessToken = "mockAccessToken",
                TokenType = TraktAccessTokenType.Bearer,
                ExpiresInSeconds = 7200,
                RefreshToken = "mockRefreshToken",
                Scope = TraktAccessScope.Public
            };

            var accessTokenJson = JsonConvert.SerializeObject(accessToken);
            accessTokenJson.Should().NotBeNullOrEmpty();

            var postContent = $"{{ \"code\": \"{mockAuthCode}\", \"client_id\": \"{clientId}\", " +
                              $"\"client_secret\": \"{clientSecret}\", \"redirect_uri\": " +
                              $"\"{redirectUri}\", \"grant_type\": \"{grantType}\" }}";

            TestUtility.SetupMockAuthenticationResponse(Constants.OAuthTokenUri, postContent, accessTokenJson);

            var response = TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode).Result;

            response.Should().NotBeNull();
            response.AccessToken.Should().Be(accessToken.AccessToken);
            response.TokenType.Should().Be(accessToken.TokenType);
            response.RefreshToken.Should().Be(accessToken.RefreshToken);
            response.Scope.Should().Be(accessToken.Scope);
            response.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, 1800 * 1000);
            response.IsExpired.Should().BeFalse();

            var clientAccessToken = TestUtility.MOCK_TEST_CLIENT.Authorization;

            clientAccessToken.Should().NotBeNull();
            clientAccessToken.AccessToken.Should().Be(response.AccessToken);
            clientAccessToken.TokenType.Should().Be(response.TokenType);
            clientAccessToken.RefreshToken.Should().Be(response.RefreshToken);
            clientAccessToken.Scope.Should().Be(response.Scope);
            clientAccessToken.CreatedAt.Should().Be(response.CreatedAt);
            clientAccessToken.IsExpired.Should().BeFalse();
        }

        [TestMethod]
        public void TestTraktOAuthGetAuthorizationWithCodeExceptions()
        {
            var mockAuthCode = "mockAuthCode";

            var clientId = TestUtility.MOCK_TEST_CLIENT.ClientId;
            var clientSecret = TestUtility.MOCK_TEST_CLIENT.ClientSecret;
            var redirectUri = TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri;
            var grantType = TraktAccessTokenGrantType.AuthorizationCode.ObjectName;

            var error = new TraktError
            {
                Error = "invalid_grant",
                Description = "The provided authorization grant is invalid, expired, revoked, does not match the redirection URI used in the authorization request, or was issued to another client."
            };

            var errorMessage = $"error on retrieving oauth access token\nerror: {error.Error}\n" +
                               $"description: {error.Description}";

            var errorJson = JsonConvert.SerializeObject(error);
            errorJson.Should().NotBeNullOrEmpty();

            var postContent = $"{{ \"code\": \"{mockAuthCode}\", \"client_id\": \"{clientId}\", " +
                              $"\"client_secret\": \"{clientSecret}\", \"redirect_uri\": " +
                              $"\"{redirectUri}\", \"grant_type\": \"{grantType}\" }}";

            var uri = Constants.OAuthTokenUri;

            TestUtility.SetupMockAuthenticationErrorResponse(uri, postContent, errorJson, HttpStatusCode.Unauthorized);

            Func<Task<TraktAuthorization>> act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode);
            act.Should().Throw<TraktAuthenticationOAuthException>().WithMessage(errorMessage);

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.NotFound);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode);
            act.Should().Throw<TraktNotFoundException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.BadRequest);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode);
            act.Should().Throw<TraktBadRequestException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.Conflict);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode);
            act.Should().Throw<TraktConflictException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.Forbidden);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode);
            act.Should().Throw<TraktForbiddenException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.MethodNotAllowed);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode);
            act.Should().Throw<TraktMethodNotFoundException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.InternalServerError);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode);
            act.Should().Throw<TraktServerException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.BadGateway);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode);
            act.Should().Throw<TraktBadGatewayException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)412);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode);
            act.Should().Throw<TraktPreconditionFailedException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)422);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode);
            act.Should().Throw<TraktValidationException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)429);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode);
            act.Should().Throw<TraktRateLimitException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)503);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode);
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)504);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode);
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)520);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode);
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)521);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode);
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)522);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode);
            act.Should().Throw<TraktServerUnavailableException>();
        }

        [TestMethod]
        public void TestTraktOAuthGetAuthorizationWithCodeArgumentExceptions()
        {
            var mockAuthCode = "mockAuthCode";

            Func<Task<TraktAuthorization>> act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(null);
            act.Should().Throw<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(string.Empty);
            act.Should().Throw<ArgumentException>();

            mockAuthCode = "mock auth code";

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode);
            act.Should().Throw<ArgumentException>();

            mockAuthCode = "mockAuthCode";
            TestUtility.MOCK_TEST_CLIENT.ClientId = null;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode);
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.ClientId = string.Empty;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode);
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.ClientId = "client id";

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode);
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.ClientId = "clientId";
            TestUtility.MOCK_TEST_CLIENT.ClientSecret = null;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode);
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.ClientSecret = string.Empty;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode);
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.ClientSecret = "client secret";

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode);
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.ClientSecret = "clientSecret";
            TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri = null;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode);
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri = string.Empty;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode);
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri = "redirect uri";

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode);
            act.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void TestTraktOAuthGetAuthorizationWithCodeAndClientId()
        {
            var mockAuthCode = "mockAuthCode";

            var clientId = "clientId";
            var clientSecret = TestUtility.MOCK_TEST_CLIENT.ClientSecret;
            var redirectUri = TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri;
            var grantType = TraktAccessTokenGrantType.AuthorizationCode.ObjectName;

            var accessToken = new TraktAuthorization
            {
                AccessToken = "mockAccessToken",
                TokenType = TraktAccessTokenType.Bearer,
                ExpiresInSeconds = 7200,
                RefreshToken = "mockRefreshToken",
                Scope = TraktAccessScope.Public
            };

            var accessTokenJson = JsonConvert.SerializeObject(accessToken);
            accessTokenJson.Should().NotBeNullOrEmpty();

            var postContent = $"{{ \"code\": \"{mockAuthCode}\", \"client_id\": \"{clientId}\", " +
                              $"\"client_secret\": \"{clientSecret}\", \"redirect_uri\": " +
                              $"\"{redirectUri}\", \"grant_type\": \"{grantType}\" }}";

            TestUtility.SetupMockAuthenticationResponse(Constants.OAuthTokenUri, postContent, accessTokenJson);

            var response = TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId).Result;

            response.Should().NotBeNull();
            response.AccessToken.Should().Be(accessToken.AccessToken);
            response.TokenType.Should().Be(accessToken.TokenType);
            response.ExpiresInSeconds.Should().Be(accessToken.ExpiresInSeconds);
            response.RefreshToken.Should().Be(accessToken.RefreshToken);
            response.Scope.Should().Be(accessToken.Scope);
            response.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, 1800 * 1000);
            response.IsExpired.Should().BeFalse();

            var clientAccessToken = TestUtility.MOCK_TEST_CLIENT.Authorization;

            clientAccessToken.Should().NotBeNull();
            clientAccessToken.AccessToken.Should().Be(response.AccessToken);
            clientAccessToken.TokenType.Should().Be(response.TokenType);
            clientAccessToken.ExpiresInSeconds.Should().Be(response.ExpiresInSeconds);
            clientAccessToken.RefreshToken.Should().Be(response.RefreshToken);
            clientAccessToken.Scope.Should().Be(response.Scope);
            clientAccessToken.CreatedAt.Should().Be(response.CreatedAt);
            clientAccessToken.IsExpired.Should().BeFalse();
        }

        [TestMethod]
        public void TestTraktOAuthGetAuthorizationWithCodeAndClientIdExceptions()
        {
            var mockAuthCode = "mockAuthCode";

            var clientId = "clientId";
            var clientSecret = TestUtility.MOCK_TEST_CLIENT.ClientSecret;
            var redirectUri = TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri;
            var grantType = TraktAccessTokenGrantType.AuthorizationCode.ObjectName;

            var error = new TraktError
            {
                Error = "invalid_grant",
                Description = "The provided authorization grant is invalid, expired, revoked, does not match the redirection URI used in the authorization request, or was issued to another client."
            };

            var errorMessage = $"error on retrieving oauth access token\nerror: {error.Error}\n" +
                               $"description: {error.Description}";

            var errorJson = JsonConvert.SerializeObject(error);
            errorJson.Should().NotBeNullOrEmpty();

            var postContent = $"{{ \"code\": \"{mockAuthCode}\", \"client_id\": \"{clientId}\", " +
                              $"\"client_secret\": \"{clientSecret}\", \"redirect_uri\": " +
                              $"\"{redirectUri}\", \"grant_type\": \"{grantType}\" }}";

            var uri = Constants.OAuthTokenUri;

            TestUtility.SetupMockAuthenticationErrorResponse(uri, postContent, errorJson, HttpStatusCode.Unauthorized);

            Func<Task<TraktAuthorization>> act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId);
            act.Should().Throw<TraktAuthenticationOAuthException>().WithMessage(errorMessage);

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.NotFound);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId);
            act.Should().Throw<TraktNotFoundException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.BadRequest);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId);
            act.Should().Throw<TraktBadRequestException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.Conflict);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId);
            act.Should().Throw<TraktConflictException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.Forbidden);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId);
            act.Should().Throw<TraktForbiddenException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.MethodNotAllowed);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId);
            act.Should().Throw<TraktMethodNotFoundException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.InternalServerError);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId);
            act.Should().Throw<TraktServerException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.BadGateway);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId);
            act.Should().Throw<TraktBadGatewayException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)412);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId);
            act.Should().Throw<TraktPreconditionFailedException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)422);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId);
            act.Should().Throw<TraktValidationException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)429);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId);
            act.Should().Throw<TraktRateLimitException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)503);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId);
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)504);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId);
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)520);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId);
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)521);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId);
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)522);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId);
            act.Should().Throw<TraktServerUnavailableException>();
        }

        [TestMethod]
        public void TestTraktOAuthGetAuthorizationWithCodeAndClientIdArgumentExceptions()
        {
            var mockAuthCode = "mockAuthCode";
            var clientId = "clientId";

            Func<Task<TraktAuthorization>> act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(null, clientId);
            act.Should().Throw<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(string.Empty, clientId);
            act.Should().Throw<ArgumentException>();

            mockAuthCode = "mock auth code";

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId);
            act.Should().Throw<ArgumentException>();

            mockAuthCode = "mockAuthCode";

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, null);
            act.Should().Throw<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, string.Empty);
            act.Should().Throw<ArgumentException>();

            clientId = "client id";

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId);
            act.Should().Throw<ArgumentException>();

            clientId = "clientId";
            TestUtility.MOCK_TEST_CLIENT.ClientSecret = null;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId);
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.ClientSecret = string.Empty;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId);
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.ClientSecret = "client secret";

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId);
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.ClientSecret = "clientSecret";
            TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri = null;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId);
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri = string.Empty;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId);
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri = "redirect uri";

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId);
            act.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void TestTraktOAuthGetAuthorizationWithCodeAndClientIdAndClientSecret()
        {
            var mockAuthCode = "mockAuthCode";

            var clientId = "clientId";
            var clientSecret = "clientSecret";
            var redirectUri = TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri;
            var grantType = TraktAccessTokenGrantType.AuthorizationCode.ObjectName;

            var accessToken = new TraktAuthorization
            {
                AccessToken = "mockAccessToken",
                TokenType = TraktAccessTokenType.Bearer,
                ExpiresInSeconds = 7200,
                RefreshToken = "mockRefreshToken",
                Scope = TraktAccessScope.Public
            };

            var accessTokenJson = JsonConvert.SerializeObject(accessToken);
            accessTokenJson.Should().NotBeNullOrEmpty();

            var postContent = $"{{ \"code\": \"{mockAuthCode}\", \"client_id\": \"{clientId}\", " +
                              $"\"client_secret\": \"{clientSecret}\", \"redirect_uri\": " +
                              $"\"{redirectUri}\", \"grant_type\": \"{grantType}\" }}";

            TestUtility.SetupMockAuthenticationResponse(Constants.OAuthTokenUri, postContent, accessTokenJson);

            var response = TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret).Result;

            response.Should().NotBeNull();
            response.AccessToken.Should().Be(accessToken.AccessToken);
            response.TokenType.Should().Be(accessToken.TokenType);
            response.RefreshToken.Should().Be(accessToken.RefreshToken);
            response.Scope.Should().Be(accessToken.Scope);
            response.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, 1800 * 1000);
            response.IsExpired.Should().BeFalse();

            var clientAccessToken = TestUtility.MOCK_TEST_CLIENT.Authorization;

            clientAccessToken.Should().NotBeNull();
            clientAccessToken.AccessToken.Should().Be(response.AccessToken);
            clientAccessToken.TokenType.Should().Be(response.TokenType);
            clientAccessToken.RefreshToken.Should().Be(response.RefreshToken);
            clientAccessToken.Scope.Should().Be(response.Scope);
            clientAccessToken.CreatedAt.Should().Be(response.CreatedAt);
            clientAccessToken.IsExpired.Should().BeFalse();
        }

        [TestMethod]
        public void TestTraktOAuthGetAuthorizationWithCodeAndClientIdAndClientSecretExceptions()
        {
            var mockAuthCode = "mockAuthCode";

            var clientId = "clientId";
            var clientSecret = "clientSecret";
            var redirectUri = TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri;
            var grantType = TraktAccessTokenGrantType.AuthorizationCode.ObjectName;

            var error = new TraktError
            {
                Error = "invalid_grant",
                Description = "The provided authorization grant is invalid, expired, revoked, does not match the redirection URI used in the authorization request, or was issued to another client."
            };

            var errorMessage = $"error on retrieving oauth access token\nerror: {error.Error}\n" +
                               $"description: {error.Description}";

            var errorJson = JsonConvert.SerializeObject(error);
            errorJson.Should().NotBeNullOrEmpty();

            var postContent = $"{{ \"code\": \"{mockAuthCode}\", \"client_id\": \"{clientId}\", " +
                              $"\"client_secret\": \"{clientSecret}\", \"redirect_uri\": " +
                              $"\"{redirectUri}\", \"grant_type\": \"{grantType}\" }}";

            var uri = Constants.OAuthTokenUri;

            TestUtility.SetupMockAuthenticationErrorResponse(uri, postContent, errorJson, HttpStatusCode.Unauthorized);

            Func<Task<TraktAuthorization>> act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret);
            act.Should().Throw<TraktAuthenticationOAuthException>().WithMessage(errorMessage);

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.NotFound);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret);
            act.Should().Throw<TraktNotFoundException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.BadRequest);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret);
            act.Should().Throw<TraktBadRequestException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.Conflict);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret);
            act.Should().Throw<TraktConflictException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.Forbidden);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret);
            act.Should().Throw<TraktForbiddenException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.MethodNotAllowed);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret);
            act.Should().Throw<TraktMethodNotFoundException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.InternalServerError);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret);
            act.Should().Throw<TraktServerException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.BadGateway);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret);
            act.Should().Throw<TraktBadGatewayException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)412);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret);
            act.Should().Throw<TraktPreconditionFailedException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)422);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret);
            act.Should().Throw<TraktValidationException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)429);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret);
            act.Should().Throw<TraktRateLimitException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)503);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret);
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)504);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret);
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)520);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret);
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)521);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret);
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)522);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret);
            act.Should().Throw<TraktServerUnavailableException>();
        }

        [TestMethod]
        public void TestTraktOAuthGetAuthorizationWithCodeAndClientIdAndClientSecretArgumentExceptions()
        {
            var mockAuthCode = "mockAuthCode";
            var clientId = "clientId";
            var clientSecret = "clientSecret";

            Func<Task<TraktAuthorization>> act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(null, clientId, clientSecret);
            act.Should().Throw<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(string.Empty, clientId, clientSecret);
            act.Should().Throw<ArgumentException>();

            mockAuthCode = "mock auth code";

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret);
            act.Should().Throw<ArgumentException>();

            mockAuthCode = "mockAuthCode";

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, null, clientSecret);
            act.Should().Throw<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, string.Empty, clientSecret);
            act.Should().Throw<ArgumentException>();

            clientId = "client id";

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret);
            act.Should().Throw<ArgumentException>();

            clientId = "clientId";

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, null);
            act.Should().Throw<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, string.Empty);
            act.Should().Throw<ArgumentException>();

            clientSecret = "client secret";

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret);
            act.Should().Throw<ArgumentException>();

            clientSecret = "clientSecret";
            TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri = null;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret);
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri = string.Empty;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret);
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri = "redirect uri";

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret);
            act.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void TestTraktOAuthGetAuthorizationWithCodeAndClientIdAndClientSecretAndRedirectUri()
        {
            var mockAuthCode = "mockAuthCode";

            var clientId = "clientId";
            var clientSecret = "clientSecret";
            var redirectUri = "redirectUri";
            var grantType = TraktAccessTokenGrantType.AuthorizationCode.ObjectName;

            var accessToken = new TraktAuthorization
            {
                AccessToken = "mockAccessToken",
                TokenType = TraktAccessTokenType.Bearer,
                ExpiresInSeconds = 7200,
                RefreshToken = "mockRefreshToken",
                Scope = TraktAccessScope.Public
            };

            var accessTokenJson = JsonConvert.SerializeObject(accessToken);
            accessTokenJson.Should().NotBeNullOrEmpty();

            var postContent = $"{{ \"code\": \"{mockAuthCode}\", \"client_id\": \"{clientId}\", " +
                              $"\"client_secret\": \"{clientSecret}\", \"redirect_uri\": " +
                              $"\"{redirectUri}\", \"grant_type\": \"{grantType}\" }}";

            TestUtility.SetupMockAuthenticationResponse(Constants.OAuthTokenUri, postContent, accessTokenJson);

            var response = TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret, redirectUri).Result;

            response.Should().NotBeNull();
            response.AccessToken.Should().Be(accessToken.AccessToken);
            response.TokenType.Should().Be(accessToken.TokenType);
            response.ExpiresInSeconds.Should().Be(accessToken.ExpiresInSeconds);
            response.RefreshToken.Should().Be(accessToken.RefreshToken);
            response.Scope.Should().Be(accessToken.Scope);
            response.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, 1800 * 1000);
            response.IsExpired.Should().BeFalse();

            var clientAccessToken = TestUtility.MOCK_TEST_CLIENT.Authorization;

            clientAccessToken.Should().NotBeNull();
            clientAccessToken.AccessToken.Should().Be(response.AccessToken);
            clientAccessToken.TokenType.Should().Be(response.TokenType);
            clientAccessToken.ExpiresInSeconds.Should().Be(response.ExpiresInSeconds);
            clientAccessToken.RefreshToken.Should().Be(response.RefreshToken);
            clientAccessToken.Scope.Should().Be(response.Scope);
            clientAccessToken.CreatedAt.Should().Be(response.CreatedAt);
            clientAccessToken.IsExpired.Should().BeFalse();
        }

        [TestMethod]
        public void TestTraktOAuthGetAuthorizationWithCodeAndClientIdAndClientSecretAndRedirectUriExceptions()
        {
            var mockAuthCode = "mockAuthCode";

            var clientId = "clientId";
            var clientSecret = "clientSecret";
            var redirectUri = "redirectUri";
            var grantType = TraktAccessTokenGrantType.AuthorizationCode.ObjectName;

            var error = new TraktError
            {
                Error = "invalid_grant",
                Description = "The provided authorization grant is invalid, expired, revoked, does not match the redirection URI used in the authorization request, or was issued to another client."
            };

            var errorMessage = $"error on retrieving oauth access token\nerror: {error.Error}\n" +
                               $"description: {error.Description}";

            var errorJson = JsonConvert.SerializeObject(error);
            errorJson.Should().NotBeNullOrEmpty();

            var postContent = $"{{ \"code\": \"{mockAuthCode}\", \"client_id\": \"{clientId}\", " +
                              $"\"client_secret\": \"{clientSecret}\", \"redirect_uri\": " +
                              $"\"{redirectUri}\", \"grant_type\": \"{grantType}\" }}";

            var uri = Constants.OAuthTokenUri;

            TestUtility.SetupMockAuthenticationErrorResponse(uri, postContent, errorJson, HttpStatusCode.Unauthorized);

            Func<Task<TraktAuthorization>> act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktAuthenticationOAuthException>().WithMessage(errorMessage);

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.NotFound);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktNotFoundException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.BadRequest);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktBadRequestException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.Conflict);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktConflictException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.Forbidden);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktForbiddenException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.MethodNotAllowed);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktMethodNotFoundException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.InternalServerError);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktServerException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.BadGateway);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktBadGatewayException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)412);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktPreconditionFailedException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)422);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktValidationException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)429);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktRateLimitException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)503);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)504);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)520);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)521);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)522);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktServerUnavailableException>();
        }

        [TestMethod]
        public void TestTraktOAuthGetAuthorizationWithCodeAndClientIdAndClientSecretAndRedirectUriArgumentExceptions()
        {
            var mockAuthCode = "mockAuthCode";
            var clientId = "clientId";
            var clientSecret = "clientSecret";
            var redirectUri = "redirectUri";

            Func<Task<TraktAuthorization>> act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(null, clientId, clientSecret, redirectUri);
            act.Should().Throw<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(string.Empty, clientId, clientSecret, redirectUri);
            act.Should().Throw<ArgumentException>();

            mockAuthCode = "mock auth code";

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret, redirectUri);
            act.Should().Throw<ArgumentException>();

            mockAuthCode = "mockAuthCode";

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, null, clientSecret, redirectUri);
            act.Should().Throw<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, string.Empty, clientSecret, redirectUri);
            act.Should().Throw<ArgumentException>();

            clientId = "client id";

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret, redirectUri);
            act.Should().Throw<ArgumentException>();

            clientId = "clientId";

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, null, redirectUri);
            act.Should().Throw<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, string.Empty, redirectUri);
            act.Should().Throw<ArgumentException>();

            clientSecret = "client secret";

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret, redirectUri);
            act.Should().Throw<ArgumentException>();

            clientSecret = "clientSecret";

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret, null);
            act.Should().Throw<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret, string.Empty);
            act.Should().Throw<ArgumentException>();

            redirectUri = "redirect uri";

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.GetAuthorizationAsync(mockAuthCode, clientId, clientSecret, redirectUri);
            act.Should().Throw<ArgumentException>();
        }

        #endregion

        // -----------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------

        #region RefreshAuthorization

        [TestMethod]
        public void TestTraktOAuthRefreshAuthorization()
        {
            var clientId = TestUtility.MOCK_TEST_CLIENT.ClientId;
            var clientSecret = TestUtility.MOCK_TEST_CLIENT.ClientSecret;
            var redirectUri = TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri;
            var grantType = TraktAccessTokenGrantType.RefreshToken.ObjectName;

            var accessToken = new TraktAuthorization
            {
                AccessToken = "mockAccessToken",
                TokenType = TraktAccessTokenType.Bearer,
                ExpiresInSeconds = 7200,
                RefreshToken = "mockRefreshToken",
                Scope = TraktAccessScope.Public
            };

            var accessTokenJson = JsonConvert.SerializeObject(accessToken);
            accessTokenJson.Should().NotBeNullOrEmpty();

            var postContent = $"{{ \"refresh_token\": \"{accessToken.RefreshToken}\", \"client_id\": \"{clientId}\", " +
                              $"\"client_secret\": \"{clientSecret}\", \"redirect_uri\": " +
                              $"\"{redirectUri}\", \"grant_type\": \"{grantType}\" }}";

            TestUtility.SetupMockAuthenticationResponse(Constants.OAuthTokenUri, postContent, accessTokenJson);
            TestUtility.MOCK_TEST_CLIENT.Authorization = accessToken;

            var response = TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync().Result;

            response.Should().NotBeNull();
            response.AccessToken.Should().Be(accessToken.AccessToken);
            response.TokenType.Should().Be(accessToken.TokenType);
            response.ExpiresInSeconds.Should().Be(accessToken.ExpiresInSeconds);
            response.RefreshToken.Should().Be(accessToken.RefreshToken);
            response.Scope.Should().Be(accessToken.Scope);
            response.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, 1800 * 1000);
            response.IsExpired.Should().BeFalse();

            var clientAccessToken = TestUtility.MOCK_TEST_CLIENT.Authorization;

            clientAccessToken.Should().NotBeNull();
            clientAccessToken.AccessToken.Should().Be(response.AccessToken);
            clientAccessToken.TokenType.Should().Be(response.TokenType);
            clientAccessToken.ExpiresInSeconds.Should().Be(response.ExpiresInSeconds);
            clientAccessToken.RefreshToken.Should().Be(response.RefreshToken);
            clientAccessToken.Scope.Should().Be(response.Scope);
            clientAccessToken.CreatedAt.Should().Be(response.CreatedAt);
            clientAccessToken.IsExpired.Should().BeFalse();
        }

        [TestMethod]
        public void TestTraktOAuthRefreshAuthorizationExceptions()
        {
            var clientId = TestUtility.MOCK_TEST_CLIENT.ClientId;
            var clientSecret = TestUtility.MOCK_TEST_CLIENT.ClientSecret;
            var redirectUri = TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri;
            var grantType = TraktAccessTokenGrantType.RefreshToken.ObjectName;

            var accessToken = new TraktAuthorization
            {
                AccessToken = "mockAccessToken",
                TokenType = TraktAccessTokenType.Bearer,
                ExpiresInSeconds = 7200,
                RefreshToken = "mockRefreshToken",
                Scope = TraktAccessScope.Public
            };

            var error = new TraktError
            {
                Error = "invalid_grant",
                Description = "The provided authorization grant is invalid, expired, revoked, does not match the redirection URI used in the authorization request, or was issued to another client."
            };

            var errorMessage = $"error on refreshing oauth access token\nerror: {error.Error}\n" +
                               $"description: {error.Description}";

            var errorJson = JsonConvert.SerializeObject(error);
            errorJson.Should().NotBeNullOrEmpty();

            var postContent = $"{{ \"refresh_token\": \"{accessToken.RefreshToken}\", \"client_id\": \"{clientId}\", " +
                              $"\"client_secret\": \"{clientSecret}\", \"redirect_uri\": " +
                              $"\"{redirectUri}\", \"grant_type\": \"{grantType}\" }}";

            var uri = Constants.OAuthTokenUri;

            TestUtility.SetupMockAuthenticationErrorResponse(uri, postContent, errorJson, HttpStatusCode.Unauthorized);
            TestUtility.MOCK_TEST_CLIENT.Authorization = accessToken;

            Func<Task<TraktAuthorization>> act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync();
            act.Should().Throw<TraktAuthenticationException>().WithMessage(errorMessage);

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.NotFound);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync();
            act.Should().Throw<TraktNotFoundException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.BadRequest);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync();
            act.Should().Throw<TraktBadRequestException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.Conflict);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync();
            act.Should().Throw<TraktConflictException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.Forbidden);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync();
            act.Should().Throw<TraktForbiddenException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.MethodNotAllowed);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync();
            act.Should().Throw<TraktMethodNotFoundException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.InternalServerError);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync();
            act.Should().Throw<TraktServerException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.BadGateway);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync();
            act.Should().Throw<TraktBadGatewayException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)412);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync();
            act.Should().Throw<TraktPreconditionFailedException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)422);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync();
            act.Should().Throw<TraktValidationException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)429);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync();
            act.Should().Throw<TraktRateLimitException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)503);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync();
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)504);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync();
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)520);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync();
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)521);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync();
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)522);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync();
            act.Should().Throw<TraktServerUnavailableException>();
        }

        [TestMethod]
        public void TestTraktOAuthRefreshAuthorizationArgumentExceptions()
        {
            var accessToken = new TraktAuthorization
            {
                AccessToken = "mockAccessToken",
                TokenType = TraktAccessTokenType.Bearer,
                ExpiresInSeconds = 7200,
                RefreshToken = "mockRefreshToken",
                Scope = TraktAccessScope.Public
            };

            TestUtility.MOCK_TEST_CLIENT.Authorization = null;

            Func<Task<TraktAuthorization>> act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync();
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = new TraktAuthorization();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync();
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = new TraktAuthorization { RefreshToken = null, ExpiresInSeconds = 7200 };

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync();
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = new TraktAuthorization { RefreshToken = string.Empty, ExpiresInSeconds = 7200 };

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync();
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = new TraktAuthorization { RefreshToken = "mock refresh token", ExpiresInSeconds = 7200 };

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync();
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = new TraktAuthorization { RefreshToken = "mock refresh token", ExpiresInSeconds = 0 };

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync();
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = accessToken;
            TestUtility.MOCK_TEST_CLIENT.ClientId = null;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync();
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.ClientId = string.Empty;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync();
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.ClientId = "client id";

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync();
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.ClientId = "clientId";
            TestUtility.MOCK_TEST_CLIENT.ClientSecret = null;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync();
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.ClientSecret = string.Empty;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync();
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.ClientSecret = "client secret";

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync();
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.ClientSecret = "clientSecret";
            TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri = null;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync();
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri = string.Empty;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync();
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri = "redirect uri";

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync();
            act.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void TestTraktOAuthRefreshAuthorizationWithToken()
        {
            var clientId = TestUtility.MOCK_TEST_CLIENT.ClientId;
            var clientSecret = TestUtility.MOCK_TEST_CLIENT.ClientSecret;
            var redirectUri = TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri;
            var grantType = TraktAccessTokenGrantType.RefreshToken.ObjectName;

            var accessToken = new TraktAuthorization
            {
                AccessToken = "mockAccessToken",
                TokenType = TraktAccessTokenType.Bearer,
                ExpiresInSeconds = 7200,
                RefreshToken = "mockRefreshToken",
                Scope = TraktAccessScope.Public
            };

            var accessTokenJson = JsonConvert.SerializeObject(accessToken);
            accessTokenJson.Should().NotBeNullOrEmpty();

            var postContent = $"{{ \"refresh_token\": \"{accessToken.RefreshToken}\", \"client_id\": \"{clientId}\", " +
                              $"\"client_secret\": \"{clientSecret}\", \"redirect_uri\": " +
                              $"\"{redirectUri}\", \"grant_type\": \"{grantType}\" }}";

            TestUtility.SetupMockAuthenticationResponse(Constants.OAuthTokenUri, postContent, accessTokenJson);

            var response = TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken).Result;

            response.Should().NotBeNull();
            response.AccessToken.Should().Be(accessToken.AccessToken);
            response.TokenType.Should().Be(accessToken.TokenType);
            response.ExpiresInSeconds.Should().Be(accessToken.ExpiresInSeconds);
            response.RefreshToken.Should().Be(accessToken.RefreshToken);
            response.Scope.Should().Be(accessToken.Scope);
            response.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, 1800 * 1000);
            response.IsExpired.Should().BeFalse();

            var clientAccessToken = TestUtility.MOCK_TEST_CLIENT.Authorization;

            clientAccessToken.Should().NotBeNull();
            clientAccessToken.AccessToken.Should().Be(response.AccessToken);
            clientAccessToken.TokenType.Should().Be(response.TokenType);
            clientAccessToken.ExpiresInSeconds.Should().Be(response.ExpiresInSeconds);
            clientAccessToken.RefreshToken.Should().Be(response.RefreshToken);
            clientAccessToken.Scope.Should().Be(response.Scope);
            clientAccessToken.CreatedAt.Should().Be(response.CreatedAt);
            clientAccessToken.IsExpired.Should().BeFalse();
        }

        [TestMethod]
        public void TestTraktOAuthRefreshAuthorizationWithTokenExceptions()
        {
            var clientId = TestUtility.MOCK_TEST_CLIENT.ClientId;
            var clientSecret = TestUtility.MOCK_TEST_CLIENT.ClientSecret;
            var redirectUri = TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri;
            var grantType = TraktAccessTokenGrantType.RefreshToken.ObjectName;

            var accessToken = new TraktAuthorization
            {
                AccessToken = "mockAccessToken",
                TokenType = TraktAccessTokenType.Bearer,
                ExpiresInSeconds = 7200,
                RefreshToken = "mockRefreshToken",
                Scope = TraktAccessScope.Public
            };

            var error = new TraktError
            {
                Error = "invalid_grant",
                Description = "The provided authorization grant is invalid, expired, revoked, does not match the redirection URI used in the authorization request, or was issued to another client."
            };

            var errorMessage = $"error on refreshing oauth access token\nerror: {error.Error}\n" +
                               $"description: {error.Description}";

            var errorJson = JsonConvert.SerializeObject(error);
            errorJson.Should().NotBeNullOrEmpty();

            var postContent = $"{{ \"refresh_token\": \"{accessToken.RefreshToken}\", \"client_id\": \"{clientId}\", " +
                              $"\"client_secret\": \"{clientSecret}\", \"redirect_uri\": " +
                              $"\"{redirectUri}\", \"grant_type\": \"{grantType}\" }}";

            var uri = Constants.OAuthTokenUri;

            TestUtility.SetupMockAuthenticationErrorResponse(uri, postContent, errorJson, HttpStatusCode.Unauthorized);
            TestUtility.MOCK_TEST_CLIENT.Authorization = accessToken;

            Func<Task<TraktAuthorization>> act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken);
            act.Should().Throw<TraktAuthenticationException>().WithMessage(errorMessage);

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.NotFound);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken);
            act.Should().Throw<TraktNotFoundException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.BadRequest);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken);
            act.Should().Throw<TraktBadRequestException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.Conflict);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken);
            act.Should().Throw<TraktConflictException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.Forbidden);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken);
            act.Should().Throw<TraktForbiddenException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.MethodNotAllowed);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken);
            act.Should().Throw<TraktMethodNotFoundException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.InternalServerError);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken);
            act.Should().Throw<TraktServerException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.BadGateway);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken);
            act.Should().Throw<TraktBadGatewayException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)412);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken);
            act.Should().Throw<TraktPreconditionFailedException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)422);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken);
            act.Should().Throw<TraktValidationException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)429);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken);
            act.Should().Throw<TraktRateLimitException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)503);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken);
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)504);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken);
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)520);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken);
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)521);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken);
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)522);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken);
            act.Should().Throw<TraktServerUnavailableException>();
        }

        [TestMethod]
        public void TestTraktOAuthRefreshAuthorizationWithTokenArgumentExceptions()
        {
            var accessToken = new TraktAuthorization
            {
                AccessToken = "mockAccessToken",
                TokenType = TraktAccessTokenType.Bearer,
                ExpiresInSeconds = 7200,
                RefreshToken = "mockRefreshToken",
                Scope = TraktAccessScope.Public
            };

            TestUtility.MOCK_TEST_CLIENT.Authorization = null;

            Func<Task<TraktAuthorization>> act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(null);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(string.Empty);
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = new TraktAuthorization();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(null);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(string.Empty);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync("mock refresh token");
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = new TraktAuthorization { RefreshToken = null, ExpiresInSeconds = 7200 };

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(null);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(string.Empty);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync("mock refresh token");
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = new TraktAuthorization { RefreshToken = string.Empty, ExpiresInSeconds = 7200 };

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(null);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(string.Empty);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync("mock refresh token");
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = new TraktAuthorization { RefreshToken = "mock refresh token", ExpiresInSeconds = 7200 };

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(null);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(string.Empty);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync("mock refresh token");
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = new TraktAuthorization { RefreshToken = "mock refresh token", ExpiresInSeconds = 0 };

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(null);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(string.Empty);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync("mock refresh token");
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = accessToken;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(null);
            act.Should().Throw<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(string.Empty);
            act.Should().Throw<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync("mock refresh token");
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.ClientId = null;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken);
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.ClientId = string.Empty;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken);
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.ClientId = "client id";

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken);
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.ClientId = "clientId";
            TestUtility.MOCK_TEST_CLIENT.ClientSecret = null;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken);
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.ClientSecret = string.Empty;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken);
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.ClientSecret = "client secret";

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken);
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.ClientSecret = "clientSecret";
            TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri = null;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken);
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri = string.Empty;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken);
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri = "redirect uri";

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken);
            act.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void TestTraktOAuthRefreshAuthorizationWithTokenAndClientId()
        {
            var clientId = "clientId";
            var clientSecret = TestUtility.MOCK_TEST_CLIENT.ClientSecret;
            var redirectUri = TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri;
            var grantType = TraktAccessTokenGrantType.RefreshToken.ObjectName;

            var accessToken = new TraktAuthorization
            {
                AccessToken = "mockAccessToken",
                TokenType = TraktAccessTokenType.Bearer,
                ExpiresInSeconds = 7200,
                RefreshToken = "mockRefreshToken",
                Scope = TraktAccessScope.Public
            };

            var accessTokenJson = JsonConvert.SerializeObject(accessToken);
            accessTokenJson.Should().NotBeNullOrEmpty();

            var postContent = $"{{ \"refresh_token\": \"{accessToken.RefreshToken}\", \"client_id\": \"{clientId}\", " +
                              $"\"client_secret\": \"{clientSecret}\", \"redirect_uri\": " +
                              $"\"{redirectUri}\", \"grant_type\": \"{grantType}\" }}";

            TestUtility.SetupMockAuthenticationResponse(Constants.OAuthTokenUri, postContent, accessTokenJson);

            var response = TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId).Result;

            response.Should().NotBeNull();
            response.AccessToken.Should().Be(accessToken.AccessToken);
            response.TokenType.Should().Be(accessToken.TokenType);
            response.ExpiresInSeconds.Should().Be(accessToken.ExpiresInSeconds);
            response.RefreshToken.Should().Be(accessToken.RefreshToken);
            response.Scope.Should().Be(accessToken.Scope);
            response.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, 1800 * 1000);
            response.IsExpired.Should().BeFalse();

            var clientAccessToken = TestUtility.MOCK_TEST_CLIENT.Authorization;

            clientAccessToken.Should().NotBeNull();
            clientAccessToken.AccessToken.Should().Be(response.AccessToken);
            clientAccessToken.TokenType.Should().Be(response.TokenType);
            clientAccessToken.ExpiresInSeconds.Should().Be(response.ExpiresInSeconds);
            clientAccessToken.RefreshToken.Should().Be(response.RefreshToken);
            clientAccessToken.Scope.Should().Be(response.Scope);
            clientAccessToken.CreatedAt.Should().Be(response.CreatedAt);
            clientAccessToken.IsExpired.Should().BeFalse();
        }

        [TestMethod]
        public void TestTraktOAuthRefreshAuthorizationWithTokenAndClientIdExceptions()
        {
            var clientId = "clientId";
            var clientSecret = TestUtility.MOCK_TEST_CLIENT.ClientSecret;
            var redirectUri = TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri;
            var grantType = TraktAccessTokenGrantType.RefreshToken.ObjectName;

            var accessToken = new TraktAuthorization
            {
                AccessToken = "mockAccessToken",
                TokenType = TraktAccessTokenType.Bearer,
                ExpiresInSeconds = 7200,
                RefreshToken = "mockRefreshToken",
                Scope = TraktAccessScope.Public
            };

            var error = new TraktError
            {
                Error = "invalid_grant",
                Description = "The provided authorization grant is invalid, expired, revoked, does not match the redirection URI used in the authorization request, or was issued to another client."
            };

            var errorMessage = $"error on refreshing oauth access token\nerror: {error.Error}\n" +
                               $"description: {error.Description}";

            var errorJson = JsonConvert.SerializeObject(error);
            errorJson.Should().NotBeNullOrEmpty();

            var postContent = $"{{ \"refresh_token\": \"{accessToken.RefreshToken}\", \"client_id\": \"{clientId}\", " +
                              $"\"client_secret\": \"{clientSecret}\", \"redirect_uri\": " +
                              $"\"{redirectUri}\", \"grant_type\": \"{grantType}\" }}";

            var uri = Constants.OAuthTokenUri;

            TestUtility.SetupMockAuthenticationErrorResponse(uri, postContent, errorJson, HttpStatusCode.Unauthorized);
            TestUtility.MOCK_TEST_CLIENT.Authorization = accessToken;

            Func<Task<TraktAuthorization>> act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId);
            act.Should().Throw<TraktAuthenticationException>().WithMessage(errorMessage);

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.NotFound);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId);
            act.Should().Throw<TraktNotFoundException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.BadRequest);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId);
            act.Should().Throw<TraktBadRequestException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.Conflict);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId);
            act.Should().Throw<TraktConflictException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.Forbidden);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId);
            act.Should().Throw<TraktForbiddenException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.MethodNotAllowed);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId);
            act.Should().Throw<TraktMethodNotFoundException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.InternalServerError);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId);
            act.Should().Throw<TraktServerException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.BadGateway);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId);
            act.Should().Throw<TraktBadGatewayException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)412);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId);
            act.Should().Throw<TraktPreconditionFailedException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)422);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId);
            act.Should().Throw<TraktValidationException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)429);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId);
            act.Should().Throw<TraktRateLimitException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)503);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId);
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)504);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId);
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)520);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId);
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)521);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId);
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)522);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId);
            act.Should().Throw<TraktServerUnavailableException>();
        }

        [TestMethod]
        public void TestTraktOAuthRefreshAuthorizationWithTokenAndClientIdArgumentExceptions()
        {
            var accessToken = new TraktAuthorization
            {
                AccessToken = "mockAccessToken",
                TokenType = TraktAccessTokenType.Bearer,
                ExpiresInSeconds = 7200,
                RefreshToken = "mockRefreshToken",
                Scope = TraktAccessScope.Public
            };

            var clientId = "clientId";

            TestUtility.MOCK_TEST_CLIENT.Authorization = null;

            Func<Task<TraktAuthorization>> act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(null, clientId);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(string.Empty, clientId);
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = new TraktAuthorization();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(null, clientId);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(string.Empty, clientId);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync("mock refresh token", clientId);
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = new TraktAuthorization { RefreshToken = null, ExpiresInSeconds = 7200 };

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(null, clientId);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(string.Empty, clientId);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync("mock refresh token", clientId);
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = new TraktAuthorization { RefreshToken = string.Empty, ExpiresInSeconds = 7200 };

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(null, clientId);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(string.Empty, clientId);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync("mock refresh token", clientId);
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = new TraktAuthorization { RefreshToken = "mock refresh token", ExpiresInSeconds = 7200 };

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(null, clientId);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(string.Empty, clientId);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync("mock refresh token", clientId);
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = new TraktAuthorization { RefreshToken = "mock refresh token", ExpiresInSeconds = 0 };

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(null, clientId);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(string.Empty, clientId);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync("mock refresh token", clientId);
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = accessToken;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(null, clientId);
            act.Should().Throw<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(string.Empty, clientId);
            act.Should().Throw<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync("mock refresh token", clientId);
            act.Should().Throw<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, null);
            act.Should().Throw<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, string.Empty);
            act.Should().Throw<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, "client id");
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.ClientSecret = null;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId);
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.ClientSecret = string.Empty;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId);
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.ClientSecret = "client secret";

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId);
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.ClientSecret = "clientSecret";
            TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri = null;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId);
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri = string.Empty;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId);
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri = "redirect uri";

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId);
            act.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void TestTraktOAuthRefreshAuthorizationWithTokenAndClientIdAndClientSecret()
        {
            var clientId = "clientId";
            var clientSecret = "clientSecret";
            var redirectUri = TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri;
            var grantType = TraktAccessTokenGrantType.RefreshToken.ObjectName;

            var accessToken = new TraktAuthorization
            {
                AccessToken = "mockAccessToken",
                TokenType = TraktAccessTokenType.Bearer,
                ExpiresInSeconds = 7200,
                RefreshToken = "mockRefreshToken",
                Scope = TraktAccessScope.Public
            };

            var accessTokenJson = JsonConvert.SerializeObject(accessToken);
            accessTokenJson.Should().NotBeNullOrEmpty();

            var postContent = $"{{ \"refresh_token\": \"{accessToken.RefreshToken}\", \"client_id\": \"{clientId}\", " +
                              $"\"client_secret\": \"{clientSecret}\", \"redirect_uri\": " +
                              $"\"{redirectUri}\", \"grant_type\": \"{grantType}\" }}";

            TestUtility.SetupMockAuthenticationResponse(Constants.OAuthTokenUri, postContent, accessTokenJson);

            var response = TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken,
                                                                                      clientId, clientSecret).Result;

            response.Should().NotBeNull();
            response.AccessToken.Should().Be(accessToken.AccessToken);
            response.TokenType.Should().Be(accessToken.TokenType);
            response.ExpiresInSeconds.Should().Be(accessToken.ExpiresInSeconds);
            response.RefreshToken.Should().Be(accessToken.RefreshToken);
            response.Scope.Should().Be(accessToken.Scope);
            response.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, 1800 * 1000);
            response.IsExpired.Should().BeFalse();

            var clientAccessToken = TestUtility.MOCK_TEST_CLIENT.Authorization;

            clientAccessToken.Should().NotBeNull();
            clientAccessToken.AccessToken.Should().Be(response.AccessToken);
            clientAccessToken.TokenType.Should().Be(response.TokenType);
            clientAccessToken.ExpiresInSeconds.Should().Be(response.ExpiresInSeconds);
            clientAccessToken.RefreshToken.Should().Be(response.RefreshToken);
            clientAccessToken.Scope.Should().Be(response.Scope);
            clientAccessToken.CreatedAt.Should().Be(response.CreatedAt);
            clientAccessToken.IsExpired.Should().BeFalse();
        }

        [TestMethod]
        public void TestTraktOAuthRefreshAuthorizationWithTokenAndClientIdAndClientSecretExceptions()
        {
            var clientId = "clientId";
            var clientSecret = "clientSecret";
            var redirectUri = TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri;
            var grantType = TraktAccessTokenGrantType.RefreshToken.ObjectName;

            var accessToken = new TraktAuthorization
            {
                AccessToken = "mockAccessToken",
                TokenType = TraktAccessTokenType.Bearer,
                ExpiresInSeconds = 7200,
                RefreshToken = "mockRefreshToken",
                Scope = TraktAccessScope.Public
            };

            var error = new TraktError
            {
                Error = "invalid_grant",
                Description = "The provided authorization grant is invalid, expired, revoked, does not match the redirection URI used in the authorization request, or was issued to another client."
            };

            var errorMessage = $"error on refreshing oauth access token\nerror: {error.Error}\n" +
                               $"description: {error.Description}";

            var errorJson = JsonConvert.SerializeObject(error);
            errorJson.Should().NotBeNullOrEmpty();

            var postContent = $"{{ \"refresh_token\": \"{accessToken.RefreshToken}\", \"client_id\": \"{clientId}\", " +
                              $"\"client_secret\": \"{clientSecret}\", \"redirect_uri\": " +
                              $"\"{redirectUri}\", \"grant_type\": \"{grantType}\" }}";

            var uri = Constants.OAuthTokenUri;

            TestUtility.SetupMockAuthenticationErrorResponse(uri, postContent, errorJson, HttpStatusCode.Unauthorized);
            TestUtility.MOCK_TEST_CLIENT.Authorization = accessToken;

            Func<Task<TraktAuthorization>> act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId, clientSecret);
            act.Should().Throw<TraktAuthenticationException>().WithMessage(errorMessage);

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.NotFound);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId, clientSecret);
            act.Should().Throw<TraktNotFoundException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.BadRequest);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId, clientSecret);
            act.Should().Throw<TraktBadRequestException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.Conflict);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId, clientSecret);
            act.Should().Throw<TraktConflictException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.Forbidden);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId, clientSecret);
            act.Should().Throw<TraktForbiddenException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.MethodNotAllowed);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId, clientSecret);
            act.Should().Throw<TraktMethodNotFoundException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.InternalServerError);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId, clientSecret);
            act.Should().Throw<TraktServerException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.BadGateway);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId, clientSecret);
            act.Should().Throw<TraktBadGatewayException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)412);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId, clientSecret);
            act.Should().Throw<TraktPreconditionFailedException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)422);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId, clientSecret);
            act.Should().Throw<TraktValidationException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)429);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId, clientSecret);
            act.Should().Throw<TraktRateLimitException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)503);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId, clientSecret);
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)504);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId, clientSecret);
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)520);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId, clientSecret);
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)521);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId, clientSecret);
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)522);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId, clientSecret);
            act.Should().Throw<TraktServerUnavailableException>();
        }

        [TestMethod]
        public void TestTraktOAuthRefreshAuthorizationWithTokenAndClientIdAndClientSecretArgumentExceptions()
        {
            var accessToken = new TraktAuthorization
            {
                AccessToken = "mockAccessToken",
                TokenType = TraktAccessTokenType.Bearer,
                ExpiresInSeconds = 7200,
                RefreshToken = "mockRefreshToken",
                Scope = TraktAccessScope.Public
            };

            var clientId = "clientId";
            var clientSecret = "clientSecret";

            TestUtility.MOCK_TEST_CLIENT.Authorization = null;

            Func<Task<TraktAuthorization>> act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(null, clientId, clientSecret);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(string.Empty, clientId, clientSecret);
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = new TraktAuthorization();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(null, clientId, clientSecret);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(string.Empty, clientId, clientSecret);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync("mock refresh token", clientId, clientSecret);
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = new TraktAuthorization { RefreshToken = null, ExpiresInSeconds = 7200 };

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(null, clientId, clientSecret);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(string.Empty, clientId, clientSecret);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync("mock refresh token", clientId, clientSecret);
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = new TraktAuthorization { RefreshToken = string.Empty, ExpiresInSeconds = 7200 };

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(null, clientId, clientSecret);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(string.Empty, clientId, clientSecret);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync("mock refresh token", clientId, clientSecret);
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = new TraktAuthorization { RefreshToken = "mock refresh token", ExpiresInSeconds = 7200 };

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(null, clientId, clientSecret);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(string.Empty, clientId, clientSecret);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync("mock refresh token", clientId, clientSecret);
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = new TraktAuthorization { RefreshToken = "mock refresh token", ExpiresInSeconds = 0 };

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(null, clientId, clientSecret);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(string.Empty, clientId, clientSecret);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync("mock refresh token", clientId, clientSecret);
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = accessToken;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(null, clientId, clientSecret);
            act.Should().Throw<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(string.Empty, clientId, clientSecret);
            act.Should().Throw<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync("mock refresh token", clientId, clientSecret);
            act.Should().Throw<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, null, clientSecret);
            act.Should().Throw<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, string.Empty, clientSecret);
            act.Should().Throw<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, "client id", clientSecret);
            act.Should().Throw<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId, null);
            act.Should().Throw<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId, string.Empty);
            act.Should().Throw<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId, "client secret");
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri = null;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId, clientSecret);
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri = string.Empty;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId);
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.Authentication.RedirectUri = "redirect uri";

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId);
            act.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void TestTraktOAuthRefreshAuthorizationWithTokenAndClientIdAndClientSecretAndRedirectUri()
        {
            var clientId = "clientId";
            var clientSecret = "clientSecret";
            var redirectUri = "redirectUri";
            var grantType = TraktAccessTokenGrantType.RefreshToken.ObjectName;

            var accessToken = new TraktAuthorization
            {
                AccessToken = "mockAccessToken",
                TokenType = TraktAccessTokenType.Bearer,
                ExpiresInSeconds = 7200,
                RefreshToken = "mockRefreshToken",
                Scope = TraktAccessScope.Public
            };

            var accessTokenJson = JsonConvert.SerializeObject(accessToken);
            accessTokenJson.Should().NotBeNullOrEmpty();

            var postContent = $"{{ \"refresh_token\": \"{accessToken.RefreshToken}\", \"client_id\": \"{clientId}\", " +
                              $"\"client_secret\": \"{clientSecret}\", \"redirect_uri\": " +
                              $"\"{redirectUri}\", \"grant_type\": \"{grantType}\" }}";

            TestUtility.SetupMockAuthenticationResponse(Constants.OAuthTokenUri, postContent, accessTokenJson);

            var response = TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken,
                                                                                      clientId, clientSecret,
                                                                                      redirectUri).Result;

            response.Should().NotBeNull();
            response.AccessToken.Should().Be(accessToken.AccessToken);
            response.TokenType.Should().Be(accessToken.TokenType);
            response.ExpiresInSeconds.Should().Be(accessToken.ExpiresInSeconds);
            response.RefreshToken.Should().Be(accessToken.RefreshToken);
            response.Scope.Should().Be(accessToken.Scope);
            response.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, 1800 * 1000);
            response.IsExpired.Should().BeFalse();

            var clientAccessToken = TestUtility.MOCK_TEST_CLIENT.Authorization;

            clientAccessToken.Should().NotBeNull();
            clientAccessToken.AccessToken.Should().Be(response.AccessToken);
            clientAccessToken.TokenType.Should().Be(response.TokenType);
            clientAccessToken.ExpiresInSeconds.Should().Be(response.ExpiresInSeconds);
            clientAccessToken.RefreshToken.Should().Be(response.RefreshToken);
            clientAccessToken.Scope.Should().Be(response.Scope);
            clientAccessToken.CreatedAt.Should().Be(response.CreatedAt);
            clientAccessToken.IsExpired.Should().BeFalse();
        }

        [TestMethod]
        public void TestTraktOAuthRefreshAuthorizationWithTokenAndClientIdAndClientSecretAndRedirectUriExceptions()
        {
            var clientId = "clientId";
            var clientSecret = "clientSecret";
            var redirectUri = "redirectUri";
            var grantType = TraktAccessTokenGrantType.RefreshToken.ObjectName;

            var accessToken = new TraktAuthorization
            {
                AccessToken = "mockAccessToken",
                TokenType = TraktAccessTokenType.Bearer,
                ExpiresInSeconds = 7200,
                RefreshToken = "mockRefreshToken",
                Scope = TraktAccessScope.Public
            };

            var error = new TraktError
            {
                Error = "invalid_grant",
                Description = "The provided authorization grant is invalid, expired, revoked, does not match the redirection URI used in the authorization request, or was issued to another client."
            };

            var errorMessage = $"error on refreshing oauth access token\nerror: {error.Error}\n" +
                               $"description: {error.Description}";

            var errorJson = JsonConvert.SerializeObject(error);
            errorJson.Should().NotBeNullOrEmpty();

            var postContent = $"{{ \"refresh_token\": \"{accessToken.RefreshToken}\", \"client_id\": \"{clientId}\", " +
                              $"\"client_secret\": \"{clientSecret}\", \"redirect_uri\": " +
                              $"\"{redirectUri}\", \"grant_type\": \"{grantType}\" }}";

            var uri = Constants.OAuthTokenUri;

            TestUtility.SetupMockAuthenticationErrorResponse(uri, postContent, errorJson, HttpStatusCode.Unauthorized);
            TestUtility.MOCK_TEST_CLIENT.Authorization = accessToken;

            Func<Task<TraktAuthorization>> act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktAuthenticationException>().WithMessage(errorMessage);

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.NotFound);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktNotFoundException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.BadRequest);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktBadRequestException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.Conflict);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktConflictException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.Forbidden);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktForbiddenException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.MethodNotAllowed);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktMethodNotFoundException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.InternalServerError);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktServerException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.BadGateway);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktBadGatewayException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)412);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktPreconditionFailedException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)422);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktValidationException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)429);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktRateLimitException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)503);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)504);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)520);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)521);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)522);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktServerUnavailableException>();
        }

        [TestMethod]
        public void TestTraktOAuthRefreshAuthorizationWithTokenAndClientIdAndClientSecretAndRedirectUriArgumentExceptions()
        {
            var accessToken = new TraktAuthorization
            {
                AccessToken = "mockAccessToken",
                TokenType = TraktAccessTokenType.Bearer,
                ExpiresInSeconds = 7200,
                RefreshToken = "mockRefreshToken",
                Scope = TraktAccessScope.Public
            };

            var clientId = "clientId";
            var clientSecret = "clientSecret";
            var redirectUri = "redirectUri";

            TestUtility.MOCK_TEST_CLIENT.Authorization = null;

            Func<Task<TraktAuthorization>> act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(null, clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(string.Empty, clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = new TraktAuthorization();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(null, clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(string.Empty, clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync("mock refresh token", clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = new TraktAuthorization { RefreshToken = null, ExpiresInSeconds = 7200 };

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(null, clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(string.Empty, clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync("mock refresh token", clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = new TraktAuthorization { RefreshToken = string.Empty, ExpiresInSeconds = 7200 };

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(null, clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(string.Empty, clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync("mock refresh token", clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = new TraktAuthorization { RefreshToken = "mock refresh token", ExpiresInSeconds = 7200 };

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(null, clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(string.Empty, clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync("mock refresh token", clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = new TraktAuthorization { RefreshToken = "mock refresh token", ExpiresInSeconds = 0 };

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(null, clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(string.Empty, clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync("mock refresh token", clientId, clientSecret, redirectUri);
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = accessToken;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(null, clientId, clientSecret, redirectUri);
            act.Should().Throw<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(string.Empty, clientId, clientSecret, redirectUri);
            act.Should().Throw<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync("mock refresh token", clientId, clientSecret, redirectUri);
            act.Should().Throw<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, null, clientSecret, redirectUri);
            act.Should().Throw<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, string.Empty, clientSecret, redirectUri);
            act.Should().Throw<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, "client id", clientSecret, redirectUri);
            act.Should().Throw<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId, null, redirectUri);
            act.Should().Throw<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId, string.Empty, redirectUri);
            act.Should().Throw<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId, "client secret", redirectUri);
            act.Should().Throw<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId, clientSecret, null);
            act.Should().Throw<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId, string.Empty);
            act.Should().Throw<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RefreshAuthorizationAsync(accessToken.RefreshToken, clientId, "redirect uri");
            act.Should().Throw<ArgumentException>();
        }

        #endregion

        // -----------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------

        #region RevokeAuthorization

        [TestMethod]
        public void TestTraktOAuthRevokeAuthorization()
        {
            var accessToken = new TraktAuthorization
            {
                AccessToken = "mockAccessToken",
                TokenType = TraktAccessTokenType.Bearer,
                ExpiresInSeconds = 7200,
                RefreshToken = "mockRefreshToken",
                Scope = TraktAccessScope.Public
            };

            var postContent = $"token={accessToken.AccessToken}";

            var uri = Constants.OAuthRevokeUri;

            TestUtility.MOCK_TEST_CLIENT.Authorization = accessToken;
            TestUtility.SetupMockAuthenticationTokenRevokeResponse(uri, postContent);

            Func<Task> act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync();
            act.Should().NotThrow();

            var authorization = TestUtility.MOCK_TEST_CLIENT.Authorization;
            authorization.Should().NotBeNull();
            authorization.AccessToken.Should().NotBeNull().And.BeEmpty();
            authorization.RefreshToken.Should().NotBeNull().And.BeEmpty();
            authorization.Scope.Should().Be(TraktAccessScope.Public);
            authorization.TokenType.Should().Be(TraktAccessTokenType.Bearer);
            authorization.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, 1000);
            authorization.ExpiresInSeconds.Should().Be(0);
            authorization.IgnoreExpiration.Should().BeTrue();
            authorization.IsExpired.Should().BeTrue();
            authorization.IsRefreshPossible.Should().BeFalse();
            authorization.IsValid.Should().BeFalse();
        }

        [TestMethod]
        public void TestTraktOAuthRevokeAuthorizationExceptions()
        {
            var accessToken = new TraktAuthorization
            {
                AccessToken = "mockAccessToken",
                TokenType = TraktAccessTokenType.Bearer,
                ExpiresInSeconds = 7200,
                RefreshToken = "mockRefreshToken",
                Scope = TraktAccessScope.Public
            };

            var uri = Constants.OAuthRevokeUri;

            TestUtility.MOCK_TEST_CLIENT.Authorization = accessToken;

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.Unauthorized);

            Func<Task> act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync();
            act.Should().Throw<TraktAuthenticationException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.NotFound);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync();
            act.Should().Throw<TraktNotFoundException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.BadRequest);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync();
            act.Should().Throw<TraktBadRequestException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.Conflict);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync();
            act.Should().Throw<TraktConflictException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.Forbidden);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync();
            act.Should().Throw<TraktForbiddenException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.MethodNotAllowed);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync();
            act.Should().Throw<TraktMethodNotFoundException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.InternalServerError);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync();
            act.Should().Throw<TraktServerException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.BadGateway);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync();
            act.Should().Throw<TraktBadGatewayException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)412);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync();
            act.Should().Throw<TraktPreconditionFailedException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)422);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync();
            act.Should().Throw<TraktValidationException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)429);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync();
            act.Should().Throw<TraktRateLimitException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)503);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync();
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)504);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync();
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)520);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync();
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)521);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync();
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)522);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync();
            act.Should().Throw<TraktServerUnavailableException>();
        }

        [TestMethod]
        public void TestTraktOAuthRevokeAuthorizationArgumentExceptions()
        {
            var accessToken = new TraktAuthorization
            {
                AccessToken = "mockAccessToken",
                TokenType = TraktAccessTokenType.Bearer,
                ExpiresInSeconds = 7200,
                RefreshToken = "mockRefreshToken",
                Scope = TraktAccessScope.Public
            };

            TestUtility.MOCK_TEST_CLIENT.Authorization = null;

            Func<Task> act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync();
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = new TraktAuthorization();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync();
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = new TraktAuthorization { RefreshToken = null, ExpiresInSeconds = 7200 };

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync();
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = new TraktAuthorization { RefreshToken = string.Empty, ExpiresInSeconds = 7200 };

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync();
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = new TraktAuthorization { RefreshToken = "mock refresh token", ExpiresInSeconds = 7200 };

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync();
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = new TraktAuthorization { RefreshToken = "mock refresh token", ExpiresInSeconds = 0 };

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync();
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = accessToken;
            TestUtility.MOCK_TEST_CLIENT.ClientId = null;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync();
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.ClientId = string.Empty;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync();
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.ClientId = "client id";

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync();
            act.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void TestTraktOAuthRevokeAuthorizationWithToken()
        {
            var accessToken = new TraktAuthorization
            {
                AccessToken = "mockAccessToken",
                TokenType = TraktAccessTokenType.Bearer,
                ExpiresInSeconds = 7200,
                RefreshToken = "mockRefreshToken",
                Scope = TraktAccessScope.Public
            };

            var postContent = $"token={accessToken.AccessToken}";

            var uri = Constants.OAuthRevokeUri;

            TestUtility.MOCK_TEST_CLIENT.Authorization = accessToken;
            TestUtility.SetupMockAuthenticationTokenRevokeResponse(uri, postContent);

            Func<Task> act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(accessToken.AccessToken);
            act.Should().NotThrow();

            var authorization = TestUtility.MOCK_TEST_CLIENT.Authorization;
            authorization.Should().NotBeNull();
            authorization.AccessToken.Should().NotBeNull().And.BeEmpty();
            authorization.RefreshToken.Should().NotBeNull().And.BeEmpty();
            authorization.Scope.Should().Be(TraktAccessScope.Public);
            authorization.TokenType.Should().Be(TraktAccessTokenType.Bearer);
            authorization.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, 1000);
            authorization.ExpiresInSeconds.Should().Be(0);
            authorization.IgnoreExpiration.Should().BeTrue();
            authorization.IsExpired.Should().BeTrue();
            authorization.IsRefreshPossible.Should().BeFalse();
            authorization.IsValid.Should().BeFalse();
        }

        [TestMethod]
        public void TestTraktOAuthRevokeAuthorizationWithTokenExceptions()
        {
            var accessToken = new TraktAuthorization
            {
                AccessToken = "mockAccessToken",
                TokenType = TraktAccessTokenType.Bearer,
                ExpiresInSeconds = 7200,
                RefreshToken = "mockRefreshToken",
                Scope = TraktAccessScope.Public
            };

            var uri = Constants.OAuthRevokeUri;

            TestUtility.MOCK_TEST_CLIENT.Authorization = accessToken;

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.Unauthorized);

            Func<Task> act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(accessToken.AccessToken);
            act.Should().Throw<TraktAuthenticationException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.NotFound);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(accessToken.AccessToken);
            act.Should().Throw<TraktNotFoundException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.BadRequest);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(accessToken.AccessToken);
            act.Should().Throw<TraktBadRequestException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.Conflict);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(accessToken.AccessToken);
            act.Should().Throw<TraktConflictException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.Forbidden);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(accessToken.AccessToken);
            act.Should().Throw<TraktForbiddenException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.MethodNotAllowed);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(accessToken.AccessToken);
            act.Should().Throw<TraktMethodNotFoundException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.InternalServerError);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(accessToken.AccessToken);
            act.Should().Throw<TraktServerException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.BadGateway);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(accessToken.AccessToken);
            act.Should().Throw<TraktBadGatewayException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)412);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(accessToken.AccessToken);
            act.Should().Throw<TraktPreconditionFailedException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)422);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(accessToken.AccessToken);
            act.Should().Throw<TraktValidationException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)429);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(accessToken.AccessToken);
            act.Should().Throw<TraktRateLimitException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)503);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(accessToken.AccessToken);
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)504);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(accessToken.AccessToken);
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)520);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(accessToken.AccessToken);
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)521);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(accessToken.AccessToken);
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)522);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(accessToken.AccessToken);
            act.Should().Throw<TraktServerUnavailableException>();
        }

        [TestMethod]
        public void TestTraktOAuthRevokeAuthorizationWithTokenArgumentExceptions()
        {
            var accessToken = new TraktAuthorization
            {
                AccessToken = "mockAccessToken",
                TokenType = TraktAccessTokenType.Bearer,
                ExpiresInSeconds = 7200,
                RefreshToken = "mockRefreshToken",
                Scope = TraktAccessScope.Public
            };

            TestUtility.MOCK_TEST_CLIENT.Authorization = null;

            Func<Task> act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(null);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(string.Empty);
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = new TraktAuthorization();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(null);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(string.Empty);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync("mock refresh token");
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = new TraktAuthorization { RefreshToken = null, ExpiresInSeconds = 7200 };

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(null);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(string.Empty);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync("mock refresh token");
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = new TraktAuthorization { RefreshToken = string.Empty, ExpiresInSeconds = 7200 };

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(null);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(string.Empty);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync("mock refresh token");
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = new TraktAuthorization { RefreshToken = "mock refresh token", ExpiresInSeconds = 7200 };

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(null);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(string.Empty);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync("mock refresh token");
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = new TraktAuthorization { RefreshToken = "mock refresh token", ExpiresInSeconds = 0 };

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(null);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(string.Empty);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync("mock refresh token");
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = accessToken;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(null);
            act.Should().Throw<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(string.Empty);
            act.Should().Throw<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync("mock refresh token");
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.ClientId = null;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(accessToken.RefreshToken);
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.ClientId = string.Empty;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(accessToken.RefreshToken);
            act.Should().Throw<ArgumentException>();

            TestUtility.MOCK_TEST_CLIENT.ClientId = "client id";

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(accessToken.RefreshToken);
            act.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void TestTraktOAuthRevokeAuthorizationWithTokenAndClientId()
        {
            var clientId = TestUtility.MOCK_TEST_CLIENT.ClientId;

            var accessToken = new TraktAuthorization
            {
                AccessToken = "mockAccessToken",
                TokenType = TraktAccessTokenType.Bearer,
                ExpiresInSeconds = 7200,
                RefreshToken = "mockRefreshToken",
                Scope = TraktAccessScope.Public
            };

            var postContent = $"token={accessToken.AccessToken}";

            var uri = Constants.OAuthRevokeUri;

            TestUtility.MOCK_TEST_CLIENT.Authorization = accessToken;
            TestUtility.SetupMockAuthenticationTokenRevokeResponse(uri, postContent);

            Func<Task> act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(accessToken.AccessToken, clientId);
            act.Should().NotThrow();

            var authorization = TestUtility.MOCK_TEST_CLIENT.Authorization;
            authorization.Should().NotBeNull();
            authorization.AccessToken.Should().NotBeNull().And.BeEmpty();
            authorization.RefreshToken.Should().NotBeNull().And.BeEmpty();
            authorization.Scope.Should().Be(TraktAccessScope.Public);
            authorization.TokenType.Should().Be(TraktAccessTokenType.Bearer);
            authorization.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, 1000);
            authorization.ExpiresInSeconds.Should().Be(0);
            authorization.IgnoreExpiration.Should().BeTrue();
            authorization.IsExpired.Should().BeTrue();
            authorization.IsRefreshPossible.Should().BeFalse();
            authorization.IsValid.Should().BeFalse();
        }

        [TestMethod]
        public void TestTraktOAuthRevokeAuthorizationWithTokenAndClientIdExceptions()
        {
            var clientId = "clientId";

            var accessToken = new TraktAuthorization
            {
                AccessToken = "mockAccessToken",
                TokenType = TraktAccessTokenType.Bearer,
                ExpiresInSeconds = 7200,
                RefreshToken = "mockRefreshToken",
                Scope = TraktAccessScope.Public
            };

            var uri = Constants.OAuthRevokeUri;

            TestUtility.MOCK_TEST_CLIENT.Authorization = accessToken;

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.Unauthorized);

            Func<Task> act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(accessToken.AccessToken, clientId);
            act.Should().Throw<TraktAuthenticationException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.NotFound);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(accessToken.AccessToken, clientId);
            act.Should().Throw<TraktNotFoundException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.BadRequest);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(accessToken.AccessToken, clientId);
            act.Should().Throw<TraktBadRequestException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.Conflict);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(accessToken.AccessToken, clientId);
            act.Should().Throw<TraktConflictException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.Forbidden);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(accessToken.AccessToken, clientId);
            act.Should().Throw<TraktForbiddenException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.MethodNotAllowed);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(accessToken.AccessToken, clientId);
            act.Should().Throw<TraktMethodNotFoundException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.InternalServerError);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(accessToken.AccessToken, clientId);
            act.Should().Throw<TraktServerException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, HttpStatusCode.BadGateway);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(accessToken.AccessToken, clientId);
            act.Should().Throw<TraktBadGatewayException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)412);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(accessToken.AccessToken, clientId);
            act.Should().Throw<TraktPreconditionFailedException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)422);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(accessToken.AccessToken, clientId);
            act.Should().Throw<TraktValidationException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)429);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(accessToken.AccessToken, clientId);
            act.Should().Throw<TraktRateLimitException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)503);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(accessToken.AccessToken, clientId);
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)504);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(accessToken.AccessToken, clientId);
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)520);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(accessToken.AccessToken, clientId);
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)521);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(accessToken.AccessToken, clientId);
            act.Should().Throw<TraktServerUnavailableException>();

            TestUtility.SetupMockAuthenticationErrorResponse(uri, (HttpStatusCode)522);

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(accessToken.AccessToken, clientId);
            act.Should().Throw<TraktServerUnavailableException>();
        }

        [TestMethod]
        public void TestTraktOAuthRevokeAuthorizationWithTokenAndClientIdArgumentExceptions()
        {
            var accessToken = new TraktAuthorization
            {
                AccessToken = "mockAccessToken",
                TokenType = TraktAccessTokenType.Bearer,
                ExpiresInSeconds = 7200,
                RefreshToken = "mockRefreshToken",
                Scope = TraktAccessScope.Public
            };

            var clientId = "clientId";

            TestUtility.MOCK_TEST_CLIENT.Authorization = null;

            Func<Task> act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(null, clientId);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(string.Empty, clientId);
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = new TraktAuthorization();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(null, clientId);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(string.Empty, clientId);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync("mock refresh token", clientId);
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = new TraktAuthorization { RefreshToken = null, ExpiresInSeconds = 7200 };

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(null, clientId);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(string.Empty, clientId);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync("mock refresh token", clientId);
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = new TraktAuthorization { RefreshToken = string.Empty, ExpiresInSeconds = 7200 };

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(null, clientId);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(string.Empty, clientId);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync("mock refresh token", clientId);
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = new TraktAuthorization { RefreshToken = "mock refresh token", ExpiresInSeconds = 7200 };

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(null, clientId);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(string.Empty, clientId);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync("mock refresh token", clientId);
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = new TraktAuthorization { RefreshToken = "mock refresh token", ExpiresInSeconds = 0 };

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(null, clientId);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(string.Empty, clientId);
            act.Should().Throw<TraktAuthorizationException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync("mock refresh token", clientId);
            act.Should().Throw<TraktAuthorizationException>();

            TestUtility.MOCK_TEST_CLIENT.Authorization = accessToken;

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(null, clientId);
            act.Should().Throw<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(string.Empty, clientId);
            act.Should().Throw<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync("mock refresh token", clientId);
            act.Should().Throw<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(accessToken.RefreshToken, null);
            act.Should().Throw<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(accessToken.RefreshToken, string.Empty);
            act.Should().Throw<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.OAuth.RevokeAuthorizationAsync(accessToken.RefreshToken, "client id");
            act.Should().Throw<ArgumentException>();
        }

        #endregion
    }
}
