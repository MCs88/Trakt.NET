﻿namespace TraktApiSharp.Experimental.Requests.Genres
{
    using Base.Get;
    using Objects.Basic;
    using TraktApiSharp.Requests;

    internal abstract class ATraktGenresRequest : ATraktListGetRequest<TraktGenre>
    {
        public ATraktGenresRequest(TraktClient client) : base(client) { }

        public override TraktAuthorizationRequirement AuthorizationRequirement => TraktAuthorizationRequirement.NotRequired;
    }
}
