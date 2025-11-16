namespace AssignmentSpraxa.Portal.Interface
{
    public interface IApiService
    {
        Task<T> PostAsync<T>(string url, object body);

        Task<T> GetAsync<T>(string url);

        Task<T> PutAsync<T>(string url, object body);

        Task<bool> DeleteAsync(string url);

        void SetBearerToken(string token);
    }
}
