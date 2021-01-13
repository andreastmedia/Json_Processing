using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace JsonMethods
{
    /// <summary>
    /// The main <see langword="class"/> that processes Json files from the web.
    /// </summary>
    public class JsonWebProcessor
    {
        /// <summary>
        /// Loads a Json file from the web asynchronously. 
        /// </summary>
        /// <param name="httpClient">The base <see langword="class"/> that sends HTTP requests and receives HTTP responses from a resource identified by a URI.</param>
        /// <param name="uri">The URI that identifies the <see cref="HttpClient"/>.</param>
        /// <returns>
        /// A <see cref="Task{}"/> of type <see cref="object"/>, if <see cref="HttpResponseMessage"/> was successful.<br/>
        /// Returns <see langword="null"/> otherwise.
        /// </returns>
        public async Task<object> LoadJsonFromWeb(HttpClient httpClient, string uri)
        {
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(uri);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string response = await httpResponseMessage.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject(response);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Loads a Json file from the web asynchronously.
        /// </summary>
        /// <remarks>
        /// This method is used when you have a base URI and a list of endpoints.
        /// </remarks>
        /// <param name="httpClient">The base <see langword="class"/> that sends HTTP requests and receives HTTP responses from a resource identified by a URI.</param>
        /// <param name="uri">The URI that identifies the <see cref="HttpClient"/>.</param>
        /// <param name="id">The id that will be used as an endpoint.</param>
        /// <returns>
        /// A<see cref="Task{}"/> of type<see cref="object"/>, if <see cref = "HttpResponseMessage" /> was successful.<br/>
        /// Returns <see langword="null"/> otherwise.
        /// </returns>
        public async Task<object> LoadJsonFromWeb(HttpClient httpClient, string uri, string id)
        {
            string uriCombined = uri + "/" + id;

            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(uriCombined);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string response = await httpResponseMessage.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject(response);
            }
            else
            {
                return null;
            }
        }
    }
}