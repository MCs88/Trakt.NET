﻿namespace TraktApiSharp.Objects.Post.Syncs.Ratings.Implementations
{
    using Get.Movies;
    using System;

    /// <summary>
    /// A Trakt ratings post movie, containing the required movie ids,
    /// an optional rating and an optional datetime, when the movie was rated.
    /// </summary>
    public class TraktSyncRatingsPostMovie : ITraktSyncRatingsPostMovie
    {
        /// <summary>Gets or sets the optional UTC datetime, when the Trakt movie was rated.</summary>
        public DateTime? RatedAt { get; set; }

        /// <summary>Gets or sets an optional rating for the movie.</summary>
        public int? Rating { get; set; }

        /// <summary>Gets or sets the optional title of the Trakt movie.<para>Nullable</para></summary>
        public string Title { get; set; }

        /// <summary>Gets or sets the optional year of the Trakt movie.</summary>
        public int? Year { get; set; }

        /// <summary>Gets or sets the required movie ids. See also <seealso cref="ITraktMovieIds" />.</summary>
        public ITraktMovieIds Ids { get; set; }
    }
}