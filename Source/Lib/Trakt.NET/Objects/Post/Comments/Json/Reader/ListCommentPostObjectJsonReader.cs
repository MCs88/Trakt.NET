﻿namespace TraktNet.Objects.Post.Comments.Json.Reader
{
    using Basic.Json.Reader;
    using Get.Users.Lists.Json.Reader;
    using Newtonsoft.Json;
    using Objects.Json;
    using System.Threading;
    using System.Threading.Tasks;

    internal class ListCommentPostObjectJsonReader : AObjectJsonReader<ITraktListCommentPost>
    {
        public override async Task<ITraktListCommentPost> ReadObjectAsync(JsonTextReader jsonReader, CancellationToken cancellationToken = default)
        {
            if (jsonReader == null)
                return await Task.FromResult(default(ITraktListCommentPost));

            if (await jsonReader.ReadAsync(cancellationToken) && jsonReader.TokenType == JsonToken.StartObject)
            {
                var sharingReader = new SharingObjectJsonReader();
                var listReader = new ListObjectJsonReader();
                ITraktListCommentPost listCommentPost = new TraktListCommentPost();

                while (await jsonReader.ReadAsync(cancellationToken) && jsonReader.TokenType == JsonToken.PropertyName)
                {
                    var propertyName = jsonReader.Value.ToString();

                    switch (propertyName)
                    {
                        case JsonProperties.COMMENT_POST_PROPERTY_NAME_COMMENT:
                            listCommentPost.Comment = await jsonReader.ReadAsStringAsync(cancellationToken);
                            break;
                        case JsonProperties.COMMENT_POST_PROPERTY_NAME_SPOILER:
                            {
                                bool? value = await jsonReader.ReadAsBooleanAsync(cancellationToken);

                                if (value.HasValue)
                                    listCommentPost.Spoiler = value.Value;

                                break;
                            }
                        case JsonProperties.COMMENT_POST_PROPERTY_NAME_SHARING:
                            listCommentPost.Sharing = await sharingReader.ReadObjectAsync(jsonReader, cancellationToken);
                            break;
                        case JsonProperties.LIST_COMMENT_POST_PROPERTY_NAME_LIST:
                            listCommentPost.List = await listReader.ReadObjectAsync(jsonReader, cancellationToken);
                            break;
                        default:
                            await JsonReaderHelper.ReadAndIgnoreInvalidContentAsync(jsonReader, cancellationToken);
                            break;
                    }
                }

                return listCommentPost;
            }

            return await Task.FromResult(default(ITraktListCommentPost));
        }
    }
}
