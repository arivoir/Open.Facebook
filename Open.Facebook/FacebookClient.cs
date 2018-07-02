using Open.IO;
using Open.Net.Http;
using Open.OAuth2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Open.Facebook
{
    public class FacebookClient : OAuth2Client
    {
        #region ** fields

        private static readonly string ApiServiceUri = "https://graph.facebook.com/v2.6/";
        private static readonly string OAuthUri = "https://graph.facebook.com/v2.6/oauth/access_token";
        private string _accessToken = null;

        #endregion

        #region ** initialization

        public FacebookClient(string accessToken)
        {
            _accessToken = accessToken;
        }

        #endregion

        #region ** authentication

        public static string GetRequestUrl(string clientId, string scope, string callbackUrl = "https://www.facebook.com/connect/login_success.html", string display = null, string auth_type = null)
        {
            return OAuth2Client.GetRequestUrl("https://www.facebook.com/dialog/oauth", clientId, scope, callbackUrl, "token", new Dictionary<string, string> { { "display", display }, { "auth_type", auth_type } });
        }

        public static async Task<OAuth2Token> ExchangeCodeForAccessTokenAsync(string code, string clientId, string clientSecret)
        {

            var uri = new Uri(OAuthUri);
            var client = new HttpClient();
            var entry = string.Format(@"client_id={1}&client_secret={2}&grant_type=fb_exchange_token&fb_exchange_token={0}",
                                        code,
                                        clientId,
                                        clientSecret);
            var content = new StringContent(entry);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            var response = await client.PostAsync(uri, content);
            if (response.IsSuccessStatusCode)
            {
                var text = await response.Content.ReadAsStringAsync();
                var fragments = text.Split('&');
                var accessToken = fragments[0].Split('=')[1];
                var expires = int.Parse(fragments[1].Split('=')[1]);
                return new OAuth2Token() { AccessToken = accessToken, ExpiresIn = expires };
            }
            else
            {
                throw await ProcessException(response.Content);
            }
        }

        /// <summary>
        /// Build the API service URI.
        /// </summary>
        /// <param name="path">The relative path requested.</param>
        /// <returns>The request URI.</returns>
        private Uri BuildApiUri(string path, IDictionary<string, string> parameters = null)
        {
            UriBuilder builder = new UriBuilder(ApiServiceUri);
            builder.Path += path;
            builder.Query = (parameters != null && parameters.Count() > 0 ? string.Join("&", parameters.Select(pair => pair.Key + "=" + Uri.EscapeDataString(pair.Value)).ToArray()) + "&" : "") + "access_token=" + Uri.EscapeUriString(_accessToken);
            return builder.Uri;
        }

        #endregion

        #region ** public methods

        public async Task<User> GetUserInfoAsync(string fields = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var parameters = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(fields))
                parameters["fields"] = fields;

            var uri = BuildApiUri("me", parameters);
            var client = CreateClient();
            var response = await client.GetAsync(uri, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadJsonAsync<User>();
            }
            else
            {
                throw await ProcessException(response.Content);
            }
        }

        public async Task<AlbumList> GetAlbumsAsync(string fields = null, int? offset = null, int? limit = null, string after = null, string before = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var parameters = new Dictionary<string, string>();
            if (offset.HasValue)
                parameters["offset"] = offset.ToString();
            if (limit.HasValue)
                parameters["limit"] = limit.ToString();
            if (!string.IsNullOrWhiteSpace(fields))
                parameters["fields"] = fields;
            if (!string.IsNullOrWhiteSpace(after))
                parameters["after"] = after;
            if (!string.IsNullOrWhiteSpace(before))
                parameters["before"] = before;

            var uri = BuildApiUri("me/albums", parameters);

            var client = CreateClient();
            var response = await client.GetAsync(uri, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadJsonAsync<AlbumList>();
            }
            else
            {
                throw await ProcessException(response.Content);
            }
        }

        public async Task<Album> GetAlbumAsync(string albumId, string fields = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var parameters = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(fields))
                parameters["fields"] = fields;
            var uri = BuildApiUri(albumId, parameters);
            var client = CreateClient();
            var response = await client.GetAsync(uri, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadJsonAsync<Album>();
            }
            else
            {
                throw await ProcessException(response.Content);
            }
        }

        public async Task<Photo> GetPhotoAsync(string photoId, string fields = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var parameters = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(fields))
                parameters["fields"] = fields;
            var uri = BuildApiUri(photoId, parameters);
            var client = CreateClient();
            var response = await client.GetAsync(uri, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadJsonAsync<Photo>();
            }
            else
            {
                throw await ProcessException(response.Content);
            }
        }

        public async Task<PhotoList> GetAlbumPhotosAsync(string albumId, string fields = null, int? offset = null, int? limit = null, string after = null, string before = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var parameters = new Dictionary<string, string>();
            if (offset.HasValue)
                parameters["offset"] = offset.ToString();
            if (limit.HasValue)
                parameters["limit"] = limit.ToString();
            if (!string.IsNullOrWhiteSpace(fields))
                parameters["fields"] = fields;
            if (!string.IsNullOrWhiteSpace(after))
                parameters["after"] = after;
            if (!string.IsNullOrWhiteSpace(before))
                parameters["before"] = before;
            var uri = BuildApiUri(albumId + "/photos", parameters);
            var client = CreateClient();
            var response = await client.GetAsync(uri, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadJsonAsync<PhotoList>();
            }
            else
            {
                throw await ProcessException(response.Content);
            }
        }

        public async Task<PhotoList> GetPhotosAsync(string fields = null, int? limit = null, string after = null, string type = "tagged", CancellationToken cancellationToken = default(CancellationToken))
        {
            var parameters = new Dictionary<string, string>();
            if (limit.HasValue)
                parameters["limit"] = limit.ToString();
            if (!string.IsNullOrWhiteSpace(fields))
                parameters["fields"] = fields;
            if (!string.IsNullOrWhiteSpace(after))
                parameters["after"] = after;
            if (!string.IsNullOrWhiteSpace(type))
                parameters["type"] = type;
            var uri = BuildApiUri("me/photos", parameters);
            var client = CreateClient();
            var response = await client.GetAsync(uri, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadJsonAsync<PhotoList>();
            }
            else
            {
                throw await ProcessException(response.Content);
            }
        }

        public async Task<VideoList> GetVideosAsync(string fields = null, int? limit = null, string after = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var parameters = new Dictionary<string, string>();
            if (limit.HasValue)
                parameters["limit"] = limit.ToString();
            if (!string.IsNullOrWhiteSpace(fields))
                parameters["fields"] = fields;
            if (!string.IsNullOrWhiteSpace(after))
                parameters["after"] = after;
            var uri = BuildApiUri("me/videos/uploaded", parameters);
            var client = CreateClient();
            var response = await client.GetAsync(uri, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadJsonAsync<VideoList>();
            }
            else
            {
                throw await ProcessException(response.Content);
            }
        }

        public async Task<IList<Comment>> GetCommentsAsync(string photoId, string fields = null, int? offset = null, int? limit = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var parameters = new Dictionary<string, string>();
            if (offset.HasValue)
                parameters["offset"] = offset.ToString();
            if (limit.HasValue)
                parameters["limit"] = limit.ToString();
            if (!string.IsNullOrWhiteSpace(fields))
                parameters["fields"] = fields;
            var uri = BuildApiUri(photoId + "/comments", parameters);
            var client = CreateClient();
            var response = await client.GetAsync(uri, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return (await response.Content.ReadJsonAsync<FacebookComments>()).Data;
            }
            else
            {
                throw await ProcessException(response.Content);
            }
        }

        public async Task<IList<Like>> GetLikesAsync(string photoId, string fields = null, int? offset = null, int? limit = null, CancellationToken cancellationToken = default(CancellationToken))
        {

            var parameters = new Dictionary<string, string>();
            if (offset.HasValue)
                parameters["offset"] = offset.ToString();
            if (limit.HasValue)
                parameters["limit"] = limit.ToString();
            if (!string.IsNullOrWhiteSpace(fields))
                parameters["fields"] = fields;
            var uri = BuildApiUri(photoId + "/likes", parameters);
            var client = CreateClient();
            var response = await client.GetAsync(uri, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return (await response.Content.ReadJsonAsync<FacebookLikes>()).Data;
            }
            else
            {
                throw await ProcessException(response.Content);
            }
        }

        public async Task<IList<Album>> SearchAsync(string searchCriteria, CancellationToken cancellationToken = default(CancellationToken))
        {
            var parameters = new Dictionary<string, string>
            {
                { "q", searchCriteria },
            };

            var uri = BuildApiUri("me/photos", parameters);
            var client = CreateClient();
            var response = await client.GetAsync(uri, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return (await response.Content.ReadJsonAsync<AlbumList>()).Data;
            }
            else
            {
                throw await ProcessException(response.Content);
            }
        }

        public async Task<Album> CreateAlbumAsync(string name, string message = null, string privacy = null, string fields = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var parameters = new Dictionary<string, string>
                {
                  { "name", name ?? ""},
                  { "privacy", @"{""value"":""" + privacy + @"""}"},
                  { "fields", fields}
                };
            var uri = BuildApiUri("me/albums", parameters);
            var client = CreateClient();
            var response = await client.PostAsync(uri, new StreamContent(new MemoryStream()), cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadJsonAsync<Album>();
            }
            else
            {
                throw await ProcessException(response.Content);
            }
        }

        public async Task<Stream> DownloadPhotoAsync(Uri fileDownloadUri, CancellationToken cancellationToken)
        {
            var client = CreateClient();
            var response = await client.GetAsync(fileDownloadUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                if (response.Content.Headers.ContentLength.HasValue)
                    return new StreamWithLength(await response.Content.ReadAsStreamAsync(), response.Content.Headers.ContentLength);
                else
                    return await response.Content.ReadAsStreamAsync();
            }
            else
            {
                throw await ProcessException(response.Content);
            }
        }

        public async Task<UploadedPhoto> UploadPhotoAsync(string albumId, string message, Stream fileStream, IProgress<StreamProgress> progress, CancellationToken cancellationToken)
        {
            var parameters = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(message))
                parameters.Add("message", message);
            var uri = BuildApiUri(string.Format("{0}/photos", albumId), parameters);
            var client = CreateClient();
            var content = new MultipartFormDataContent();
            content.Add(new StreamedContent(fileStream, progress, cancellationToken), "file", "file.jpeg");//"image/jpeg"
            var response = await client.PostAsync(uri, content, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadJsonAsync<UploadedPhoto>();
            }
            else
            {
                throw await ProcessException(response.Content);
            }
        }

        public async Task<Comment> AddCommentAsync(string photoId, string message, string fields = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var parameters = new Dictionary<string, string>();
            if (message != null)
                parameters["message"] = message;
            if (!string.IsNullOrWhiteSpace(fields))
                parameters["fields"] = fields;
            var uri = BuildApiUri(photoId + "/comments", parameters);
            var client = CreateClient();
            var response = await client.PostAsync(uri, new StreamContent(new MemoryStream()), cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadJsonAsync<Comment>();
            }
            else
            {
                throw await ProcessException(response.Content);
            }
        }

        public async Task<bool> AddLikeAsync(string photoId, string fields = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var parameters = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(fields))
                parameters["fields"] = fields;
            var uri = BuildApiUri(photoId + "/likes", parameters);
            var client = CreateClient();
            var response = await client.PostAsync(uri, new StreamContent(new MemoryStream()), cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return (await response.Content.ReadJsonAsync<Result>()).Success;
            }
            else
            {
                throw await ProcessException(response.Content);
            }
        }

        public async Task<bool> RemoveLikeAsync(string photoId, string fields = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var parameters = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(fields))
                parameters["fields"] = fields;
            var uri = BuildApiUri(photoId + "/likes", parameters);
            var client = CreateClient();
            var response = await client.DeleteAsync(uri, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return (await response.Content.ReadJsonAsync<Result>()).Success;
            }
            else
            {
                throw await ProcessException(response.Content);
            }
        }

        public async Task<bool> DeleteResourceByIdAsync(string resourceId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var parameters = new Dictionary<string, string>();
            var uri = BuildApiUri(resourceId, parameters);
            var client = CreateClient();
            var response = await client.DeleteAsync(uri, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return (await response.Content.ReadJsonAsync<Result>()).Success;
            }
            else
            {
                throw await ProcessException(response.Content);
            }
        }

        #endregion

        #region ** private stuff

        private static HttpClient CreateClient()
        {
            var client = new HttpClient(new RetryMessageHandler());
            client.Timeout = Timeout.InfiniteTimeSpan;
            return client;
        }

        private static async Task<Exception> ProcessException(HttpContent content)
        {
            var error = await content.ReadJsonAsync<ErrorResponse>();
            return new FacebookException(error.Error);
        }

        #endregion
    }

    [DataContract]
    public class AlbumList
    {
        [DataMember(Name = "data")]
        public List<Album> Data { get; set; }
        [DataMember(Name = "paging")]
        public Paging Paging { get; set; }
    }

    [DataContract]
    public class PhotoList
    {
        [DataMember(Name = "data")]
        public List<Photo> Data { get; set; }
        [DataMember(Name = "paging")]
        public Paging Paging { get; set; }
    }

    [DataContract]
    public class VideoList
    {
        [DataMember(Name = "data")]
        public List<Video> Data { get; set; }
        [DataMember(Name = "paging")]
        public Paging Paging { get; set; }
    }


    //[DataContract]
    //public class FacebookPhotos
    //{
    //    [DataMember(Name = "data")]
    //    public List<Photo> Data { get; set; }
    //}

    [DataContract]
    public class FacebookComments
    {
        [DataMember(Name = "data")]
        public List<Comment> Data { get; set; }
    }

    [DataContract]
    public class FacebookLikes
    {
        [DataMember(Name = "data")]
        public List<Like> Data { get; set; }
    }

    [DataContract]
    public class Paging
    {
        [DataMember(Name = "cursors")]
        public Cursors Cursors { get; set; }
        [DataMember(Name = "next")]
        public string Next { get; set; }
        [DataMember(Name = "previous")]
        public string Previous { get; set; }
    }

    [DataContract]
    public class Cursors
    {
        [DataMember(Name = "after")]
        public string After { get; set; }
        [DataMember(Name = "before")]
        public string Before { get; set; }
    }
}
