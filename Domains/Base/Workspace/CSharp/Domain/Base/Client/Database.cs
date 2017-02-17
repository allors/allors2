namespace Allors.Workspace.Client
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;

    using Data;

    public class Database
    {
        public Database(HttpClient httpClient)
        {
            this.HttpClient = httpClient;

            this.HttpClient.DefaultRequestHeaders.Accept.Clear();
            this.HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        ~Database()
        {
            this.HttpClient.Dispose();
        }

        public HttpClient HttpClient { get; }

        public async Task<PullResponse> Pull(string name, object args)
        {
            var uri = new Uri(name + "/pull", UriKind.Relative);
            var response = await this.HttpClient.PostAsJsonAsync(uri, args);
            response.EnsureSuccessStatusCode();

            var pullResponse = await response.Content.ReadAsAsync<PullResponse>();
            return pullResponse;
        }

        public async Task<SyncResponse> Sync(SyncRequest syncRequest)
        {
            var uri = new Uri("Database/Sync", UriKind.Relative);
            var response = await this.HttpClient.PostAsJsonAsync(uri, syncRequest);
            response.EnsureSuccessStatusCode();

            var syncResponse = await response.Content.ReadAsAsync<SyncResponse>();
            return syncResponse;
        }

        public async Task<PushResponse> Push(PushRequest pushRequest)
        {
            var uri = new Uri("Database/Push", UriKind.Relative);
            var response = await this.HttpClient.PostAsJsonAsync(uri, pushRequest);
            response.EnsureSuccessStatusCode();

            var pushResponse = await response.Content.ReadAsAsync<PushResponse>();
            return pushResponse;
        }

        public async Task<InvokeResponse> Invoke(Method method)
        {
            var invokeRequest = new InvokeRequest
            {
                i = method.Object.Id.ToString(),
                v = method.Object.Version.ToString(),
                m = method.Name,
            };

            var uri = new Uri("Database/Invoke", UriKind.Relative);
            var response = await this.HttpClient.PostAsJsonAsync(uri, invokeRequest);
            response.EnsureSuccessStatusCode();

            var invokeResponse = await response.Content.ReadAsAsync<InvokeResponse>();
            return invokeResponse;
        }

        public async Task<InvokeResponse> Invoke(string service, object args)
        {
            var uri = new Uri(service, UriKind.Relative);
            var response = await this.HttpClient.PostAsJsonAsync(uri, args);
            response.EnsureSuccessStatusCode();

            var invokeResponse = await response.Content.ReadAsAsync<InvokeResponse>();
            return invokeResponse;
        }
    }
}