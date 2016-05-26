﻿namespace TraktApiSharp.Tests.Modules
{
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using TraktApiSharp.Enums;
    using TraktApiSharp.Exceptions;
    using TraktApiSharp.Modules;
    using TraktApiSharp.Objects.Basic;
    using TraktApiSharp.Objects.Get.Shows;
    using TraktApiSharp.Objects.Get.Shows.Common;
    using TraktApiSharp.Requests;
    using Utils;

    [TestClass]
    public class TraktShowsModuleTests
    {
        [TestMethod]
        public void TestTraktShowsModuleIsModule()
        {
            typeof(TraktBaseModule).IsAssignableFrom(typeof(TraktShowsModule)).Should().BeTrue();
        }

        [ClassInitialize]
        public static void InitializeTests(TestContext context)
        {
            TestUtility.SetupMockHttpClient();
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
        }

        // -----------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------

        #region SingleShow

        [TestMethod]
        public void TestTraktShowsModuleGetShow()
        {
            var show = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowSummaryFullAndImages.json");
            show.Should().NotBeNullOrEmpty();

            var showId = "1390";

            TestUtility.SetupMockResponseWithoutOAuth($"shows/{showId}", show);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetShowAsync(showId).Result;

            response.Should().NotBeNull();
            response.Title.Should().Be("Game of Thrones");
            response.Year.Should().Be(2011);
            response.Ids.Should().NotBeNull();
            response.Ids.Trakt.Should().Be(1390);
            response.Ids.Slug.Should().Be("game-of-thrones");
            response.Ids.Tvdb.Should().Be(121361);
            response.Ids.Imdb.Should().Be("tt0944947");
            response.Ids.Tmdb.Should().Be(1399);
            response.Ids.TvRage.Should().Be(24493);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowWithExtendedOption()
        {
            var show = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowSummaryFullAndImages.json");
            show.Should().NotBeNullOrEmpty();

            var showId = "1390";

            var extendedOption = new TraktExtendedOption
            {
                Full = true,
                Images = true
            };

            TestUtility.SetupMockResponseWithoutOAuth($"shows/{showId}?extended={extendedOption.ToString()}", show);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetShowAsync(showId, extendedOption).Result;

            response.Should().NotBeNull();
            response.Title.Should().Be("Game of Thrones");
            response.Year.Should().Be(2011);
            response.Airs.Should().NotBeNull();
            response.Airs.Day.Should().Be("Sunday");
            response.Airs.Time.Should().Be("21:00");
            response.Airs.TimeZoneId.Should().Be("America/New_York");
            response.AvailableTranslationLanguageCodes.Should().NotBeNull().And.HaveCount(4).And.Contain("en", "fr", "it", "de");
            response.Ids.Should().NotBeNull();
            response.Ids.Trakt.Should().Be(1390);
            response.Ids.Slug.Should().Be("game-of-thrones");
            response.Ids.Tvdb.Should().Be(121361);
            response.Ids.Imdb.Should().Be("tt0944947");
            response.Ids.Tmdb.Should().Be(1399);
            response.Ids.TvRage.Should().Be(24493);
            response.Images.Should().NotBeNull();
            response.Images.FanArt.Full.Should().Be("https://walter.trakt.us/images/shows/000/001/390/fanarts/original/76d5df8aed.jpg");
            response.Images.FanArt.Medium.Should().Be("https://walter.trakt.us/images/shows/000/001/390/fanarts/medium/76d5df8aed.jpg");
            response.Images.FanArt.Thumb.Should().Be("https://walter.trakt.us/images/shows/000/001/390/fanarts/thumb/76d5df8aed.jpg");
            response.Images.Poster.Full.Should().Be("https://walter.trakt.us/images/shows/000/001/390/posters/original/93df9cd612.jpg");
            response.Images.Poster.Medium.Should().Be("https://walter.trakt.us/images/shows/000/001/390/posters/medium/93df9cd612.jpg");
            response.Images.Poster.Thumb.Should().Be("https://walter.trakt.us/images/shows/000/001/390/posters/thumb/93df9cd612.jpg");
            response.Images.Logo.Full.Should().Be("https://walter.trakt.us/images/shows/000/001/390/logos/original/13b614ad43.png");
            response.Images.ClearArt.Full.Should().Be("https://walter.trakt.us/images/shows/000/001/390/cleararts/original/5cbde9e647.png");
            response.Images.Banner.Full.Should().Be("https://walter.trakt.us/images/shows/000/001/390/banners/original/9fefff703d.jpg");
            response.Images.Thumb.Full.Should().Be("https://walter.trakt.us/images/shows/000/001/390/thumbs/original/7beccbd5a1.jpg");
            response.Genres.Should().NotBeNull().And.HaveCount(5).And.Contain("drama", "fantasy", "science-fiction", "action", "adventure");
            response.Seasons.Should().BeNull();
            response.Overview.Should().Be("Seven noble families fight for control of the mythical land of Westeros. Friction between the houses leads to full-scale war. All while a very ancient evil awakens in the farthest north. Amidst the war, a neglected military order of misfits, the Night's Watch, is all that stands between the realms of men and the icy horrors beyond.");
            response.FirstAired.Should().Be(DateTime.Parse("2011-04-17T07:00:00Z").ToUniversalTime());
            response.Runtime.Should().Be(60);
            response.Certification.Should().Be("TV-MA");
            response.Network.Should().Be("HBO");
            response.CountryCode.Should().Be("us");
            response.UpdatedAt.Should().Be(DateTime.Parse("2016-04-06T10:39:11Z").ToUniversalTime());
            response.Trailer.Should().Be("http://youtube.com/watch?v=F9Bo89m2f6g");
            response.TrailerUri.Should().NotBeNull();
            response.Homepage.Should().Be("http://www.hbo.com/game-of-thrones");
            response.HomepageUri.Should().NotBeNull();
            response.Status.Should().Be(TraktShowStatus.ReturningSeries);
            response.Rating.Should().Be(9.38327f);
            response.Votes.Should().Be(44773);
            response.LanguageCode.Should().Be("en");
            response.AiredEpisodes.Should().Be(50);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowExceptions()
        {
            var showId = "1390";
            var uri = $"shows/{showId}";

            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.NotFound);

            Func<Task<TraktShow>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetShowAsync(showId);
            act.ShouldThrow<TraktShowNotFoundException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.BadRequest);
            act.ShouldThrow<TraktBadRequestException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.Forbidden);
            act.ShouldThrow<TraktForbiddenException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)412);
            act.ShouldThrow<TraktPreconditionFailedException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)429);
            act.ShouldThrow<TraktRateLimitException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.InternalServerError);
            act.ShouldThrow<TraktServerException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)503);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)504);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)520);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)521);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)522);
            act.ShouldThrow<TraktServerUnavailableException>();
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowArgumentExceptions()
        {
            var show = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowSummaryFullAndImages.json");
            show.Should().NotBeNullOrEmpty();

            var showId = "1390";

            TestUtility.SetupMockResponseWithoutOAuth($"shows/{showId}", show);

            Func<Task<TraktShow>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetShowAsync(null);
            act.ShouldThrow<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetShowAsync(string.Empty);
            act.ShouldThrow<ArgumentException>();
        }

        #endregion

        // -----------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------

        #region MultipleShows

        [TestMethod]
        public void TestTraktShowsModuleGetShowsArgumentExceptions()
        {
            Func<Task<TraktListResult<TraktShow>>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetShowsAsync(new string[] { null });
            act.ShouldThrow<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetShowsAsync(new string[] { string.Empty });
            act.ShouldThrow<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetShowsAsync(new string[] { });
            act.ShouldNotThrow();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetShowsAsync(null);
            act.ShouldNotThrow();
        }

        #endregion

        // -----------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------

        #region ShowAliases

        [TestMethod]
        public void TestTraktShowsModuleGetShowAliases()
        {
            var showAliases = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowAliases.json");
            showAliases.Should().NotBeNullOrEmpty();

            var showId = "1390";

            TestUtility.SetupMockResponseWithoutOAuth($"shows/{showId}/aliases", showAliases);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetShowAliasesAsync(showId).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(8);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowAliasesExceptions()
        {
            var showId = "1390";
            var uri = $"shows/{showId}/aliases";

            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.NotFound);

            Func<Task<TraktListResult<TraktShowAlias>>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetShowAliasesAsync(showId);
            act.ShouldThrow<TraktShowNotFoundException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.BadRequest);
            act.ShouldThrow<TraktBadRequestException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.Forbidden);
            act.ShouldThrow<TraktForbiddenException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)412);
            act.ShouldThrow<TraktPreconditionFailedException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)429);
            act.ShouldThrow<TraktRateLimitException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.InternalServerError);
            act.ShouldThrow<TraktServerException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)503);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)504);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)520);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)521);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)522);
            act.ShouldThrow<TraktServerUnavailableException>();
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowAliasesArgumentExceptions()
        {
            var showAliases = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowAliases.json");
            showAliases.Should().NotBeNullOrEmpty();

            var showId = "1390";

            TestUtility.SetupMockResponseWithoutOAuth($"shows/{showId}/aliases", showAliases);

            Func<Task<TraktListResult<TraktShowAlias>>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetShowAliasesAsync(null);
            act.ShouldThrow<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetShowAliasesAsync(string.Empty);
            act.ShouldThrow<ArgumentException>();
        }

        #endregion

        // -----------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------

        #region ShowTranslations

        [TestMethod]
        public void TestTraktShowsModuleGetShowTranslations()
        {
            var showTranslations = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowTranslations.json");
            showTranslations.Should().NotBeNullOrEmpty();

            var showId = "1390";

            TestUtility.SetupMockResponseWithoutOAuth($"shows/{showId}/translations", showTranslations);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetShowTranslationsAsync(showId).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(4);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowTranslationsExceptions()
        {
            var showId = "1390";
            var uri = $"shows/{showId}/translations";

            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.NotFound);

            Func<Task<TraktListResult<TraktShowTranslation>>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetShowTranslationsAsync(showId);
            act.ShouldThrow<TraktShowNotFoundException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.BadRequest);
            act.ShouldThrow<TraktBadRequestException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.Forbidden);
            act.ShouldThrow<TraktForbiddenException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)412);
            act.ShouldThrow<TraktPreconditionFailedException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)429);
            act.ShouldThrow<TraktRateLimitException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.InternalServerError);
            act.ShouldThrow<TraktServerException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)503);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)504);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)520);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)521);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)522);
            act.ShouldThrow<TraktServerUnavailableException>();
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowTranslationsArgumentExceptions()
        {
            var showTranslations = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowTranslations.json");
            showTranslations.Should().NotBeNullOrEmpty();

            var showId = "1390";

            TestUtility.SetupMockResponseWithoutOAuth($"shows/{showId}/translations", showTranslations);

            Func<Task<TraktListResult<TraktShowTranslation>>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetShowTranslationsAsync(null);
            act.ShouldThrow<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetShowTranslationsAsync(string.Empty);
            act.ShouldThrow<ArgumentException>();
        }

        #endregion

        // -----------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------

        #region ShowSingleTranslation

        [TestMethod]
        public void TestTraktShowsModuleGetShowSingleTranslation()
        {
            var showTranslation = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowSingleTranslation.json");
            showTranslation.Should().NotBeNullOrEmpty();

            var showId = "1390";
            var languageCode = "de";

            TestUtility.SetupMockResponseWithoutOAuth($"shows/{showId}/translations/{languageCode}", showTranslation);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetShowSingleTranslationAsync(showId, languageCode).Result;

            response.Should().NotBeNull();
            response.Title.Should().Be("Game of Thrones");
            response.Overview.Should().Be("Die Handlung ist in einer fiktiven Welt angesiedelt und spielt auf den Kontinenten Westeros (den Sieben Königreichen sowie im Gebiet der „Mauer“ und jenseits davon im Norden) und Essos. In dieser Welt ist die Länge der Sommer und Winter unvorhersehbar und variabel; eine Jahreszeit kann Jahre oder sogar Jahrzehnte dauern. Der Handlungsort auf dem Kontinent Westeros in den Sieben Königreichen ähnelt dabei stark dem mittelalterlichen Europa. Die Geschichte spielt am Ende eines langen Sommers und wird in drei Handlungssträngen weitgehend parallel erzählt. In den Sieben Königreichen bauen sich zwischen den mächtigsten Adelshäusern des Reiches Spannungen auf, die schließlich zum offenen Thronkampf führen. Gleichzeitig droht der Wintereinbruch und es zeichnet sich eine Gefahr durch eine fremde Rasse im hohen Norden von Westeros ab. Der dritte Handlungsstrang spielt auf dem Kontinent Essos, wo Daenerys Targaryen, Mitglied der vor Jahren abgesetzten Königsfamilie, bestrebt ist, wieder an die Macht zu gelangen. Die komplexe Handlung umfasst zahlreiche Figuren und thematisiert unter anderem Politik und Machtkämpfe, Gesellschaftsverhältnisse und Religion.");
            response.LanguageCode.Should().Be("de");
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowSingleTranslationExceptions()
        {
            var showId = "1390";
            var languageCode = "de";
            var uri = $"shows/{showId}/translations/{languageCode}";

            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.NotFound);

            Func<Task<TraktShowTranslation>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetShowSingleTranslationAsync(showId, languageCode);
            act.ShouldThrow<TraktShowNotFoundException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.BadRequest);
            act.ShouldThrow<TraktBadRequestException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.Forbidden);
            act.ShouldThrow<TraktForbiddenException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)412);
            act.ShouldThrow<TraktPreconditionFailedException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)429);
            act.ShouldThrow<TraktRateLimitException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.InternalServerError);
            act.ShouldThrow<TraktServerException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)503);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)504);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)520);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)521);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)522);
            act.ShouldThrow<TraktServerUnavailableException>();
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowSingleTranslationArgumentExceptions()
        {
            var showTranslation = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowSingleTranslation.json");
            showTranslation.Should().NotBeNullOrEmpty();

            var showId = "1390";
            var languageCode = "de";

            TestUtility.SetupMockResponseWithoutOAuth($"shows/{showId}/translations/{languageCode}", showTranslation);

            Func<Task<TraktShowTranslation>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetShowSingleTranslationAsync(null, languageCode);
            act.ShouldThrow<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetShowSingleTranslationAsync(string.Empty, languageCode);
            act.ShouldThrow<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetShowSingleTranslationAsync(showId, null);
            act.ShouldThrow<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetShowSingleTranslationAsync(showId, string.Empty);
            act.ShouldThrow<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetShowSingleTranslationAsync(showId, "d");
            act.ShouldThrow<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetShowSingleTranslationAsync(showId, "deu");
            act.ShouldThrow<ArgumentException>();
        }

        #endregion

        // -----------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------

        #region ShowComments

        [TestMethod]
        public void TestTraktShowsModuleGetShowComments()
        {
            var showComments = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowComments.json");
            showComments.Should().NotBeNullOrEmpty();

            var showId = "1390";
            var itemCount = 2;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/{showId}/comments",
                                                                showComments, 1, 10, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetShowCommentsAsync(showId).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(10);
            response.Page.Should().Be(1);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowCommentsWithSortOrder()
        {
            var showComments = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowComments.json");
            showComments.Should().NotBeNullOrEmpty();

            var showId = "1390";
            var itemCount = 2;
            var sortOrder = TraktCommentSortOrder.Likes;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/{showId}/comments/{sortOrder.AsString()}",
                                                                showComments, 1, 10, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetShowCommentsAsync(showId, sortOrder).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(10);
            response.Page.Should().Be(1);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowCommentsWithPage()
        {
            var showComments = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowComments.json");
            showComments.Should().NotBeNullOrEmpty();

            var showId = "1390";
            var itemCount = 2;
            var page = 2;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/{showId}/comments?page={page}",
                                                                showComments, page, 10, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetShowCommentsAsync(showId, null, page).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(10);
            response.Page.Should().Be(page);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowCommentsWithSortOrderAndPage()
        {
            var showComments = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowComments.json");
            showComments.Should().NotBeNullOrEmpty();

            var showId = "1390";
            var itemCount = 2;
            var page = 2;
            var sortOrder = TraktCommentSortOrder.Likes;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/{showId}/comments/{sortOrder.AsString()}?page={page}",
                                                                showComments, page, 10, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetShowCommentsAsync(showId, sortOrder, page).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(10);
            response.Page.Should().Be(page);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowCommentsWithLimit()
        {
            var showComments = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowComments.json");
            showComments.Should().NotBeNullOrEmpty();

            var showId = "1390";
            var itemCount = 2;
            var limit = 4;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/{showId}/comments?limit={limit}",
                                                                showComments, 1, limit, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetShowCommentsAsync(showId, null, null, limit).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(limit);
            response.Page.Should().Be(1);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowCommentsWithSortOrderAndLimit()
        {
            var showComments = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowComments.json");
            showComments.Should().NotBeNullOrEmpty();

            var showId = "1390";
            var itemCount = 2;
            var limit = 4;
            var sortOrder = TraktCommentSortOrder.Likes;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/{showId}/comments/{sortOrder.AsString()}?limit={limit}",
                                                                showComments, 1, limit, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetShowCommentsAsync(showId, sortOrder, null, limit).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(limit);
            response.Page.Should().Be(1);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowCommentsWithPageAndLimit()
        {
            var showComments = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowComments.json");
            showComments.Should().NotBeNullOrEmpty();

            var showId = "1390";
            var itemCount = 2;
            var page = 2;
            var limit = 4;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/{showId}/comments?page={page}&limit={limit}",
                                                                showComments, page, limit, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetShowCommentsAsync(showId, null, page, limit).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(limit);
            response.Page.Should().Be(page);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowCommentsComplete()
        {
            var showComments = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowComments.json");
            showComments.Should().NotBeNullOrEmpty();

            var showId = "1390";
            var itemCount = 2;
            var page = 2;
            var limit = 4;
            var sortOrder = TraktCommentSortOrder.Likes;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/{showId}/comments/{sortOrder.AsString()}?page={page}&limit={limit}",
                                                                showComments, page, limit, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetShowCommentsAsync(showId, sortOrder, page, limit).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(limit);
            response.Page.Should().Be(page);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowCommentsExceptions()
        {
            var showId = "1390";
            var uri = $"shows/{showId}/comments";

            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.NotFound);

            Func<Task<TraktPaginationListResult<TraktShowComment>>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetShowCommentsAsync(showId);
            act.ShouldThrow<TraktShowNotFoundException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.BadRequest);
            act.ShouldThrow<TraktBadRequestException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.Forbidden);
            act.ShouldThrow<TraktForbiddenException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)412);
            act.ShouldThrow<TraktPreconditionFailedException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)429);
            act.ShouldThrow<TraktRateLimitException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.InternalServerError);
            act.ShouldThrow<TraktServerException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)503);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)504);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)520);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)521);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)522);
            act.ShouldThrow<TraktServerUnavailableException>();
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowCommentsArgumentExceptions()
        {
            var showComments = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowComments.json");
            showComments.Should().NotBeNullOrEmpty();

            var showId = "1390";

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/{showId}/comments", showComments);

            Func<Task<TraktPaginationListResult<TraktShowComment>>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetShowCommentsAsync(null);
            act.ShouldThrow<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetShowCommentsAsync(string.Empty);
            act.ShouldThrow<ArgumentException>();
        }

        #endregion

        // -----------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------

        #region ShowPeople

        [TestMethod]
        public void TestTraktShowsModuleGetShowPeople()
        {
            var showPeople = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowPeople.json");
            showPeople.Should().NotBeNullOrEmpty();

            var showId = "1390";

            TestUtility.SetupMockResponseWithoutOAuth($"shows/{showId}/people", showPeople);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetShowPeopleAsync(showId).Result;

            response.Should().NotBeNull();
            response.Cast.Should().NotBeNull().And.HaveCount(3);
            response.Crew.Should().NotBeNull();
            response.Crew.Production.Should().NotBeNull().And.HaveCount(2);
            response.Crew.Art.Should().NotBeNull().And.HaveCount(1);
            response.Crew.Crew.Should().NotBeNull().And.HaveCount(1);
            response.Crew.CostumeAndMakeup.Should().BeNull();
            response.Crew.Directing.Should().BeNull();
            response.Crew.Writing.Should().NotBeNull().And.HaveCount(3);
            response.Crew.Sound.Should().NotBeNull().And.HaveCount(1);
            response.Crew.Camera.Should().BeNull();
            response.Crew.Lighting.Should().BeNull();
            response.Crew.VisualEffects.Should().BeNull();
            response.Crew.Editing.Should().BeNull();
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowPeopleWithExtendedOption()
        {
            var showPeople = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowPeople.json");
            showPeople.Should().NotBeNullOrEmpty();

            var showId = "1390";

            var extendedOption = new TraktExtendedOption
            {
                Full = true,
                Images = true
            };

            TestUtility.SetupMockResponseWithoutOAuth($"shows/{showId}/people?extended={extendedOption.ToString()}", showPeople);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetShowPeopleAsync(showId, extendedOption).Result;

            response.Should().NotBeNull();
            response.Cast.Should().NotBeNull().And.HaveCount(3);
            response.Crew.Should().NotBeNull();
            response.Crew.Production.Should().NotBeNull().And.HaveCount(2);
            response.Crew.Art.Should().NotBeNull().And.HaveCount(1);
            response.Crew.Crew.Should().NotBeNull().And.HaveCount(1);
            response.Crew.CostumeAndMakeup.Should().BeNull();
            response.Crew.Directing.Should().BeNull();
            response.Crew.Writing.Should().NotBeNull().And.HaveCount(3);
            response.Crew.Sound.Should().NotBeNull().And.HaveCount(1);
            response.Crew.Camera.Should().BeNull();
            response.Crew.Lighting.Should().BeNull();
            response.Crew.VisualEffects.Should().BeNull();
            response.Crew.Editing.Should().BeNull();
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowPeopleExceptions()
        {
            var showId = "1390";
            var uri = $"shows/{showId}/people";

            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.NotFound);

            Func<Task<TraktShowPeople>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetShowPeopleAsync(showId);
            act.ShouldThrow<TraktShowNotFoundException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.BadRequest);
            act.ShouldThrow<TraktBadRequestException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.Forbidden);
            act.ShouldThrow<TraktForbiddenException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)412);
            act.ShouldThrow<TraktPreconditionFailedException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)429);
            act.ShouldThrow<TraktRateLimitException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.InternalServerError);
            act.ShouldThrow<TraktServerException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)503);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)504);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)520);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)521);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)522);
            act.ShouldThrow<TraktServerUnavailableException>();
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowPeopleArgumentExceptions()
        {
            var showPeople = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowPeople.json");
            showPeople.Should().NotBeNullOrEmpty();

            var showId = "1390";

            TestUtility.SetupMockResponseWithoutOAuth($"shows/{showId}/people", showPeople);

            Func<Task<TraktShowPeople>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetShowPeopleAsync(null);
            act.ShouldThrow<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetShowPeopleAsync(string.Empty);
            act.ShouldThrow<ArgumentException>();
        }

        #endregion

        // -----------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------

        #region ShowRatings

        [TestMethod]
        public void TestTraktShowsModuleGetShowRatings()
        {
            var showRatings = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowRatings.json");
            showRatings.Should().NotBeNullOrEmpty();

            var showId = "1390";

            TestUtility.SetupMockResponseWithoutOAuth($"shows/{showId}/ratings", showRatings);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetShowRatingsAsync(showId).Result;

            response.Should().NotBeNull();
            response.Rating.Should().Be(9.38231f);
            response.Votes.Should().Be(44590);

            var distribution = new Dictionary<string, int>()
            {
                { "1",  258 }, { "2", 57 }, { "3", 59 }, { "4", 116 }, { "5", 262 },
                { "6",  448 }, { "7", 1427 }, { "8", 3893 }, { "9", 8467 }, { "10", 29590 }
            };

            response.Distribution.Should().HaveCount(10).And.Contain(distribution);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowRatingsExceptions()
        {
            var showId = "1390";
            var uri = $"shows/{showId}/ratings";

            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.NotFound);

            Func<Task<TraktShowRating>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetShowRatingsAsync(showId);
            act.ShouldThrow<TraktShowNotFoundException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.BadRequest);
            act.ShouldThrow<TraktBadRequestException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.Forbidden);
            act.ShouldThrow<TraktForbiddenException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)412);
            act.ShouldThrow<TraktPreconditionFailedException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)429);
            act.ShouldThrow<TraktRateLimitException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.InternalServerError);
            act.ShouldThrow<TraktServerException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)503);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)504);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)520);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)521);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)522);
            act.ShouldThrow<TraktServerUnavailableException>();
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowRatingsArgumentExceptions()
        {
            var showRatings = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowRatings.json");
            showRatings.Should().NotBeNullOrEmpty();

            var showId = "1390";

            TestUtility.SetupMockResponseWithoutOAuth($"shows/{showId}/ratings", showRatings);

            Func<Task<TraktShowRating>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetShowRatingsAsync(null);
            act.ShouldThrow<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetShowRatingsAsync(string.Empty);
            act.ShouldThrow<ArgumentException>();
        }

        #endregion

        // -----------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------

        #region ShowRelatedShows

        [TestMethod]
        public void TestTraktShowsModuleGetShowRelatedShows()
        {
            var showRelatedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowRelatedShowsFullAndImages.json");
            showRelatedShows.Should().NotBeNullOrEmpty();

            var showId = "1390";
            var itemCount = 2;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/{showId}/related", showRelatedShows, 1, 10, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetShowRelatedShowsAsync(showId).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(10);
            response.Page.Should().Be(1);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowRelatedShowsWithExtendedOption()
        {
            var showRelatedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowRelatedShowsFullAndImages.json");
            showRelatedShows.Should().NotBeNullOrEmpty();

            var showId = "1390";
            var itemCount = 2;

            var extendedOption = new TraktExtendedOption
            {
                Full = true,
                Images = true
            };

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/{showId}/related?extended={extendedOption.ToString()}",
                                                                showRelatedShows, 1, 10, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetShowRelatedShowsAsync(showId, extendedOption).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(10);
            response.Page.Should().Be(1);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowRelatedShowsWithPage()
        {
            var showRelatedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowRelatedShowsFullAndImages.json");
            showRelatedShows.Should().NotBeNullOrEmpty();

            var showId = "1390";
            var itemCount = 2;
            var page = 2;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/{showId}/related?page={page}", showRelatedShows, page, 10, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetShowRelatedShowsAsync(showId, null, page).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(10);
            response.Page.Should().Be(page);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowRelatedShowsWithExtendedOptionAndPage()
        {
            var showRelatedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowRelatedShowsFullAndImages.json");
            showRelatedShows.Should().NotBeNullOrEmpty();

            var showId = "1390";
            var itemCount = 2;
            var page = 2;

            var extendedOption = new TraktExtendedOption
            {
                Full = true,
                Images = true
            };

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/{showId}/related?extended={extendedOption.ToString()}&page={page}",
                                                                showRelatedShows, page, 10, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetShowRelatedShowsAsync(showId, extendedOption, page).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(10);
            response.Page.Should().Be(page);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowRelatedShowsWithLimit()
        {
            var showRelatedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowRelatedShowsFullAndImages.json");
            showRelatedShows.Should().NotBeNullOrEmpty();

            var showId = "1390";
            var itemCount = 2;
            var limit = 4;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/{showId}/related?limit={limit}", showRelatedShows, 1, limit, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetShowRelatedShowsAsync(showId, null, null, limit).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(limit);
            response.Page.Should().Be(1);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowRelatedShowsWithExtendedOptionAndLimit()
        {
            var showRelatedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowRelatedShowsFullAndImages.json");
            showRelatedShows.Should().NotBeNullOrEmpty();

            var showId = "1390";
            var itemCount = 2;
            var limit = 4;

            var extendedOption = new TraktExtendedOption
            {
                Full = true,
                Images = true
            };

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/{showId}/related?extended={extendedOption.ToString()}&limit={limit}",
                                                                showRelatedShows, 1, limit, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetShowRelatedShowsAsync(showId, extendedOption, null, limit).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(limit);
            response.Page.Should().Be(1);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowRelatedShowsWithPageAndLimit()
        {
            var showRelatedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowRelatedShowsFullAndImages.json");
            showRelatedShows.Should().NotBeNullOrEmpty();

            var showId = "1390";
            var itemCount = 2;
            var page = 2;
            var limit = 4;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/{showId}/related?page={page}&limit={limit}",
                                                                showRelatedShows, page, limit, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetShowRelatedShowsAsync(showId, null, page, limit).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(limit);
            response.Page.Should().Be(page);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowRelatedShowsComplete()
        {
            var showRelatedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowRelatedShowsFullAndImages.json");
            showRelatedShows.Should().NotBeNullOrEmpty();

            var showId = "1390";
            var itemCount = 2;
            var page = 2;
            var limit = 4;

            var extendedOption = new TraktExtendedOption
            {
                Full = true,
                Images = true
            };

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/{showId}/related?extended={extendedOption.ToString()}&page={page}&limit={limit}",
                                                                showRelatedShows, page, limit, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetShowRelatedShowsAsync(showId, extendedOption, page, limit).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(limit);
            response.Page.Should().Be(page);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowRelatedShowsExceptions()
        {
            var showId = "1390";
            var uri = $"shows/{showId}/related";

            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.NotFound);

            Func<Task<TraktPaginationListResult<TraktShow>>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetShowRelatedShowsAsync(showId);
            act.ShouldThrow<TraktShowNotFoundException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.BadRequest);
            act.ShouldThrow<TraktBadRequestException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.Forbidden);
            act.ShouldThrow<TraktForbiddenException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)412);
            act.ShouldThrow<TraktPreconditionFailedException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)429);
            act.ShouldThrow<TraktRateLimitException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.InternalServerError);
            act.ShouldThrow<TraktServerException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)503);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)504);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)520);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)521);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)522);
            act.ShouldThrow<TraktServerUnavailableException>();
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowRelatedShowsArgumentExceptions()
        {
            var showRelatedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowRelatedShowsFullAndImages.json");
            showRelatedShows.Should().NotBeNullOrEmpty();

            var showId = "1390";

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/{showId}/related", showRelatedShows);

            Func<Task<TraktPaginationListResult<TraktShow>>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetShowRelatedShowsAsync(null);
            act.ShouldThrow<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetShowRelatedShowsAsync(string.Empty);
            act.ShouldThrow<ArgumentException>();
        }

        #endregion

        // -----------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------

        #region ShowStatistics

        [TestMethod]
        public void TestTraktShowsModuleGetShowStatistics()
        {
            var showStatistics = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowStatistics.json");
            showStatistics.Should().NotBeNullOrEmpty();

            var showId = "1390";

            TestUtility.SetupMockResponseWithoutOAuth($"shows/{showId}/stats", showStatistics);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetShowStatisticsAsync(showId).Result;

            response.Should().NotBeNull();
            response.Watchers.Should().Be(265955);
            response.Plays.Should().Be(12491168);
            response.Collectors.Should().Be(106028);
            response.CollectedEpisodes.Should().Be(4092901);
            response.Comments.Should().Be(233);
            response.Lists.Should().Be(103943);
            response.Votes.Should().Be(44590);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowStatisticsExceptions()
        {
            var showId = "1390";
            var uri = $"shows/{showId}/stats";

            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.NotFound);

            Func<Task<TraktShowStatistics>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetShowStatisticsAsync(showId);
            act.ShouldThrow<TraktShowNotFoundException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.BadRequest);
            act.ShouldThrow<TraktBadRequestException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.Forbidden);
            act.ShouldThrow<TraktForbiddenException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)412);
            act.ShouldThrow<TraktPreconditionFailedException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)429);
            act.ShouldThrow<TraktRateLimitException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.InternalServerError);
            act.ShouldThrow<TraktServerException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)503);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)504);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)520);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)521);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)522);
            act.ShouldThrow<TraktServerUnavailableException>();
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowStatisticsArgumentExceptions()
        {
            var showStatistics = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowStatistics.json");
            showStatistics.Should().NotBeNullOrEmpty();

            var showId = "1390";

            TestUtility.SetupMockResponseWithoutOAuth($"shows/{showId}/stats", showStatistics);

            Func<Task<TraktShowStatistics>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetShowStatisticsAsync(null);
            act.ShouldThrow<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetShowStatisticsAsync(string.Empty);
            act.ShouldThrow<ArgumentException>();
        }

        #endregion

        // -----------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------

        #region ShowWatchingUsers

        [TestMethod]
        public void TestTraktShowsModuleGetShowWatchingUsers()
        {
            var showWatchingUsers = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowWatchingUsers.json");
            showWatchingUsers.Should().NotBeNullOrEmpty();

            var showId = "1390";

            TestUtility.SetupMockResponseWithoutOAuth($"shows/{showId}/watching", showWatchingUsers);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetShowWatchingUsersAsync(showId).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(3);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowWatchingUsersWithExtendedOption()
        {
            var showWatchingUsers = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowWatchingUsers.json");
            showWatchingUsers.Should().NotBeNullOrEmpty();

            var showId = "1390";

            var extendedOption = new TraktExtendedOption
            {
                Full = true,
                Images = true
            };

            TestUtility.SetupMockResponseWithoutOAuth($"shows/{showId}/watching?extended={extendedOption.ToString()}", showWatchingUsers);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetShowWatchingUsersAsync(showId, extendedOption).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(3);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowWatchingUsersExceptions()
        {
            var showId = "1390";
            var uri = $"shows/{showId}/watching";

            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.NotFound);

            Func<Task<TraktListResult<TraktShowWatchingUser>>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetShowWatchingUsersAsync(showId);
            act.ShouldThrow<TraktShowNotFoundException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.BadRequest);
            act.ShouldThrow<TraktBadRequestException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.Forbidden);
            act.ShouldThrow<TraktForbiddenException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)412);
            act.ShouldThrow<TraktPreconditionFailedException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)429);
            act.ShouldThrow<TraktRateLimitException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.InternalServerError);
            act.ShouldThrow<TraktServerException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)503);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)504);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)520);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)521);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)522);
            act.ShouldThrow<TraktServerUnavailableException>();
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowWatchingUsersArgumentExceptions()
        {
            var showWatchingUsers = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowWatchingUsers.json");
            showWatchingUsers.Should().NotBeNullOrEmpty();

            var showId = "1390";

            TestUtility.SetupMockResponseWithoutOAuth($"shows/{showId}/watching", showWatchingUsers);

            Func<Task<TraktListResult<TraktShowWatchingUser>>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetShowWatchingUsersAsync(null);
            act.ShouldThrow<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetShowWatchingUsersAsync(string.Empty);
            act.ShouldThrow<ArgumentException>();
        }

        #endregion

        // -----------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------

        #region ShowCollectionProgress

        [TestMethod]
        public void TestTraktShowsModuleGetShowCollectionProgress()
        {
            var showCollectionProgress = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowCollectionProgressComplete.json");
            showCollectionProgress.Should().NotBeNullOrEmpty();

            var showId = "1390";

            TestUtility.SetupMockResponseWithOAuth($"shows/{showId}/progress/collection", showCollectionProgress);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetShowCollectionProgressAsync(showId).Result;

            response.Should().NotBeNull();

            response.Aired.Should().Be(6);
            response.Completed.Should().Be(6);
            response.LastCollectedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());
            response.Seasons.Should().NotBeNull();
            response.Seasons.Should().HaveCount(1);

            var seasons = response.Seasons.ToArray();

            seasons[0].Number.Should().Be(1);
            seasons[0].Aired.Should().Be(6);
            seasons[0].Completed.Should().Be(6);
            seasons[0].Episodes.Should().NotBeNull();
            seasons[0].Episodes.Should().HaveCount(6);

            var episodes = seasons[0].Episodes.ToArray();

            episodes[0].Number.Should().Be(1);
            episodes[0].Completed.Should().BeTrue();
            episodes[0].CollectedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            episodes[1].Number.Should().Be(2);
            episodes[1].Completed.Should().BeTrue();
            episodes[1].CollectedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            episodes[2].Number.Should().Be(3);
            episodes[2].Completed.Should().BeTrue();
            episodes[2].CollectedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            episodes[3].Number.Should().Be(4);
            episodes[3].Completed.Should().BeTrue();
            episodes[3].CollectedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            episodes[4].Number.Should().Be(5);
            episodes[4].Completed.Should().BeTrue();
            episodes[4].CollectedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            episodes[5].Number.Should().Be(6);
            episodes[5].Completed.Should().BeTrue();
            episodes[5].CollectedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            response.HiddenSeasons.Should().NotBeNull();
            response.HiddenSeasons.Should().HaveCount(1);

            var hiddenSeasons = response.HiddenSeasons.ToArray();

            hiddenSeasons[0].Number.Should().Be(2);
            hiddenSeasons[0].Ids.Should().NotBeNull();
            hiddenSeasons[0].Ids.Trakt.Should().Be(3051);
            hiddenSeasons[0].Ids.Tvdb.Should().Be(498968);
            hiddenSeasons[0].Ids.Tmdb.Should().Be(53334);
            hiddenSeasons[0].Ids.TvRage.Should().NotHaveValue();

            response.NextEpisode.Should().BeNull();
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowCollectionProgressWithHidden()
        {
            var showCollectionProgress = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowCollectionProgressComplete.json");
            showCollectionProgress.Should().NotBeNullOrEmpty();

            var showId = "1390";
            var hidden = true;

            TestUtility.SetupMockResponseWithOAuth($"shows/{showId}/progress/collection?hidden={hidden.ToString().ToLower()}",
                                                   showCollectionProgress);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetShowCollectionProgressAsync(showId, hidden).Result;

            response.Should().NotBeNull();

            response.Aired.Should().Be(6);
            response.Completed.Should().Be(6);
            response.LastCollectedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());
            response.Seasons.Should().NotBeNull();
            response.Seasons.Should().HaveCount(1);

            var seasons = response.Seasons.ToArray();

            seasons[0].Number.Should().Be(1);
            seasons[0].Aired.Should().Be(6);
            seasons[0].Completed.Should().Be(6);
            seasons[0].Episodes.Should().NotBeNull();
            seasons[0].Episodes.Should().HaveCount(6);

            var episodes = seasons[0].Episodes.ToArray();

            episodes[0].Number.Should().Be(1);
            episodes[0].Completed.Should().BeTrue();
            episodes[0].CollectedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            episodes[1].Number.Should().Be(2);
            episodes[1].Completed.Should().BeTrue();
            episodes[1].CollectedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            episodes[2].Number.Should().Be(3);
            episodes[2].Completed.Should().BeTrue();
            episodes[2].CollectedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            episodes[3].Number.Should().Be(4);
            episodes[3].Completed.Should().BeTrue();
            episodes[3].CollectedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            episodes[4].Number.Should().Be(5);
            episodes[4].Completed.Should().BeTrue();
            episodes[4].CollectedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            episodes[5].Number.Should().Be(6);
            episodes[5].Completed.Should().BeTrue();
            episodes[5].CollectedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            response.HiddenSeasons.Should().NotBeNull();
            response.HiddenSeasons.Should().HaveCount(1);

            var hiddenSeasons = response.HiddenSeasons.ToArray();

            hiddenSeasons[0].Number.Should().Be(2);
            hiddenSeasons[0].Ids.Should().NotBeNull();
            hiddenSeasons[0].Ids.Trakt.Should().Be(3051);
            hiddenSeasons[0].Ids.Tvdb.Should().Be(498968);
            hiddenSeasons[0].Ids.Tmdb.Should().Be(53334);
            hiddenSeasons[0].Ids.TvRage.Should().NotHaveValue();

            response.NextEpisode.Should().BeNull();
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowCollectionProgressWithSpecials()
        {
            var showCollectionProgress = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowCollectionProgressComplete.json");
            showCollectionProgress.Should().NotBeNullOrEmpty();

            var showId = "1390";
            var specials = true;

            TestUtility.SetupMockResponseWithOAuth($"shows/{showId}/progress/collection?specials={specials.ToString().ToLower()}",
                                                   showCollectionProgress);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetShowCollectionProgressAsync(showId, null, specials).Result;

            response.Should().NotBeNull();

            response.Aired.Should().Be(6);
            response.Completed.Should().Be(6);
            response.LastCollectedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());
            response.Seasons.Should().NotBeNull();
            response.Seasons.Should().HaveCount(1);

            var seasons = response.Seasons.ToArray();

            seasons[0].Number.Should().Be(1);
            seasons[0].Aired.Should().Be(6);
            seasons[0].Completed.Should().Be(6);
            seasons[0].Episodes.Should().NotBeNull();
            seasons[0].Episodes.Should().HaveCount(6);

            var episodes = seasons[0].Episodes.ToArray();

            episodes[0].Number.Should().Be(1);
            episodes[0].Completed.Should().BeTrue();
            episodes[0].CollectedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            episodes[1].Number.Should().Be(2);
            episodes[1].Completed.Should().BeTrue();
            episodes[1].CollectedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            episodes[2].Number.Should().Be(3);
            episodes[2].Completed.Should().BeTrue();
            episodes[2].CollectedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            episodes[3].Number.Should().Be(4);
            episodes[3].Completed.Should().BeTrue();
            episodes[3].CollectedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            episodes[4].Number.Should().Be(5);
            episodes[4].Completed.Should().BeTrue();
            episodes[4].CollectedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            episodes[5].Number.Should().Be(6);
            episodes[5].Completed.Should().BeTrue();
            episodes[5].CollectedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            response.HiddenSeasons.Should().NotBeNull();
            response.HiddenSeasons.Should().HaveCount(1);

            var hiddenSeasons = response.HiddenSeasons.ToArray();

            hiddenSeasons[0].Number.Should().Be(2);
            hiddenSeasons[0].Ids.Should().NotBeNull();
            hiddenSeasons[0].Ids.Trakt.Should().Be(3051);
            hiddenSeasons[0].Ids.Tvdb.Should().Be(498968);
            hiddenSeasons[0].Ids.Tmdb.Should().Be(53334);
            hiddenSeasons[0].Ids.TvRage.Should().NotHaveValue();

            response.NextEpisode.Should().BeNull();
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowCollectionProgressComplete()
        {
            var showCollectionProgress = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowCollectionProgressComplete.json");
            showCollectionProgress.Should().NotBeNullOrEmpty();

            var showId = "1390";
            var hidden = true;
            var specials = true;

            TestUtility.SetupMockResponseWithOAuth(
                $"shows/{showId}/progress/collection?hidden={hidden.ToString().ToLower()}&specials={specials.ToString().ToLower()}",
                showCollectionProgress);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetShowCollectionProgressAsync(showId, hidden, specials).Result;

            response.Should().NotBeNull();

            response.Aired.Should().Be(6);
            response.Completed.Should().Be(6);
            response.LastCollectedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());
            response.Seasons.Should().NotBeNull();
            response.Seasons.Should().HaveCount(1);

            var seasons = response.Seasons.ToArray();

            seasons[0].Number.Should().Be(1);
            seasons[0].Aired.Should().Be(6);
            seasons[0].Completed.Should().Be(6);
            seasons[0].Episodes.Should().NotBeNull();
            seasons[0].Episodes.Should().HaveCount(6);

            var episodes = seasons[0].Episodes.ToArray();

            episodes[0].Number.Should().Be(1);
            episodes[0].Completed.Should().BeTrue();
            episodes[0].CollectedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            episodes[1].Number.Should().Be(2);
            episodes[1].Completed.Should().BeTrue();
            episodes[1].CollectedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            episodes[2].Number.Should().Be(3);
            episodes[2].Completed.Should().BeTrue();
            episodes[2].CollectedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            episodes[3].Number.Should().Be(4);
            episodes[3].Completed.Should().BeTrue();
            episodes[3].CollectedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            episodes[4].Number.Should().Be(5);
            episodes[4].Completed.Should().BeTrue();
            episodes[4].CollectedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            episodes[5].Number.Should().Be(6);
            episodes[5].Completed.Should().BeTrue();
            episodes[5].CollectedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            response.HiddenSeasons.Should().NotBeNull();
            response.HiddenSeasons.Should().HaveCount(1);

            var hiddenSeasons = response.HiddenSeasons.ToArray();

            hiddenSeasons[0].Number.Should().Be(2);
            hiddenSeasons[0].Ids.Should().NotBeNull();
            hiddenSeasons[0].Ids.Trakt.Should().Be(3051);
            hiddenSeasons[0].Ids.Tvdb.Should().Be(498968);
            hiddenSeasons[0].Ids.Tmdb.Should().Be(53334);
            hiddenSeasons[0].Ids.TvRage.Should().NotHaveValue();

            response.NextEpisode.Should().BeNull();
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowCollectionProgressExceptions()
        {
            var showId = "1390";
            var uri = $"shows/{showId}/progress/collection";

            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.Unauthorized);

            Func<Task<TraktShowCollectionProgress>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetShowCollectionProgressAsync(showId);
            act.ShouldThrow<TraktAuthorizationException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithOAuth(uri, HttpStatusCode.NotFound);
            act.ShouldThrow<TraktShowNotFoundException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithOAuth(uri, HttpStatusCode.BadRequest);
            act.ShouldThrow<TraktBadRequestException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithOAuth(uri, HttpStatusCode.Forbidden);
            act.ShouldThrow<TraktForbiddenException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithOAuth(uri, (HttpStatusCode)412);
            act.ShouldThrow<TraktPreconditionFailedException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithOAuth(uri, (HttpStatusCode)429);
            act.ShouldThrow<TraktRateLimitException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithOAuth(uri, HttpStatusCode.InternalServerError);
            act.ShouldThrow<TraktServerException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithOAuth(uri, (HttpStatusCode)503);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithOAuth(uri, (HttpStatusCode)504);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithOAuth(uri, (HttpStatusCode)520);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithOAuth(uri, (HttpStatusCode)521);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithOAuth(uri, (HttpStatusCode)522);
            act.ShouldThrow<TraktServerUnavailableException>();
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowCollectionProgressArgumentExceptions()
        {
            var showCollectionProgress = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowCollectionProgressComplete.json");
            showCollectionProgress.Should().NotBeNullOrEmpty();

            var showId = "1390";

            TestUtility.SetupMockResponseWithOAuth($"shows/{showId}/progress/collection", showCollectionProgress);

            Func<Task<TraktShowCollectionProgress>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetShowCollectionProgressAsync(null);
            act.ShouldThrow<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetShowCollectionProgressAsync(string.Empty);
            act.ShouldThrow<ArgumentException>();
        }

        #endregion

        // -----------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------

        #region ShowWatchedProgress

        [TestMethod]
        public void TestTraktShowsModuleGetShowWatchedProgress()
        {
            var showWatchedProgress = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowWatchedProgressComplete.json");
            showWatchedProgress.Should().NotBeNullOrEmpty();

            var showId = "1390";

            TestUtility.SetupMockResponseWithOAuth($"shows/{showId}/progress/watched", showWatchedProgress);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetShowWatchedProgressAsync(showId).Result;

            response.Should().NotBeNull();

            response.Aired.Should().Be(6);
            response.Completed.Should().Be(6);
            response.LastWatchedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());
            response.Seasons.Should().NotBeNull();
            response.Seasons.Should().HaveCount(1);

            var seasons = response.Seasons.ToArray();

            seasons[0].Number.Should().Be(1);
            seasons[0].Aired.Should().Be(6);
            seasons[0].Completed.Should().Be(6);
            seasons[0].Episodes.Should().NotBeNull();
            seasons[0].Episodes.Should().HaveCount(6);

            var episodes = seasons[0].Episodes.ToArray();

            episodes[0].Number.Should().Be(1);
            episodes[0].Completed.Should().BeTrue();
            episodes[0].LastWatchedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            episodes[1].Number.Should().Be(2);
            episodes[1].Completed.Should().BeTrue();
            episodes[1].LastWatchedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            episodes[2].Number.Should().Be(3);
            episodes[2].Completed.Should().BeTrue();
            episodes[2].LastWatchedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            episodes[3].Number.Should().Be(4);
            episodes[3].Completed.Should().BeTrue();
            episodes[3].LastWatchedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            episodes[4].Number.Should().Be(5);
            episodes[4].Completed.Should().BeTrue();
            episodes[4].LastWatchedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            episodes[5].Number.Should().Be(6);
            episodes[5].Completed.Should().BeTrue();
            episodes[5].LastWatchedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            response.HiddenSeasons.Should().NotBeNull();
            response.HiddenSeasons.Should().HaveCount(1);

            var hiddenSeasons = response.HiddenSeasons.ToArray();

            hiddenSeasons[0].Number.Should().Be(2);
            hiddenSeasons[0].Ids.Should().NotBeNull();
            hiddenSeasons[0].Ids.Trakt.Should().Be(3051);
            hiddenSeasons[0].Ids.Tvdb.Should().Be(498968);
            hiddenSeasons[0].Ids.Tmdb.Should().Be(53334);
            hiddenSeasons[0].Ids.TvRage.Should().NotHaveValue();

            response.NextEpisode.Should().BeNull();
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowWatchedProgressWithHidden()
        {
            var showWatchedProgress = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowWatchedProgressComplete.json");
            showWatchedProgress.Should().NotBeNullOrEmpty();

            var showId = "1390";
            var hidden = true;

            TestUtility.SetupMockResponseWithOAuth($"shows/{showId}/progress/watched?hidden={hidden.ToString().ToLower()}",
                                                   showWatchedProgress);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetShowWatchedProgressAsync(showId, hidden).Result;

            response.Should().NotBeNull();

            response.Aired.Should().Be(6);
            response.Completed.Should().Be(6);
            response.LastWatchedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());
            response.Seasons.Should().NotBeNull();
            response.Seasons.Should().HaveCount(1);

            var seasons = response.Seasons.ToArray();

            seasons[0].Number.Should().Be(1);
            seasons[0].Aired.Should().Be(6);
            seasons[0].Completed.Should().Be(6);
            seasons[0].Episodes.Should().NotBeNull();
            seasons[0].Episodes.Should().HaveCount(6);

            var episodes = seasons[0].Episodes.ToArray();

            episodes[0].Number.Should().Be(1);
            episodes[0].Completed.Should().BeTrue();
            episodes[0].LastWatchedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            episodes[1].Number.Should().Be(2);
            episodes[1].Completed.Should().BeTrue();
            episodes[1].LastWatchedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            episodes[2].Number.Should().Be(3);
            episodes[2].Completed.Should().BeTrue();
            episodes[2].LastWatchedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            episodes[3].Number.Should().Be(4);
            episodes[3].Completed.Should().BeTrue();
            episodes[3].LastWatchedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            episodes[4].Number.Should().Be(5);
            episodes[4].Completed.Should().BeTrue();
            episodes[4].LastWatchedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            episodes[5].Number.Should().Be(6);
            episodes[5].Completed.Should().BeTrue();
            episodes[5].LastWatchedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            response.HiddenSeasons.Should().NotBeNull();
            response.HiddenSeasons.Should().HaveCount(1);

            var hiddenSeasons = response.HiddenSeasons.ToArray();

            hiddenSeasons[0].Number.Should().Be(2);
            hiddenSeasons[0].Ids.Should().NotBeNull();
            hiddenSeasons[0].Ids.Trakt.Should().Be(3051);
            hiddenSeasons[0].Ids.Tvdb.Should().Be(498968);
            hiddenSeasons[0].Ids.Tmdb.Should().Be(53334);
            hiddenSeasons[0].Ids.TvRage.Should().NotHaveValue();

            response.NextEpisode.Should().BeNull();
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowWatchedProgressWithSpecials()
        {
            var showWatchedProgress = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowWatchedProgressComplete.json");
            showWatchedProgress.Should().NotBeNullOrEmpty();

            var showId = "1390";
            var specials = true;

            TestUtility.SetupMockResponseWithOAuth($"shows/{showId}/progress/watched?specials={specials.ToString().ToLower()}",
                                                   showWatchedProgress);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetShowWatchedProgressAsync(showId, null, specials).Result;

            response.Should().NotBeNull();

            response.Aired.Should().Be(6);
            response.Completed.Should().Be(6);
            response.LastWatchedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());
            response.Seasons.Should().NotBeNull();
            response.Seasons.Should().HaveCount(1);

            var seasons = response.Seasons.ToArray();

            seasons[0].Number.Should().Be(1);
            seasons[0].Aired.Should().Be(6);
            seasons[0].Completed.Should().Be(6);
            seasons[0].Episodes.Should().NotBeNull();
            seasons[0].Episodes.Should().HaveCount(6);

            var episodes = seasons[0].Episodes.ToArray();

            episodes[0].Number.Should().Be(1);
            episodes[0].Completed.Should().BeTrue();
            episodes[0].LastWatchedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            episodes[1].Number.Should().Be(2);
            episodes[1].Completed.Should().BeTrue();
            episodes[1].LastWatchedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            episodes[2].Number.Should().Be(3);
            episodes[2].Completed.Should().BeTrue();
            episodes[2].LastWatchedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            episodes[3].Number.Should().Be(4);
            episodes[3].Completed.Should().BeTrue();
            episodes[3].LastWatchedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            episodes[4].Number.Should().Be(5);
            episodes[4].Completed.Should().BeTrue();
            episodes[4].LastWatchedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            episodes[5].Number.Should().Be(6);
            episodes[5].Completed.Should().BeTrue();
            episodes[5].LastWatchedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            response.HiddenSeasons.Should().NotBeNull();
            response.HiddenSeasons.Should().HaveCount(1);

            var hiddenSeasons = response.HiddenSeasons.ToArray();

            hiddenSeasons[0].Number.Should().Be(2);
            hiddenSeasons[0].Ids.Should().NotBeNull();
            hiddenSeasons[0].Ids.Trakt.Should().Be(3051);
            hiddenSeasons[0].Ids.Tvdb.Should().Be(498968);
            hiddenSeasons[0].Ids.Tmdb.Should().Be(53334);
            hiddenSeasons[0].Ids.TvRage.Should().NotHaveValue();

            response.NextEpisode.Should().BeNull();
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowWatchedProgressComplete()
        {
            var showWatchedProgress = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowWatchedProgressComplete.json");
            showWatchedProgress.Should().NotBeNullOrEmpty();

            var showId = "1390";
            var hidden = true;
            var specials = true;

            TestUtility.SetupMockResponseWithOAuth(
                $"shows/{showId}/progress/watched?hidden={hidden.ToString().ToLower()}&specials={specials.ToString().ToLower()}",
                showWatchedProgress);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetShowWatchedProgressAsync(showId, hidden, specials).Result;

            response.Should().NotBeNull();

            response.Aired.Should().Be(6);
            response.Completed.Should().Be(6);
            response.LastWatchedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());
            response.Seasons.Should().NotBeNull();
            response.Seasons.Should().HaveCount(1);

            var seasons = response.Seasons.ToArray();

            seasons[0].Number.Should().Be(1);
            seasons[0].Aired.Should().Be(6);
            seasons[0].Completed.Should().Be(6);
            seasons[0].Episodes.Should().NotBeNull();
            seasons[0].Episodes.Should().HaveCount(6);

            var episodes = seasons[0].Episodes.ToArray();

            episodes[0].Number.Should().Be(1);
            episodes[0].Completed.Should().BeTrue();
            episodes[0].LastWatchedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            episodes[1].Number.Should().Be(2);
            episodes[1].Completed.Should().BeTrue();
            episodes[1].LastWatchedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            episodes[2].Number.Should().Be(3);
            episodes[2].Completed.Should().BeTrue();
            episodes[2].LastWatchedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            episodes[3].Number.Should().Be(4);
            episodes[3].Completed.Should().BeTrue();
            episodes[3].LastWatchedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            episodes[4].Number.Should().Be(5);
            episodes[4].Completed.Should().BeTrue();
            episodes[4].LastWatchedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            episodes[5].Number.Should().Be(6);
            episodes[5].Completed.Should().BeTrue();
            episodes[5].LastWatchedAt.Should().Be(DateTime.Parse("2015-03-21T19:03:58.000Z").ToUniversalTime());

            response.HiddenSeasons.Should().NotBeNull();
            response.HiddenSeasons.Should().HaveCount(1);

            var hiddenSeasons = response.HiddenSeasons.ToArray();

            hiddenSeasons[0].Number.Should().Be(2);
            hiddenSeasons[0].Ids.Should().NotBeNull();
            hiddenSeasons[0].Ids.Trakt.Should().Be(3051);
            hiddenSeasons[0].Ids.Tvdb.Should().Be(498968);
            hiddenSeasons[0].Ids.Tmdb.Should().Be(53334);
            hiddenSeasons[0].Ids.TvRage.Should().NotHaveValue();

            response.NextEpisode.Should().BeNull();
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowWatchedProgressExceptions()
        {
            var showId = "1390";
            var uri = $"shows/{showId}/progress/watched";

            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.Unauthorized);

            Func<Task<TraktShowWatchedProgress>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetShowWatchedProgressAsync(showId);
            act.ShouldThrow<TraktAuthorizationException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithOAuth(uri, HttpStatusCode.NotFound);
            act.ShouldThrow<TraktShowNotFoundException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithOAuth(uri, HttpStatusCode.BadRequest);
            act.ShouldThrow<TraktBadRequestException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithOAuth(uri, HttpStatusCode.Forbidden);
            act.ShouldThrow<TraktForbiddenException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithOAuth(uri, (HttpStatusCode)412);
            act.ShouldThrow<TraktPreconditionFailedException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithOAuth(uri, (HttpStatusCode)429);
            act.ShouldThrow<TraktRateLimitException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithOAuth(uri, HttpStatusCode.InternalServerError);
            act.ShouldThrow<TraktServerException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithOAuth(uri, (HttpStatusCode)503);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithOAuth(uri, (HttpStatusCode)504);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithOAuth(uri, (HttpStatusCode)520);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithOAuth(uri, (HttpStatusCode)521);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithOAuth(uri, (HttpStatusCode)522);
            act.ShouldThrow<TraktServerUnavailableException>();
        }

        [TestMethod]
        public void TestTraktShowsModuleGetShowWatchedProgressArgumentExceptions()
        {
            var showWatchedProgress = TestUtility.ReadFileContents(@"Objects\Get\Shows\ShowWatchedProgressComplete.json");
            showWatchedProgress.Should().NotBeNullOrEmpty();

            var showId = "1390";

            TestUtility.SetupMockResponseWithOAuth($"shows/{showId}/progress/watched", showWatchedProgress);

            Func<Task<TraktShowWatchedProgress>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetShowWatchedProgressAsync(null);
            act.ShouldThrow<ArgumentException>();

            act = async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetShowWatchedProgressAsync(string.Empty);
            act.ShouldThrow<ArgumentException>();
        }

        #endregion

        // -----------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------

        #region TrendingShows

        [TestMethod]
        public void TestTraktShowsModuleGetTrendingShows()
        {
            var showsTrending = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsTrending.json");
            showsTrending.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var userCount = 300;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/trending", showsTrending, 1, 10, 1, itemCount, userCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetTrendingShowsAsync().Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(10);
            response.Page.Should().Be(1);
            response.PageCount.Should().Be(1);
            response.UserCount.Should().HaveValue().And.Be(userCount);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetTrendingShowsWithExtendedOption()
        {
            var showsTrending = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsTrending.json");
            showsTrending.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var userCount = 300;

            var extendedOption = new TraktExtendedOption
            {
                Full = true,
                Images = true
            };

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/trending?extended={extendedOption.ToString()}",
                                                                showsTrending, 1, 10, 1, itemCount, userCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetTrendingShowsAsync(extendedOption).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(10);
            response.Page.Should().Be(1);
            response.PageCount.Should().Be(1);
            response.UserCount.Should().HaveValue().And.Be(userCount);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetTrendingShowsWithPage()
        {
            var showsTrending = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsTrending.json");
            showsTrending.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var userCount = 300;
            var page = 2;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/trending?page={page}", showsTrending, page, 10, 1, itemCount, userCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetTrendingShowsAsync(null, page).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(10);
            response.Page.Should().Be(page);
            response.PageCount.Should().Be(1);
            response.UserCount.Should().HaveValue().And.Be(userCount);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetTrendingShowsWithExtendedOptionAndPage()
        {
            var showsTrending = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsTrending.json");
            showsTrending.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var userCount = 300;
            var page = 2;

            var extendedOption = new TraktExtendedOption
            {
                Full = true,
                Images = true
            };

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/trending?extended={extendedOption.ToString()}&page={page}",
                                                                showsTrending, page, 10, 1, itemCount, userCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetTrendingShowsAsync(extendedOption, page).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(10);
            response.Page.Should().Be(page);
            response.PageCount.Should().Be(1);
            response.UserCount.Should().HaveValue().And.Be(userCount);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetTrendingShowsWithLimit()
        {
            var showsTrending = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsTrending.json");
            showsTrending.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var userCount = 300;
            var limit = 4;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/trending?limit={limit}", showsTrending, 1, limit, 1, itemCount, userCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetTrendingShowsAsync(null, null, limit).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(limit);
            response.Page.Should().Be(1);
            response.PageCount.Should().Be(1);
            response.UserCount.Should().HaveValue().And.Be(userCount);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetTrendingShowsWithExtendedOptionAndLimit()
        {
            var showsTrending = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsTrending.json");
            showsTrending.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var userCount = 300;
            var limit = 4;

            var extendedOption = new TraktExtendedOption
            {
                Full = true,
                Images = true
            };

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/trending?extended={extendedOption.ToString()}&limit={limit}",
                                                                showsTrending, 1, limit, 1, itemCount, userCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetTrendingShowsAsync(extendedOption, null, limit).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(limit);
            response.Page.Should().Be(1);
            response.PageCount.Should().Be(1);
            response.UserCount.Should().HaveValue().And.Be(userCount);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetTrendingShowsWithPageAndLimit()
        {
            var showsTrending = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsTrending.json");
            showsTrending.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var userCount = 300;
            var page = 2;
            var limit = 4;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/trending?page={page}&limit={limit}",
                                                                showsTrending, page, limit, 1, itemCount, userCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetTrendingShowsAsync(null, page, limit).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(limit);
            response.Page.Should().Be(page);
            response.PageCount.Should().Be(1);
            response.UserCount.Should().HaveValue().And.Be(userCount);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetTrendingShowsComplete()
        {
            var showsTrending = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsTrending.json");
            showsTrending.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var userCount = 300;
            var page = 2;
            var limit = 4;

            var extendedOption = new TraktExtendedOption
            {
                Full = true,
                Images = true
            };

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/trending?extended={extendedOption.ToString()}&page={page}&limit={limit}",
                                                                showsTrending, page, limit, 1, itemCount, userCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetTrendingShowsAsync(extendedOption, page, limit).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(limit);
            response.Page.Should().Be(page);
            response.PageCount.Should().Be(1);
            response.UserCount.Should().HaveValue().And.Be(userCount);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetTrendingShowsExceptions()
        {
            var uri = $"shows/trending";

            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.BadRequest);

            Func<Task<TraktPaginationListResult<TraktTrendingShow>>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetTrendingShowsAsync();
            act.ShouldThrow<TraktBadRequestException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.Forbidden);
            act.ShouldThrow<TraktForbiddenException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)412);
            act.ShouldThrow<TraktPreconditionFailedException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)429);
            act.ShouldThrow<TraktRateLimitException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.InternalServerError);
            act.ShouldThrow<TraktServerException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)503);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)504);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)520);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)521);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)522);
            act.ShouldThrow<TraktServerUnavailableException>();
        }

        #endregion

        // -----------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------

        #region PopularShows

        [TestMethod]
        public void TestTraktShowsModuleGetPopularShows()
        {
            var popularShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsPopular.json");
            popularShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/popular", popularShows, 1, 10, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetPopularShowsAsync().Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(10);
            response.Page.Should().Be(1);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetPopularShowsWithExtendedOption()
        {
            var popularShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsPopular.json");
            popularShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;

            var extendedOption = new TraktExtendedOption
            {
                Full = true,
                Images = true
            };

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/popular?extended={extendedOption.ToString()}",
                                                                popularShows, 1, 10, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetPopularShowsAsync(extendedOption).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(10);
            response.Page.Should().Be(1);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetPopularShowsWithPage()
        {
            var popularShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsPopular.json");
            popularShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var page = 2;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/popular?page={page}", popularShows, page, 10, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetPopularShowsAsync(null, page).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(10);
            response.Page.Should().Be(page);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetPopularShowsWithExtendedOptionAndPage()
        {
            var popularShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsPopular.json");
            popularShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var page = 2;

            var extendedOption = new TraktExtendedOption
            {
                Full = true,
                Images = true
            };

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/popular?extended={extendedOption.ToString()}&page={page}",
                                                                popularShows, page, 10, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetPopularShowsAsync(extendedOption, page).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(10);
            response.Page.Should().Be(page);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetPopularShowsWithLimit()
        {
            var popularShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsPopular.json");
            popularShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var limit = 4;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/popular?limit={limit}", popularShows, 1, limit, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetPopularShowsAsync(null, null, limit).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(limit);
            response.Page.Should().Be(1);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetPopularShowsWithExtendedOptionAndLimit()
        {
            var popularShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsPopular.json");
            popularShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var limit = 4;

            var extendedOption = new TraktExtendedOption
            {
                Full = true,
                Images = true
            };

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/popular?extended={extendedOption.ToString()}&limit={limit}",
                                                                popularShows, 1, limit, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetPopularShowsAsync(extendedOption, null, limit).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(limit);
            response.Page.Should().Be(1);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetPopularShowsWithPageAndLimit()
        {
            var popularShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsPopular.json");
            popularShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var page = 2;
            var limit = 4;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/popular?page={page}&limit={limit}", popularShows, page, limit, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetPopularShowsAsync(null, page, limit).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(limit);
            response.Page.Should().Be(page);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetPopularShowsComplete()
        {
            var popularShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsPopular.json");
            popularShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var page = 2;
            var limit = 4;

            var extendedOption = new TraktExtendedOption
            {
                Full = true,
                Images = true
            };

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/popular?extended={extendedOption.ToString()}&page={page}&limit={limit}",
                                                                popularShows, page, limit, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetPopularShowsAsync(extendedOption, page, limit).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(limit);
            response.Page.Should().Be(page);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetPopularShowsExceptions()
        {
            var uri = $"shows/popular";

            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.BadRequest);

            Func<Task<TraktPaginationListResult<TraktPopularShow>>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetPopularShowsAsync();
            act.ShouldThrow<TraktBadRequestException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.Forbidden);
            act.ShouldThrow<TraktForbiddenException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)412);
            act.ShouldThrow<TraktPreconditionFailedException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)429);
            act.ShouldThrow<TraktRateLimitException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.InternalServerError);
            act.ShouldThrow<TraktServerException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)503);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)504);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)520);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)521);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)522);
            act.ShouldThrow<TraktServerUnavailableException>();
        }

        #endregion

        // -----------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------

        #region MostPlayedShows

        [TestMethod]
        public void TestTraktShowsModuleGetMostPlayedShows()
        {
            var mostPlayedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostPlayed.json");
            mostPlayedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/played", mostPlayedShows, 1, 10, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostPlayedShowsAsync().Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(10);
            response.Page.Should().Be(1);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostPlayedShowsWithPeriod()
        {
            var mostPlayedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostPlayed.json");
            mostPlayedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var period = TraktPeriod.Monthly;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/played/{period.AsString()}",
                                                                mostPlayedShows, 1, 10, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostPlayedShowsAsync(period).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(10);
            response.Page.Should().Be(1);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostPlayedShowsWithExtendedOption()
        {
            var mostPlayedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostPlayed.json");
            mostPlayedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;

            var extendedOption = new TraktExtendedOption
            {
                Full = true,
                Images = true
            };

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/played?extended={extendedOption.ToString()}",
                                                                mostPlayedShows, 1, 10, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostPlayedShowsAsync(null, extendedOption).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(10);
            response.Page.Should().Be(1);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostPlayedShowsWithPage()
        {
            var mostPlayedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostPlayed.json");
            mostPlayedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var page = 2;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/played?page={page}", mostPlayedShows, page, 10, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostPlayedShowsAsync(null, null, page).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(10);
            response.Page.Should().Be(page);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostPlayedShowsWithLimit()
        {
            var mostPlayedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostPlayed.json");
            mostPlayedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var limit = 4;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/played?limit={limit}", mostPlayedShows, 1, limit, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostPlayedShowsAsync(null, null, null, limit).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(limit);
            response.Page.Should().Be(1);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostPlayedShowsWithPeriodAndExtendedOption()
        {
            var mostPlayedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostPlayed.json");
            mostPlayedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var period = TraktPeriod.Monthly;

            var extendedOption = new TraktExtendedOption
            {
                Full = true,
                Images = true
            };

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/played/{period.AsString()}?extended={extendedOption.ToString()}",
                                                                mostPlayedShows, 1, 10, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostPlayedShowsAsync(period, extendedOption).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(10);
            response.Page.Should().Be(1);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostPlayedShowsWithPeriodAndPage()
        {
            var mostPlayedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostPlayed.json");
            mostPlayedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var page = 2;
            var period = TraktPeriod.Monthly;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/played/{period.AsString()}?page={page}",
                                                                mostPlayedShows, page, 10, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostPlayedShowsAsync(period, null, page).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(10);
            response.Page.Should().Be(page);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostPlayedShowsWithPeriodAndLimit()
        {
            var mostPlayedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostPlayed.json");
            mostPlayedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var limit = 4;
            var period = TraktPeriod.Monthly;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/played/{period.AsString()}?limit={limit}",
                                                                mostPlayedShows, 1, limit, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostPlayedShowsAsync(period, null, null, limit).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(limit);
            response.Page.Should().Be(1);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostPlayedShowsWithExtendedOptionAndPage()
        {
            var mostPlayedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostPlayed.json");
            mostPlayedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var page = 2;

            var extendedOption = new TraktExtendedOption
            {
                Full = true,
                Images = true
            };

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/played?extended={extendedOption.ToString()}&page={page}",
                                                                mostPlayedShows, page, 10, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostPlayedShowsAsync(null, extendedOption, page).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(10);
            response.Page.Should().Be(page);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostPlayedShowsWithExtendedOptionAndLimit()
        {
            var mostPlayedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostPlayed.json");
            mostPlayedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var limit = 4;

            var extendedOption = new TraktExtendedOption
            {
                Full = true,
                Images = true
            };

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/played?extended={extendedOption.ToString()}&limit={limit}",
                                                                mostPlayedShows, 1, limit, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostPlayedShowsAsync(null, extendedOption, null, limit).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(limit);
            response.Page.Should().Be(1);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostPlayedShowsWithPageAndLimit()
        {
            var mostPlayedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostPlayed.json");
            mostPlayedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var page = 2;
            var limit = 4;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/played?page={page}&limit={limit}",
                                                                mostPlayedShows, page, limit, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostPlayedShowsAsync(null, null, page, limit).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(limit);
            response.Page.Should().Be(page);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostPlayedShowsWithExtendedOptionAndPageAndLimit()
        {
            var mostPlayedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostPlayed.json");
            mostPlayedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var page = 2;
            var limit = 4;

            var extendedOption = new TraktExtendedOption
            {
                Full = true,
                Images = true
            };

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/played?extended={extendedOption.ToString()}&page={page}&limit={limit}",
                                                                mostPlayedShows, page, limit, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostPlayedShowsAsync(null, extendedOption, page, limit).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(limit);
            response.Page.Should().Be(page);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostPlayedShowsWithPeriodAndPageAndLimit()
        {
            var mostPlayedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostPlayed.json");
            mostPlayedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var page = 2;
            var limit = 4;
            var period = TraktPeriod.Monthly;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/played/{period.AsString()}?page={page}&limit={limit}",
                                                                mostPlayedShows, page, limit, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostPlayedShowsAsync(period, null, page, limit).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(limit);
            response.Page.Should().Be(page);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostPlayedShowsComplete()
        {
            var mostPlayedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostPlayed.json");
            mostPlayedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var page = 2;
            var limit = 4;
            var period = TraktPeriod.Monthly;

            var extendedOption = new TraktExtendedOption
            {
                Full = true,
                Images = true
            };

            TestUtility.SetupMockPaginationResponseWithoutOAuth(
                $"shows/played/{period.AsString()}?extended={extendedOption.ToString()}&page={page}&limit={limit}",
                mostPlayedShows, page, limit, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostPlayedShowsAsync(period, extendedOption, page, limit).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(limit);
            response.Page.Should().Be(page);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostPlayedShowsExceptions()
        {
            var uri = $"shows/played";

            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.BadRequest);

            Func<Task<TraktPaginationListResult<TraktMostPlayedShow>>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetMostPlayedShowsAsync();
            act.ShouldThrow<TraktBadRequestException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.Forbidden);
            act.ShouldThrow<TraktForbiddenException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)412);
            act.ShouldThrow<TraktPreconditionFailedException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)429);
            act.ShouldThrow<TraktRateLimitException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.InternalServerError);
            act.ShouldThrow<TraktServerException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)503);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)504);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)520);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)521);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)522);
            act.ShouldThrow<TraktServerUnavailableException>();
        }

        #endregion

        // -----------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------

        #region MostWatchedShows

        [TestMethod]
        public void TestTraktShowsModuleGetMostWatchedShows()
        {
            var mostWatchedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostWatched.json");
            mostWatchedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/watched", mostWatchedShows, 1, 10, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostWatchedShowsAsync().Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(10);
            response.Page.Should().Be(1);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostWatchedShowsWithPeriod()
        {
            var mostWatchedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostWatched.json");
            mostWatchedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var period = TraktPeriod.Monthly;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/watched/{period.AsString()}",
                                                                mostWatchedShows, 1, 10, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostWatchedShowsAsync(period).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(10);
            response.Page.Should().Be(1);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostWatchedShowsWithExtendedOption()
        {
            var mostWatchedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostWatched.json");
            mostWatchedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;

            var extendedOption = new TraktExtendedOption
            {
                Full = true,
                Images = true
            };

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/watched?extended={extendedOption.ToString()}",
                                                                mostWatchedShows, 1, 10, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostWatchedShowsAsync(null, extendedOption).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(10);
            response.Page.Should().Be(1);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostWatchedShowsWithPage()
        {
            var mostWatchedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostWatched.json");
            mostWatchedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var page = 2;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/watched?page={page}", mostWatchedShows, page, 10, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostWatchedShowsAsync(null, null, page).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(10);
            response.Page.Should().Be(page);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostWatchedShowsWithLimit()
        {
            var mostWatchedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostWatched.json");
            mostWatchedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var limit = 4;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/watched?limit={limit}", mostWatchedShows, 1, limit, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostWatchedShowsAsync(null, null, null, limit).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(limit);
            response.Page.Should().Be(1);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostWatchedShowsWithPeriodAndExtendedOption()
        {
            var mostWatchedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostWatched.json");
            mostWatchedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var period = TraktPeriod.Monthly;

            var extendedOption = new TraktExtendedOption
            {
                Full = true,
                Images = true
            };

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/watched/{period.AsString()}?extended={extendedOption.ToString()}",
                                                                mostWatchedShows, 1, 10, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostWatchedShowsAsync(period, extendedOption).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(10);
            response.Page.Should().Be(1);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostWatchedShowsWithPeriodAndPage()
        {
            var mostWatchedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostWatched.json");
            mostWatchedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var page = 2;
            var period = TraktPeriod.Monthly;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/watched/{period.AsString()}?page={page}",
                                                                mostWatchedShows, page, 10, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostWatchedShowsAsync(period, null, page).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(10);
            response.Page.Should().Be(page);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostWatchedShowsWithPeriodAndLimit()
        {
            var mostWatchedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostWatched.json");
            mostWatchedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var limit = 4;
            var period = TraktPeriod.Monthly;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/watched/{period.AsString()}?limit={limit}",
                                                                mostWatchedShows, 1, limit, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostWatchedShowsAsync(period, null, null, limit).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(limit);
            response.Page.Should().Be(1);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostWatchedShowsWithExtendedOptionAndPage()
        {
            var mostWatchedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostWatched.json");
            mostWatchedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var page = 2;

            var extendedOption = new TraktExtendedOption
            {
                Full = true,
                Images = true
            };

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/watched?extended={extendedOption.ToString()}&page={page}",
                                                                mostWatchedShows, page, 10, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostWatchedShowsAsync(null, extendedOption, page).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(10);
            response.Page.Should().Be(page);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostWatchedShowsWithExtendedOptionAndLimit()
        {
            var mostWatchedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostWatched.json");
            mostWatchedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var limit = 4;

            var extendedOption = new TraktExtendedOption
            {
                Full = true,
                Images = true
            };

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/watched?extended={extendedOption.ToString()}&limit={limit}",
                                                                mostWatchedShows, 1, limit, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostWatchedShowsAsync(null, extendedOption, null, limit).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(limit);
            response.Page.Should().Be(1);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostWatchedShowsWithPageAndLimit()
        {
            var mostWatchedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostWatched.json");
            mostWatchedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var page = 2;
            var limit = 4;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/watched?page={page}&limit={limit}",
                                                                mostWatchedShows, page, limit, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostWatchedShowsAsync(null, null, page, limit).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(limit);
            response.Page.Should().Be(page);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostWatchedShowsWithExtendedOptionAndPageAndLimit()
        {
            var mostWatchedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostWatched.json");
            mostWatchedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var page = 2;
            var limit = 4;

            var extendedOption = new TraktExtendedOption
            {
                Full = true,
                Images = true
            };

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/watched?extended={extendedOption.ToString()}&page={page}&limit={limit}",
                                                                mostWatchedShows, page, limit, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostWatchedShowsAsync(null, extendedOption, page, limit).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(limit);
            response.Page.Should().Be(page);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostWatchedShowsWithPeriodAndPageAndLimit()
        {
            var mostWatchedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostWatched.json");
            mostWatchedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var page = 2;
            var limit = 4;
            var period = TraktPeriod.Monthly;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/watched/{period.AsString()}?page={page}&limit={limit}",
                                                                mostWatchedShows, page, limit, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostWatchedShowsAsync(period, null, page, limit).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(limit);
            response.Page.Should().Be(page);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostWatchedShowsComplete()
        {
            var mostWatchedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostWatched.json");
            mostWatchedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var page = 2;
            var limit = 4;
            var period = TraktPeriod.Monthly;

            var extendedOption = new TraktExtendedOption
            {
                Full = true,
                Images = true
            };

            TestUtility.SetupMockPaginationResponseWithoutOAuth(
                $"shows/watched/{period.AsString()}?extended={extendedOption.ToString()}&page={page}&limit={limit}",
                mostWatchedShows, page, limit, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostWatchedShowsAsync(period, extendedOption, page, limit).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(limit);
            response.Page.Should().Be(page);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostWatchedShowsExceptions()
        {
            var uri = $"shows/watched";

            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.BadRequest);

            Func<Task<TraktPaginationListResult<TraktMostWatchedShow>>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetMostWatchedShowsAsync();
            act.ShouldThrow<TraktBadRequestException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.Forbidden);
            act.ShouldThrow<TraktForbiddenException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)412);
            act.ShouldThrow<TraktPreconditionFailedException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)429);
            act.ShouldThrow<TraktRateLimitException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.InternalServerError);
            act.ShouldThrow<TraktServerException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)503);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)504);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)520);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)521);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)522);
            act.ShouldThrow<TraktServerUnavailableException>();
        }

        #endregion

        // -----------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------

        #region MostCollectedShows

        [TestMethod]
        public void TestTraktShowsModuleGetMostCollectedShows()
        {
            var mostCollectedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostCollected.json");
            mostCollectedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/collected", mostCollectedShows, 1, 10, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostCollectedShowsAsync().Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(10);
            response.Page.Should().Be(1);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostCollectedShowsWithPeriod()
        {
            var mostCollectedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostCollected.json");
            mostCollectedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var period = TraktPeriod.Monthly;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/collected/{period.AsString()}",
                                                                mostCollectedShows, 1, 10, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostCollectedShowsAsync(period).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(10);
            response.Page.Should().Be(1);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostCollectedShowsWithExtendedOption()
        {
            var mostCollectedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostCollected.json");
            mostCollectedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;

            var extendedOption = new TraktExtendedOption
            {
                Full = true,
                Images = true
            };

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/collected?extended={extendedOption.ToString()}",
                                                                mostCollectedShows, 1, 10, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostCollectedShowsAsync(null, extendedOption).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(10);
            response.Page.Should().Be(1);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostCollectedShowsWithPage()
        {
            var mostCollectedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostCollected.json");
            mostCollectedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var page = 2;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/collected?page={page}", mostCollectedShows, page, 10, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostCollectedShowsAsync(null, null, page).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(10);
            response.Page.Should().Be(page);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostCollectedShowsWithLimit()
        {
            var mostCollectedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostCollected.json");
            mostCollectedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var limit = 4;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/collected?limit={limit}", mostCollectedShows, 1, limit, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostCollectedShowsAsync(null, null, null, limit).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(limit);
            response.Page.Should().Be(1);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostCollectedShowsWithPeriodAndExtendedOption()
        {
            var mostCollectedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostCollected.json");
            mostCollectedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var period = TraktPeriod.Monthly;

            var extendedOption = new TraktExtendedOption
            {
                Full = true,
                Images = true
            };

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/collected/{period.AsString()}?extended={extendedOption.ToString()}",
                                                                mostCollectedShows, 1, 10, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostCollectedShowsAsync(period, extendedOption).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(10);
            response.Page.Should().Be(1);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostCollectedShowsWithPeriodAndPage()
        {
            var mostCollectedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostCollected.json");
            mostCollectedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var page = 2;
            var period = TraktPeriod.Monthly;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/collected/{period.AsString()}?page={page}",
                                                                mostCollectedShows, page, 10, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostCollectedShowsAsync(period, null, page).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(10);
            response.Page.Should().Be(page);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostCollectedShowsWithPeriodAndLimit()
        {
            var mostCollectedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostCollected.json");
            mostCollectedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var limit = 4;
            var period = TraktPeriod.Monthly;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/collected/{period.AsString()}?limit={limit}",
                                                                mostCollectedShows, 1, limit, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostCollectedShowsAsync(period, null, null, limit).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(limit);
            response.Page.Should().Be(1);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostCollectedShowsWithExtendedOptionAndPage()
        {
            var mostCollectedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostCollected.json");
            mostCollectedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var page = 2;

            var extendedOption = new TraktExtendedOption
            {
                Full = true,
                Images = true
            };

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/collected?extended={extendedOption.ToString()}&page={page}",
                                                                mostCollectedShows, page, 10, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostCollectedShowsAsync(null, extendedOption, page).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(10);
            response.Page.Should().Be(page);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostCollectedShowsWithExtendedOptionAndLimit()
        {
            var mostCollectedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostCollected.json");
            mostCollectedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var limit = 4;

            var extendedOption = new TraktExtendedOption
            {
                Full = true,
                Images = true
            };

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/collected?extended={extendedOption.ToString()}&limit={limit}",
                                                                mostCollectedShows, 1, limit, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostCollectedShowsAsync(null, extendedOption, null, limit).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(limit);
            response.Page.Should().Be(1);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostCollectedShowsWithPageAndLimit()
        {
            var mostCollectedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostCollected.json");
            mostCollectedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var page = 2;
            var limit = 4;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/collected?page={page}&limit={limit}",
                                                                mostCollectedShows, page, limit, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostCollectedShowsAsync(null, null, page, limit).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(limit);
            response.Page.Should().Be(page);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostCollectedShowsWithExtendedOptionAndPageAndLimit()
        {
            var mostCollectedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostCollected.json");
            mostCollectedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var page = 2;
            var limit = 4;

            var extendedOption = new TraktExtendedOption
            {
                Full = true,
                Images = true
            };

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/collected?extended={extendedOption.ToString()}&page={page}&limit={limit}",
                                                                mostCollectedShows, page, limit, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostCollectedShowsAsync(null, extendedOption, page, limit).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(limit);
            response.Page.Should().Be(page);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostCollectedShowsWithPeriodAndPageAndLimit()
        {
            var mostCollectedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostCollected.json");
            mostCollectedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var page = 2;
            var limit = 4;
            var period = TraktPeriod.Monthly;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/collected/{period.AsString()}?page={page}&limit={limit}",
                                                                mostCollectedShows, page, limit, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostCollectedShowsAsync(period, null, page, limit).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(limit);
            response.Page.Should().Be(page);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostCollectedShowsComplete()
        {
            var mostCollectedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostCollected.json");
            mostCollectedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var page = 2;
            var limit = 4;
            var period = TraktPeriod.Monthly;

            var extendedOption = new TraktExtendedOption
            {
                Full = true,
                Images = true
            };

            TestUtility.SetupMockPaginationResponseWithoutOAuth(
                $"shows/collected/{period.AsString()}?extended={extendedOption.ToString()}&page={page}&limit={limit}",
                mostCollectedShows, page, limit, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostCollectedShowsAsync(period, extendedOption, page, limit).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(limit);
            response.Page.Should().Be(page);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostCollectedShowsExceptions()
        {
            var uri = $"shows/collected";

            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.BadRequest);

            Func<Task<TraktPaginationListResult<TraktMostCollectedShow>>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetMostCollectedShowsAsync();
            act.ShouldThrow<TraktBadRequestException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.Forbidden);
            act.ShouldThrow<TraktForbiddenException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)412);
            act.ShouldThrow<TraktPreconditionFailedException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)429);
            act.ShouldThrow<TraktRateLimitException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.InternalServerError);
            act.ShouldThrow<TraktServerException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)503);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)504);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)520);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)521);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)522);
            act.ShouldThrow<TraktServerUnavailableException>();
        }

        #endregion

        // -----------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------

        #region MostAnticipatedShows

        [TestMethod]
        public void TestTraktShowsModuleGetMostAnticipatedShows()
        {
            var mostAnticipatedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostAnticipated.json");
            mostAnticipatedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/anticipated", mostAnticipatedShows, 1, 10, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostAnticipatedShowsAsync().Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(10);
            response.Page.Should().Be(1);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostAnticipatedShowsWithExtendedOption()
        {
            var mostAnticipatedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostAnticipated.json");
            mostAnticipatedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;

            var extendedOption = new TraktExtendedOption
            {
                Full = true,
                Images = true
            };

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/anticipated?extended={extendedOption.ToString()}",
                                                                mostAnticipatedShows, 1, 10, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostAnticipatedShowsAsync(extendedOption).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(10);
            response.Page.Should().Be(1);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostAnticipatedShowsWithPage()
        {
            var mostAnticipatedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostAnticipated.json");
            mostAnticipatedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var page = 2;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/anticipated?page={page}", mostAnticipatedShows, page, 10, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostAnticipatedShowsAsync(null, page).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(10);
            response.Page.Should().Be(page);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostAnticipatedShowsWithExtendedOptionAndPage()
        {
            var mostAnticipatedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostAnticipated.json");
            mostAnticipatedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var page = 2;

            var extendedOption = new TraktExtendedOption
            {
                Full = true,
                Images = true
            };

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/anticipated?extended={extendedOption.ToString()}&page={page}",
                                                                mostAnticipatedShows, page, 10, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostAnticipatedShowsAsync(extendedOption, page).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(10);
            response.Page.Should().Be(page);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostAnticipatedShowsWithLimit()
        {
            var mostAnticipatedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostAnticipated.json");
            mostAnticipatedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var limit = 4;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/anticipated?limit={limit}", mostAnticipatedShows, 1, limit, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostAnticipatedShowsAsync(null, null, limit).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(limit);
            response.Page.Should().Be(1);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostAnticipatedShowsWithExtendedOptionAndLimit()
        {
            var mostAnticipatedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostAnticipated.json");
            mostAnticipatedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var limit = 4;

            var extendedOption = new TraktExtendedOption
            {
                Full = true,
                Images = true
            };

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/anticipated?extended={extendedOption.ToString()}&limit={limit}",
                                                                mostAnticipatedShows, 1, limit, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostAnticipatedShowsAsync(extendedOption, null, limit).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(limit);
            response.Page.Should().Be(1);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostAnticipatedShowsWithPageAndLimit()
        {
            var mostAnticipatedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostAnticipated.json");
            mostAnticipatedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var page = 2;
            var limit = 4;

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/anticipated?page={page}&limit={limit}",
                                                                mostAnticipatedShows, page, limit, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostAnticipatedShowsAsync(null, page, limit).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(limit);
            response.Page.Should().Be(page);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostAnticipatedShowsComplete()
        {
            var mostAnticipatedShows = TestUtility.ReadFileContents(@"Objects\Get\Shows\Common\ShowsMostAnticipated.json");
            mostAnticipatedShows.Should().NotBeNullOrEmpty();

            var itemCount = 2;
            var page = 2;
            var limit = 4;

            var extendedOption = new TraktExtendedOption
            {
                Full = true,
                Images = true
            };

            TestUtility.SetupMockPaginationResponseWithoutOAuth($"shows/anticipated?extended={extendedOption.ToString()}&page={page}&limit={limit}",
                                                                mostAnticipatedShows, page, limit, 1, itemCount);

            var response = TestUtility.MOCK_TEST_CLIENT.Shows.GetMostAnticipatedShowsAsync(extendedOption, page, limit).Result;

            response.Should().NotBeNull();
            response.Items.Should().NotBeNull().And.HaveCount(itemCount);
            response.ItemCount.Should().Be(itemCount);
            response.Limit.Should().Be(limit);
            response.Page.Should().Be(page);
            response.PageCount.Should().Be(1);
        }

        [TestMethod]
        public void TestTraktShowsModuleGetMostAnticipatedShowsExceptions()
        {
            var uri = $"shows/anticipated";

            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.BadRequest);

            Func<Task<TraktPaginationListResult<TraktMostAnticipatedShow>>> act =
                async () => await TestUtility.MOCK_TEST_CLIENT.Shows.GetMostAnticipatedShowsAsync();
            act.ShouldThrow<TraktBadRequestException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.Forbidden);
            act.ShouldThrow<TraktForbiddenException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)412);
            act.ShouldThrow<TraktPreconditionFailedException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)429);
            act.ShouldThrow<TraktRateLimitException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, HttpStatusCode.InternalServerError);
            act.ShouldThrow<TraktServerException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)503);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)504);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)520);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)521);
            act.ShouldThrow<TraktServerUnavailableException>();

            TestUtility.ClearMockHttpClient();
            TestUtility.SetupMockErrorResponseWithoutOAuth(uri, (HttpStatusCode)522);
            act.ShouldThrow<TraktServerUnavailableException>();
        }

        #endregion

        // -----------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------

        #region RecentlyUpdatedShows

        [TestMethod]
        public void TestTraktShowsModuleGetRecentlyUpdatedShowss()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void TestTraktShowsModuleGetRecentlyUpdatedShowsWithStartDate()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void TestTraktShowsModuleGetRecentlyUpdatedShowsWithExtendedOption()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void TestTraktShowsModuleGetRecentlyUpdatedShowsWithPage()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void TestTraktShowsModuleGetRecentlyUpdatedShowsWithLimit()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void TestTraktShowsModuleGetRecentlyUpdatedShowsWithStartDateAndExtendedOption()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void TestTraktShowsModuleGetRecentlyUpdatedShowsWithStartDateAndPage()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void TestTraktShowsModuleGetRecentlyUpdatedShowsWithStartDateAndLimit()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void TestTraktShowsModuleGetRecentlyUpdatedShowsWithExtendedOptionAndPage()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void TestTraktShowsModuleGetRecentlyUpdatedShowsWithExtendedOptionAndLimit()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void TestTraktShowsModuleGetRecentlyUpdatedShowsWithPageAndLimit()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void TestTraktShowsModuleGetRecentlyUpdatedShowsWithExtendedOptionAndPageAndLimit()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void TestTraktShowsModuleGetRecentlyUpdatedShowsWithStartDateAndPageAndLimit()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void TestTraktShowsModuleGetRecentlyUpdatedShowsComplete()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void TestTraktShowsModuleGetRecentlyUpdatedShowsExceptions()
        {
            Assert.Fail();
        }

        #endregion
    }
}
