using Newtonsoft.Json;
using Polly;
using Polly.Contrib.WaitAndRetry;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace JsonMethods
{
    /// <summary>
    /// The main <see langword="class"/> that processes Json files from the web.
    /// </summary>
    public static class JsonWebProcessor
    {
        /// <summary>
        /// Downloads a Json file from the web asynchronously. 
        /// </summary>
        /// <param name="httpClient">The base <see langword="class"/> that sends HTTP requests and receives HTTP responses from a resource identified by a URI.</param>
        /// <param name="uri">The URI that identifies the <see cref="HttpClient"/>.</param>
        /// <returns>
        /// A <see cref="Task{}"/> of type <see cref="object"/>, if <see cref="HttpResponseMessage"/> was successful.<br/>
        /// Throws <see cref = "Exception" /> otherwise.
        /// </returns>
        /// <exception cref="HttpResponseMessage.ReasonPhrase"/>
        public static async Task<object> DownloadJsonFromWeb(HttpClient httpClient, string uri)
        {
            HttpStatusCode[] httpStatusCodesWorthRetrying = 
                {
                    HttpStatusCode.RequestTimeout,      // 408
                    HttpStatusCode.InternalServerError, // 500
                    HttpStatusCode.BadGateway,          // 502
                    HttpStatusCode.ServiceUnavailable,  // 503
                    HttpStatusCode.GatewayTimeout       // 504
                };

            var delay = Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromSeconds(1), retryCount: 5);

            using (HttpResponseMessage httpResponseMessage = await Policy
                .Handle<HttpRequestException>()
                .Or<TaskCanceledException>()
                .Or<System.IO.IOException>()
                .Or<System.Net.Sockets.SocketException>()
                .OrResult<HttpResponseMessage>(r => httpStatusCodesWorthRetrying.Contains(r.StatusCode))
                .WaitAndRetryAsync(delay)
                .ExecuteAsync(() => httpClient.GetAsync(uri)))
            {
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string response = await httpResponseMessage.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject(response);
                }
                else
                {
                    throw new Exception(httpResponseMessage.ReasonPhrase);
                }
            }
        }

        /// <summary>
        /// Downloads a Json file from the web asynchronously.
        /// </summary>
        /// <remarks>
        /// This method is used when you have a base URI and a list of endpoints.
        /// </remarks>
        /// <param name="httpClient">The base <see langword="class"/> that sends HTTP requests and receives HTTP responses from a resource identified by a URI.</param>
        /// <param name="uri">The URI that identifies the <see cref="HttpClient"/>.</param>
        /// <param name="id">The id that will be used as an endpoint.</param>
        /// <returns>
        /// A<see cref="Task{}"/> of type<see cref="object"/>, if <see cref = "HttpResponseMessage" /> was successful.<br/>
        /// Throws <see cref = "Exception" /> otherwise.
        /// </returns>
        /// <exception cref="HttpResponseMessage.ReasonPhrase"/>
        public static async Task<object> DownloadJsonFromWeb(HttpClient httpClient, string uri, string id)
        {
            string uriCombined = uri + "/" + id;

            HttpStatusCode[] httpStatusCodesWorthRetrying =
                {
                    HttpStatusCode.RequestTimeout,      // 408
                    HttpStatusCode.InternalServerError, // 500
                    HttpStatusCode.BadGateway,          // 502
                    HttpStatusCode.ServiceUnavailable,  // 503
                    HttpStatusCode.GatewayTimeout       // 504
                };

            var delay = Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromSeconds(1), retryCount: 5);

            using (HttpResponseMessage httpResponseMessage = await Policy
                .Handle<HttpRequestException>()
                .Or<TaskCanceledException>()
                .Or<System.IO.IOException>()
                .Or<System.Net.Sockets.SocketException>()
                .OrResult<HttpResponseMessage>(r => httpStatusCodesWorthRetrying.Contains(r.StatusCode))
                .WaitAndRetryAsync(delay)
                .ExecuteAsync(() => httpClient.GetAsync(uriCombined)))
            {
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string response = await httpResponseMessage.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject(response);
                }
                else
                {
                    throw new Exception(httpResponseMessage.ReasonPhrase);
                }
            }
        }
    }
}