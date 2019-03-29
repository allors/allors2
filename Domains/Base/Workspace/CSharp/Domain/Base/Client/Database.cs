namespace Allors.Workspace.Client
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using Polly;

    using Server;

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

        public IAsyncPolicy Policy { get; set; } = Polly.Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        public async Task<PullResponse> Pull(string name, object args)
        {
            var uri = new Uri(name + "/pull", UriKind.Relative);
            var response = await this.PostAsJsonAsync(uri, args);
            response.EnsureSuccessStatusCode();
            var pullResponse = await this.ReadAsAsync<PullResponse>(response);
            return pullResponse;
        }

        public async Task<SyncResponse> Sync(SyncRequest syncRequest)
        {
            var uri = new Uri("Database/Sync", UriKind.Relative);
            var response = await this.PostAsJsonAsync(uri, syncRequest);
            response.EnsureSuccessStatusCode();

            var syncResponse = await this.ReadAsAsync<SyncResponse>(response);
            return syncResponse;
        }

        public async Task<PushResponse> Push(PushRequest pushRequest)
        {
            var uri = new Uri("Database/Push", UriKind.Relative);
            var response = await this.PostAsJsonAsync(uri, pushRequest);
            response.EnsureSuccessStatusCode();

            var pushResponse = await this.ReadAsAsync<PushResponse>(response);
            return pushResponse;
        }

        public async Task<InvokeResponse> Invoke(Method method)
        {
            return await this.Invoke(new[] { method });
        }

        public async Task<InvokeResponse> Invoke(Method[] methods, InvokeOptions options = null)
        {
            var invokeRequest = new InvokeRequest
                                    {
                                        I = methods.Select(v => new Invocation
                                                                   {
                                                                       I = v.Object.Id.ToString(),
                                                                       V = v.Object.Version.ToString(),
                                                                       M = v.Name,
                                                                   }).ToArray(),
                                        O = options
                                    }; 
                

            var uri = new Uri("Database/Invoke", UriKind.Relative);
            var response = await this.PostAsJsonAsync(uri, invokeRequest);
            response.EnsureSuccessStatusCode();

            var invokeResponse = await this.ReadAsAsync<InvokeResponse>(response);
            return invokeResponse;
        }

        public async Task<InvokeResponse> Invoke(string service, object args)
        {
            var uri = new Uri(service + "/Pull", UriKind.Relative);
            var response = await this.PostAsJsonAsync(uri, args);
            response.EnsureSuccessStatusCode();

            var invokeResponse = await this.ReadAsAsync<InvokeResponse>(response);
            return invokeResponse;
        }

        public async Task<HttpResponseMessage> PostAsJsonAsync(Uri uri, object args)
        {
            return await this.Policy.ExecuteAsync(
                       async () =>
                           {
                               var json = JsonConvert.SerializeObject(args);
                               return await this.HttpClient.PostAsync(
                                          uri,
                                          new StringContent(json, Encoding.UTF8, "application/json"));
                           });
        }

        public async Task<T> ReadAsAsync<T>(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            var deserializedObject = JsonConvert.DeserializeObject<T>(json);
            return deserializedObject;
        }
    }
}