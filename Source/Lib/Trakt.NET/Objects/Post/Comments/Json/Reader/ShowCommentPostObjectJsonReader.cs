﻿namespace TraktNet.Objects.Post.Comments.Json.Reader
{
    using Basic.Json.Reader;
    using Get.Shows.Json.Reader;
    using Newtonsoft.Json;
    using Objects.Json;
    using System.Threading;
    using System.Threading.Tasks;

    internal class ShowCommentPostObjectJsonReader : AObjectJsonReader<ITraktShowCommentPost>
    {
        public override async Task<ITraktShowCommentPost> ReadObjectAsync(JsonTextReader jsonReader, CancellationToken cancellationToken = default)
        {
            if (jsonReader == null)
                return await Task.FromResult(default(ITraktShowCommentPost));

            if (await jsonReader.ReadAsync(cancellationToken) && jsonReader.TokenType == JsonToken.StartObject)
            {
                var sharingReader = new SharingObjectJsonReader();
                var showReader = new ShowObjectJsonReader();
                ITraktShowCommentPost showCommentPost = new TraktShowCommentPost();

                while (await jsonReader.ReadAsync(cancellationToken) && jsonReader.TokenType == JsonToken.PropertyName)
                {
                    var propertyName = jsonReader.Value.ToString();

                    switch (propertyName)
                    {
                        case JsonProperties.COMMENT_POST_PROPERTY_NAME_COMMENT:
                            showCommentPost.Comment = await jsonReader.ReadAsStringAsync(cancellationToken);
                            break;
                        case JsonProperties.COMMENT_POST_PROPERTY_NAME_SPOILER:
                            {
                                bool? value = await jsonReader.ReadAsBooleanAsync(cancellationToken);

                                if (value.HasValue)
                                    showCommentPost.Spoiler = value.Value;

                                break;
                            }
                        case JsonProperties.COMMENT_POST_PROPERTY_NAME_SHARING:
                            showCommentPost.Sharing = await sharingReader.ReadObjectAsync(jsonReader, cancellationToken);
                            break;
                        case JsonProperties.SHOW_COMMENT_POST_PROPERTY_NAME_SHOW:
                            showCommentPost.Show = await showReader.ReadObjectAsync(jsonReader, cancellationToken);
                            break;
                        default:
                            await JsonReaderHelper.ReadAndIgnoreInvalidContentAsync(jsonReader, cancellationToken);
                            break;
                    }
                }

                return showCommentPost;
            }

            return await Task.FromResult(default(ITraktShowCommentPost));
        }
    }
}
