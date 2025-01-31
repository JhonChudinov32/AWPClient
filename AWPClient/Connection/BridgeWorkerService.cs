using AWPClient.Classes;
using System.Net.Http;
using System.Threading.Tasks;

namespace AWPClient.Connection
{
    public class BridgeWorkerService
    {
        private HttpClient _httpClient;

        /// <summary>
        /// Создаем экземпляр объекта httpClient
        /// </summary>
        public BridgeWorkerService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("ID", Setter.ID);
        }
        /// <summary>
        ///Асинхронные методы POST и GET
        /// </summary>
        public async Task<string> GetAsync(string url)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        public async Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
        {
            return await _httpClient.PostAsync(url, content);
        }

        /// <summary>
        /// Не асинхронные методы POST и GET
        /// </summary>
        public string GetJsonData(string url)
        {
            HttpResponseMessage response = _httpClient.GetAsync(url).GetAwaiter().GetResult();
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        }
        public HttpResponseMessage PostJsonData(string url, HttpContent content)
        {
            return _httpClient.PostAsync(url, content).GetAwaiter().GetResult();
        }
    }
}
