﻿namespace TraktApiSharp.Experimental.Requests.Comments.OAuth
{
    using TraktApiSharp.Requests;

    internal sealed class TraktCommentUpdateRequest
    {
        internal TraktCommentUpdateRequest(TraktClient client) { }

        public TraktRequestObjectType RequestObjectType => TraktRequestObjectType.Comments;

        public string UriTemplate => "comments/{id}";
    }
}
