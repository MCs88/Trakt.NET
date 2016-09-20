﻿namespace TraktApiSharp.Experimental.Requests.Base.Get
{
    using Interfaces.Requests;
    using System.Net.Http;
    using TraktApiSharp.Requests;

    internal abstract class ATraktPaginationGetByIdRequest<TItem> : ATraktPaginationRequest<TItem>, ITraktRequest, ITraktHasId
    {
        public ATraktPaginationGetByIdRequest(TraktClient client) : base(client)
        {
            RequestId = new TraktRequestId();
        }

        public abstract TraktAuthorizationRequirement AuthorizationRequirement { get; }

        public HttpMethod Method => HttpMethod.Get;

        public string Id
        {
            get { return RequestId.Id; }
            set { RequestId.Id = value; }
        }

        public TraktRequestId RequestId { get; set; }
    }
}
