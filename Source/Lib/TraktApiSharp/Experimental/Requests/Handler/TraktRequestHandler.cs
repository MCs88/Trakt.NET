﻿namespace TraktApiSharp.Experimental.Requests.Handler
{
    using Checkins.OAuth;
    using Core;
    using Exceptions;
    using Interfaces.Base;
    using Responses;
    using Responses.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using TraktApiSharp.Experimental.Requests.Interfaces;
    using TraktApiSharp.Requests;
    using UriTemplates;
    using Utils;

    internal sealed class TraktRequestHandler : ITraktRequestHandler
    {
        internal static HttpClient s_httpClient;

        private TraktClient _client;

        internal TraktRequestHandler(TraktClient client)
        {
            _client = client;
        }

        public async Task<ITraktNoContentResponse> ExecuteNoContentRequestAsync(ITraktRequest request)
        {
            PreExecuteRequest(request);
            return await QueryNoContentAsync(SetupRequestMessage(request));
        }

        public async Task<ITraktResponse<TContentType>> ExecuteSingleItemRequestAsync<TContentType>(ITraktRequest<TContentType> request)
        {
            PreExecuteRequest(request);
            return await QuerySingleItemAsync<TContentType>(SetupRequestMessage(request));
        }

        public async Task<ITraktListResponse<TContentType>> ExecuteListRequestAsync<TContentType>(ITraktRequest<TContentType> request)
        {
            PreExecuteRequest(request);
            return await QueryListAsync<TContentType>(SetupRequestMessage(request));
        }

        public async Task<ITraktPagedResponse<TContentType>> ExecutePagedRequestAsync<TContentType>(ITraktRequest<TContentType> request)
        {
            PreExecuteRequest(request);
            return await QueryPagedListAsync<TContentType>(SetupRequestMessage(request));
        }

        // post requests

        public async Task<ITraktNoContentResponse> ExecuteNoContentRequestAsync<TRequestBody>(ITraktPostRequest<TRequestBody> request)
        {
            PreExecuteRequest(request);
            return await QueryNoContentAsync(SetupRequestMessage(request));
        }

        public async Task<ITraktResponse<TContentType>> ExecuteSingleItemRequestAsync<TContentType, TRequestBody>(ITraktPostRequest<TContentType, TRequestBody> request)
        {
            PreExecuteRequest(request);
            var isCheckinRequest = request is TraktCheckinRequest<TContentType, TRequestBody>;
            return await QuerySingleItemAsync<TContentType>(SetupRequestMessage(request), isCheckinRequest);
        }

        public async Task<ITraktListResponse<TContentType>> ExecuteListRequestAsync<TContentType, TRequestBody>(ITraktPostRequest<TContentType, TRequestBody> request)
        {
            PreExecuteRequest(request);
            return await QueryListAsync<TContentType>(SetupRequestMessage(request));
        }

        public async Task<ITraktPagedResponse<TContentType>> ExecutePagedRequestAsync<TContentType, TRequestBody>(ITraktPostRequest<TContentType, TRequestBody> request)
        {
            PreExecuteRequest(request);
            return await QueryPagedListAsync<TContentType>(SetupRequestMessage(request));
        }

        // put requests

        public async Task<ITraktNoContentResponse> ExecuteNoContentRequestAsync<TRequestBody>(ITraktPutRequest<TRequestBody> request)
        {
            PreExecuteRequest(request);
            return await QueryNoContentAsync(SetupRequestMessage(request));
        }

        public async Task<ITraktResponse<TContentType>> ExecuteSingleItemRequestAsync<TContentType, TRequestBody>(ITraktPutRequest<TContentType, TRequestBody> request)
        {
            PreExecuteRequest(request);
            return await QuerySingleItemAsync<TContentType>(SetupRequestMessage(request));
        }

        public async Task<ITraktListResponse<TContentType>> ExecuteListRequestAsync<TContentType, TRequestBody>(ITraktPutRequest<TContentType, TRequestBody> request)
        {
            PreExecuteRequest(request);
            return await QueryListAsync<TContentType>(SetupRequestMessage(request));
        }

        public async Task<ITraktPagedResponse<TContentType>> ExecutePagedRequestAsync<TContentType, TRequestBody>(ITraktPutRequest<TContentType, TRequestBody> request)
        {
            PreExecuteRequest(request);
            return await QueryPagedListAsync<TContentType>(SetupRequestMessage(request));
        }

        // query response helper methods

        private async Task<ITraktNoContentResponse> QueryNoContentAsync(TraktHttpRequestMessage requestMessage)
        {
            var response = await ExecuteRequestAsync(requestMessage).ConfigureAwait(false);

            Debug.Assert(response != null && response.StatusCode == HttpStatusCode.NoContent,
                         "precondition for generating no content response failed");

            return new TraktNoContentResponse { IsSuccess = true };
        }

        private async Task<ITraktResponse<TContentType>> QuerySingleItemAsync<TContentType>(TraktHttpRequestMessage requestMessage, bool isCheckinRequest = false)
        {
            var response = await ExecuteRequestAsync(requestMessage, isCheckinRequest).ConfigureAwait(false);

            Debug.Assert(response != null && response.StatusCode != HttpStatusCode.NoContent,
                         "precondition for generating single item response failed");

            var responseContent = await GetResponseContentAsync(response).ConfigureAwait(false);
            Debug.Assert(!string.IsNullOrEmpty(responseContent), "precondition for deserializing response content failed");

            var contentObject = Json.Deserialize<TContentType>(responseContent);
            return new TraktResponse<TContentType> { IsSuccess = true, HasValue = contentObject != null, Value = contentObject };
        }

        private async Task<ITraktListResponse<TContentType>> QueryListAsync<TContentType>(TraktHttpRequestMessage requestMessage)
        {
            var response = await ExecuteRequestAsync(requestMessage).ConfigureAwait(false);

            Debug.Assert(response != null && response.StatusCode != HttpStatusCode.NoContent,
                         "precondition for generating list response failed");

            var responseContent = await GetResponseContentAsync(response).ConfigureAwait(false);
            Debug.Assert(!string.IsNullOrEmpty(responseContent), "precondition for deserializing response content failed");

            var contentObject = Json.Deserialize<IEnumerable<TContentType>>(responseContent);
            return new TraktListResponse<TContentType> { IsSuccess = true, HasValue = contentObject != null, Value = contentObject };
        }

        private async Task<ITraktPagedResponse<TContentType>> QueryPagedListAsync<TContentType>(TraktHttpRequestMessage requestMessage)
        {
            var response = await ExecuteRequestAsync(requestMessage).ConfigureAwait(false);

            Debug.Assert(response != null && response.StatusCode != HttpStatusCode.NoContent,
                         "precondition for generating paged list response failed");

            var responseContent = await GetResponseContentAsync(response).ConfigureAwait(false);
            Debug.Assert(!string.IsNullOrEmpty(responseContent), "precondition for deserializing response content failed");

            var contentObject = Json.Deserialize<IEnumerable<TContentType>>(responseContent);
            return new TraktPagedResponse<TContentType> { IsSuccess = true, HasValue = contentObject != null, Value = contentObject };
        }

        private async Task<HttpResponseMessage> ExecuteRequestAsync(TraktHttpRequestMessage requestMessage, bool isCheckinRequest = false)
        {
            var response = await s_httpClient.SendAsync(requestMessage).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                await ErrorHandlingAsync(response, requestMessage, isCheckinRequest).ConfigureAwait(false);

            return response;
        }

        private async Task<string> GetResponseContentAsync(HttpResponseMessage response)
            => response.Content != null ? await response.Content.ReadAsStringAsync() : string.Empty;

        private void PreExecuteRequest(ITraktRequest request)
        {
            ValidateRequest(request);
            SetupHttpClient();
        }

        private void ValidateRequest(ITraktRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            request.Validate();
        }

        private void SetupHttpClient()
        {
            var httpClient = s_httpClient ?? new HttpClient();
            SetDefaultRequestHeaders(httpClient);
        }

        private string BuildUrl(ITraktRequest request)
        {
            var uriTemplate = new UriTemplate(request.UriTemplate);
            var requestUriParameters = request.GetUriPathParameters();

            foreach (var parameter in requestUriParameters)
                uriTemplate.AddParameterFromKeyValuePair(parameter.Key, parameter.Value);

            var uri = uriTemplate.Resolve();
            return $"{_client.Configuration.BaseUrl}{uri}";
        }

        private TraktHttpRequestMessage SetupRequestMessage(ITraktRequest request)
        {
            var requestMessage = CreateRequestMessage(request);
            SetRequestMessageHeadersForAuthorization(requestMessage, request.AuthorizationRequirement);
            return requestMessage;
        }

        private TraktHttpRequestMessage SetupRequestMessage<TRequestBody>(ITraktPostRequest<TRequestBody> request)
        {
            var requestMessage = CreateRequestMessage(request);
            AddRequestBodyContent(requestMessage, request);
            SetRequestMessageHeadersForAuthorization(requestMessage, request.AuthorizationRequirement);
            return requestMessage;
        }

        private TraktHttpRequestMessage SetupRequestMessage<TContentType, TRequestBody>(ITraktPostRequest<TContentType, TRequestBody> request)
        {
            var requestMessage = CreateRequestMessage(request);
            AddRequestBodyContent(requestMessage, request);
            SetRequestMessageHeadersForAuthorization(requestMessage, request.AuthorizationRequirement);
            return requestMessage;
        }

        private TraktHttpRequestMessage SetupRequestMessage<TRequestBody>(ITraktPutRequest<TRequestBody> request)
        {
            var requestMessage = CreateRequestMessage(request);
            AddRequestBodyContent(requestMessage, request);
            SetRequestMessageHeadersForAuthorization(requestMessage, request.AuthorizationRequirement);
            return requestMessage;
        }

        private TraktHttpRequestMessage SetupRequestMessage<TContentType, TRequestBody>(ITraktPutRequest<TContentType, TRequestBody> request)
        {
            var requestMessage = CreateRequestMessage(request);
            AddRequestBodyContent(requestMessage, request);
            SetRequestMessageHeadersForAuthorization(requestMessage, request.AuthorizationRequirement);
            return requestMessage;
        }

        private TraktHttpRequestMessage CreateRequestMessage(ITraktRequest request)
        {
            const string seasonKey = "season";
            const string episodeKey = "episode";

            var url = BuildUrl(request);
            var requestMessage = new TraktHttpRequestMessage(request.Method, url) { Url = url };

            if (request is ITraktHasId)
            {
                var idRequest = request as ITraktHasId;

                requestMessage.Id = idRequest?.Id;
                requestMessage.RequestObjectType = idRequest?.RequestObjectType;
            }

            var parameters = request.GetUriPathParameters();

            if (parameters.Count != 0)
            {
                if (parameters.ContainsKey(seasonKey))
                    requestMessage.SeasonNumber = (uint)parameters[seasonKey];

                if (parameters.ContainsKey(episodeKey))
                    requestMessage.EpisodeNumber = (uint)parameters[episodeKey];
            }

            return requestMessage;
        }

        private void AddRequestBodyContent<TRequestBody>(TraktHttpRequestMessage requestMessage, ITraktHasRequestBody<TRequestBody> request)
        {
            if (requestMessage == null)
                throw new ArgumentNullException(nameof(requestMessage));

            string requestBodyJson;
            requestMessage.Content = GetRequestBodyContent(request, out requestBodyJson);
            requestMessage.RequestBodyJson = requestBodyJson;
        }

        private HttpContent GetRequestBodyContent<TRequestBody>(ITraktHasRequestBody<TRequestBody> request, out string requestBodyJson)
        {
            var requestBody = request.RequestBody;

            if (requestBody == null)
            {
                requestBodyJson = string.Empty;
                return null;
            }

            var json = Json.Serialize(requestBody);
            requestBodyJson = json;
            return !string.IsNullOrEmpty(json) ? new StringContent(json, Encoding.UTF8, "application/json") : null;
        }

        private void SetDefaultRequestHeaders(HttpClient httpClient)
        {
            var appJsonHeader = new MediaTypeWithQualityHeaderValue("application/json");

            if (!httpClient.DefaultRequestHeaders.Contains(TraktConstants.APIClientIdHeaderKey))
                httpClient.DefaultRequestHeaders.Add(TraktConstants.APIClientIdHeaderKey, _client.ClientId);

            if (!httpClient.DefaultRequestHeaders.Contains(TraktConstants.APIVersionHeaderKey))
                httpClient.DefaultRequestHeaders.Add(TraktConstants.APIVersionHeaderKey, $"{_client.Configuration.ApiVersion}");

            if (!httpClient.DefaultRequestHeaders.Accept.Contains(appJsonHeader))
                httpClient.DefaultRequestHeaders.Accept.Add(appJsonHeader);
        }

        private void SetRequestMessageHeadersForAuthorization(TraktHttpRequestMessage requestMessage, TraktAuthorizationRequirement authorizationRequirement)
        {
            if (requestMessage == null)
                throw new ArgumentNullException(nameof(requestMessage));

            if (authorizationRequirement == TraktAuthorizationRequirement.Required)
            {
                if (!_client.Authentication.IsAuthorized)
                    throw new TraktAuthorizationException("authorization is required for this request, but the current authorization parameters are invalid");

                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _client.Authentication.Authorization.AccessToken);
            }

            if (authorizationRequirement == TraktAuthorizationRequirement.Optional && _client.Configuration.ForceAuthorization && _client.Authentication.IsAuthorized)
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _client.Authentication.Authorization.AccessToken);
        }

        private async Task ErrorHandlingAsync(HttpResponseMessage response, TraktHttpRequestMessage requestMessage, bool isCheckinRequest = false)
        {
            var responseContent = string.Empty;

            if (response.Content != null)
                responseContent = await response.Content.ReadAsStringAsync();

            var code = response.StatusCode;

            //switch (code)
            //{
            //    case HttpStatusCode.NotFound:
            //        {
            //            if (RequestObjectType.HasValue)
            //            {
            //                switch (RequestObjectType.Value)
            //                {
            //                    case TraktRequestObjectType.Episodes:
            //                        throw new TraktEpisodeNotFoundException(Id, Season, Episode)
            //                        {
            //                            RequestUrl = Url,
            //                            RequestBody = RequestBodyJson,
            //                            Response = responseContent,
            //                            ServerReasonPhrase = response.ReasonPhrase
            //                        };
            //                    case TraktRequestObjectType.Seasons:
            //                        throw new TraktSeasonNotFoundException(Id, Season)
            //                        {
            //                            RequestUrl = Url,
            //                            RequestBody = RequestBodyJson,
            //                            Response = responseContent,
            //                            ServerReasonPhrase = response.ReasonPhrase
            //                        };
            //                    case TraktRequestObjectType.Shows:
            //                        throw new TraktShowNotFoundException(Id)
            //                        {
            //                            RequestUrl = Url,
            //                            RequestBody = RequestBodyJson,
            //                            Response = responseContent,
            //                            ServerReasonPhrase = response.ReasonPhrase
            //                        };
            //                    case TraktRequestObjectType.Movies:
            //                        throw new TraktMovieNotFoundException(Id)
            //                        {
            //                            RequestUrl = Url,
            //                            RequestBody = RequestBodyJson,
            //                            Response = responseContent,
            //                            ServerReasonPhrase = response.ReasonPhrase
            //                        };
            //                    case TraktRequestObjectType.People:
            //                        throw new TraktPersonNotFoundException(Id)
            //                        {
            //                            RequestUrl = Url,
            //                            RequestBody = RequestBodyJson,
            //                            Response = responseContent,
            //                            ServerReasonPhrase = response.ReasonPhrase
            //                        };
            //                    case TraktRequestObjectType.Comments:
            //                        throw new TraktCommentNotFoundException(Id)
            //                        {
            //                            RequestUrl = Url,
            //                            RequestBody = RequestBodyJson,
            //                            Response = responseContent,
            //                            ServerReasonPhrase = response.ReasonPhrase
            //                        };
            //                    case TraktRequestObjectType.Lists:
            //                        throw new TraktListNotFoundException(Id)
            //                        {
            //                            RequestUrl = Url,
            //                            RequestBody = RequestBodyJson,
            //                            Response = responseContent,
            //                            ServerReasonPhrase = response.ReasonPhrase
            //                        };
            //                    default:
            //                        throw new TraktObjectNotFoundException(Id)
            //                        {
            //                            RequestUrl = Url,
            //                            RequestBody = RequestBodyJson,
            //                            Response = responseContent,
            //                            ServerReasonPhrase = response.ReasonPhrase
            //                        };
            //                }
            //            }

            //            throw new TraktNotFoundException($"Resource not found - Reason Phrase: {response.ReasonPhrase}");
            //        }
            //    case HttpStatusCode.BadRequest:
            //        throw new TraktBadRequestException()
            //        {
            //            RequestUrl = Url,
            //            RequestBody = RequestBodyJson,
            //            Response = responseContent,
            //            ServerReasonPhrase = response.ReasonPhrase
            //        };
            //    case HttpStatusCode.Unauthorized:
            //        throw new TraktAuthorizationException()
            //        {
            //            RequestUrl = Url,
            //            RequestBody = RequestBodyJson,
            //            Response = responseContent,
            //            ServerReasonPhrase = response.ReasonPhrase
            //        };
            //    case HttpStatusCode.Forbidden:
            //        throw new TraktForbiddenException()
            //        {
            //            RequestUrl = Url,
            //            RequestBody = RequestBodyJson,
            //            Response = responseContent,
            //            ServerReasonPhrase = response.ReasonPhrase
            //        };
            //    case HttpStatusCode.MethodNotAllowed:
            //        throw new TraktMethodNotFoundException()
            //        {
            //            RequestUrl = Url,
            //            RequestBody = RequestBodyJson,
            //            Response = responseContent,
            //            ServerReasonPhrase = response.ReasonPhrase
            //        };
            //    case HttpStatusCode.Conflict:
            //        if (isCheckinRequest)
            //        {
            //            TraktCheckinPostErrorResponse errorResponse = null;

            //            if (!string.IsNullOrEmpty(responseContent))
            //                errorResponse = Json.Deserialize<TraktCheckinPostErrorResponse>(responseContent);

            //            throw new TraktCheckinException("checkin is already in progress")
            //            {
            //                RequestUrl = Url,
            //                RequestBody = RequestBodyJson,
            //                Response = responseContent,
            //                ServerReasonPhrase = response.ReasonPhrase,
            //                ExpiresAt = errorResponse?.ExpiresAt
            //            };
            //        }

            //        throw new TraktConflictException()
            //        {
            //            RequestUrl = Url,
            //            RequestBody = RequestBodyJson,
            //            Response = responseContent,
            //            ServerReasonPhrase = response.ReasonPhrase
            //        };
            //    case HttpStatusCode.InternalServerError:
            //        throw new TraktServerException()
            //        {
            //            RequestUrl = Url,
            //            RequestBody = RequestBodyJson,
            //            Response = responseContent,
            //            ServerReasonPhrase = response.ReasonPhrase
            //        };
            //    case HttpStatusCode.BadGateway:
            //        throw new TraktBadGatewayException()
            //        {
            //            RequestUrl = Url,
            //            RequestBody = RequestBodyJson,
            //            Response = responseContent,
            //            ServerReasonPhrase = response.ReasonPhrase
            //        };
            //    case (HttpStatusCode)412:
            //        throw new TraktPreconditionFailedException()
            //        {
            //            RequestUrl = Url,
            //            RequestBody = RequestBodyJson,
            //            Response = responseContent,
            //            ServerReasonPhrase = response.ReasonPhrase
            //        };
            //    case (HttpStatusCode)422:
            //        throw new TraktValidationException()
            //        {
            //            RequestUrl = Url,
            //            RequestBody = RequestBodyJson,
            //            Response = responseContent,
            //            ServerReasonPhrase = response.ReasonPhrase
            //        };
            //    case (HttpStatusCode)429:
            //        throw new TraktRateLimitException()
            //        {
            //            RequestUrl = Url,
            //            RequestBody = RequestBodyJson,
            //            Response = responseContent,
            //            ServerReasonPhrase = response.ReasonPhrase
            //        };
            //    case (HttpStatusCode)503:
            //    case (HttpStatusCode)504:
            //        throw new TraktServerUnavailableException("Service Unavailable - server overloaded (try again in 30s)")
            //        {
            //            RequestUrl = Url,
            //            RequestBody = RequestBodyJson,
            //            StatusCode = HttpStatusCode.ServiceUnavailable,
            //            Response = responseContent,
            //            ServerReasonPhrase = response.ReasonPhrase
            //        };
            //    case (HttpStatusCode)520:
            //    case (HttpStatusCode)521:
            //    case (HttpStatusCode)522:
            //        throw new TraktServerUnavailableException("Service Unavailable - Cloudflare error")
            //        {
            //            RequestUrl = Url,
            //            RequestBody = RequestBodyJson,
            //            StatusCode = HttpStatusCode.ServiceUnavailable,
            //            Response = responseContent,
            //            ServerReasonPhrase = response.ReasonPhrase
            //        };
            //}

            //TraktError error = null;

            //try
            //{
            //    error = Json.Deserialize<TraktError>(responseContent);
            //}
            //catch (Exception ex)
            //{
            //    throw new TraktException("json convert exception", ex);
            //}

            //var errorMessage = (error == null || string.IsNullOrEmpty(error.Description))
            //                        ? $"Trakt API error without content. Response status code was {(int)code}"
            //                        : error.Description;

            //throw new TraktException(errorMessage)
            //{
            //    RequestUrl = Url,
            //    RequestBody = RequestBodyJson,
            //    Response = responseContent,
            //    ServerReasonPhrase = response.ReasonPhrase
            //};
        }
    }
}
