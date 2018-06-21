﻿namespace TraktNet.Requests.Movies
{
    using Objects.Get.Movies;

    internal sealed class MoviesMostPlayedRequest : AMoviesMostPWCRequest<ITraktMostPWCMovie>
    {
        public override string UriTemplate => "movies/played{/period}{?extended,page,limit,query,years,genres,languages,countries,runtimes,ratings,certifications}";

        public override void Validate() { }
    }
}
