﻿namespace TraktNet.Objects.Post.Scrobbles.Responses
{
    using Get.Movies;

    /// <summary>Represents a movie scrobble response.</summary>
    public class TraktMovieScrobblePostResponse : TraktScrobblePostResponse, ITraktMovieScrobblePostResponse
    {
        /// <summary>
        /// Gets or sets the Trakt movie, which was scrobbled.
        /// See also <seealso cref="ITraktMovie" />.
        /// <para>Nullable</para>
        /// </summary>
        public ITraktMovie Movie { get; set; }
    }
}
