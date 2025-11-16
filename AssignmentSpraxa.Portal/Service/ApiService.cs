using AssignmentSpraxa.Portal.DTO;
using AssignmentSpraxa.Portal.Interface;
using System.Net.Http.Headers;

namespace AssignmentSpraxa.Portal.Service
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _client;

        public ApiService(HttpClient client)
        {
            _client = client;
        }

        public async Task<T> GetAsync<T>(string url)
        {
            var response = await _client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                throw new Exception(await response.Content.ReadAsStringAsync());

            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<T> PostAsync<T>(string url, object body)
        {
            var response = await _client.PostAsJsonAsync(url, body);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                try
                {
                    // Try to read model state errors
                    var problem = System.Text.Json.JsonSerializer.Deserialize<ApiErrorResponse>(content);

                    if (problem?.Errors != null && problem.Errors.Any())
                    {
                        var msg = string.Join("<br>", problem.Errors.SelectMany(e => e.Value));
                        throw new Exception(msg);
                    }
                }
                catch { }

                throw new Exception(content);
            }

            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<T> PutAsync<T>(string url, object body)
        {
            var response = await _client.PutAsJsonAsync(url, body);

            if (!response.IsSuccessStatusCode)
                throw new Exception(await response.Content.ReadAsStringAsync());

            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<bool> DeleteAsync(string url)
        {
            var response = await _client.DeleteAsync(url);

            if (!response.IsSuccessStatusCode)
                throw new Exception(await response.Content.ReadAsStringAsync());

            return true;
        }

        public void SetBearerToken(string token)
        {
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
