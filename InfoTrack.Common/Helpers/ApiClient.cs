using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace InfoTrack.Common.Helpers
{
    public class ApiClient
    {
        public ApiClient() 
        {
            _httpClient = new HttpClient();
        }

        public async Task<T> Post<T>(string uri, object data)
        {
            var response = await _httpClient.PostAsJsonAsync(uri, data);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }

            var responseStream = await response.Content.ReadAsStreamAsync();

            return await JsonSerializer.DeserializeAsync<T>(responseStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        private HttpClient _httpClient;
    }
}
