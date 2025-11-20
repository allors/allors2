// <copyright file="IDatabase.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Remote
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Allors.Protocol.Remote.Invoke;
    using Allors.Protocol.Remote.Pull;
    using Allors.Protocol.Remote.Push;
    using Allors.Protocol.Remote.Sync;
    using Polly;
    using Protocol.Remote.Security;

    public class RemoteDatabase : IDatabase
    {
        public RemoteDatabase(HttpClient httpClient)
        {
            this.HttpClient = httpClient;

            this.HttpClient.DefaultRequestHeaders.Accept.Clear();
            this.HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        ~RemoteDatabase() => this.HttpClient.Dispose();

        public HttpClient HttpClient { get; }

        public string UserId { get; private set; }

        public IAsyncPolicy Policy { get; set; } = Polly.Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        public async Task<PullResponse> Pull(PullRequest pullRequest)
        {
            var uri = new Uri("pull", UriKind.Relative);
            var response = await this.PostAsJsonAsync(uri, pullRequest);
            response.EnsureSuccessStatusCode();
            var pullResponse = await this.ReadAsAsync<PullResponse>(response);
            return pullResponse;
        }

        public async Task<PullResponse> Pull(string name, object pullRequest)
        {
            var uri = new Uri(name + "/pull", UriKind.Relative);
            var response = await this.PostAsJsonAsync(uri, pullRequest);
            response.EnsureSuccessStatusCode();
            var pullResponse = await this.ReadAsAsync<PullResponse>(response);
            return pullResponse;
        }

        public async Task<SyncResponse> Sync(SyncRequest syncRequest)
        {
            var uri = new Uri("sync", UriKind.Relative);
            var response = await this.PostAsJsonAsync(uri, syncRequest);
            response.EnsureSuccessStatusCode();

            var syncResponse = await this.ReadAsAsync<SyncResponse>(response);
            return syncResponse;
        }

        public async Task<PushResponse> Push(PushRequest pushRequest)
        {
            var uri = new Uri("push", UriKind.Relative);
            var response = await this.PostAsJsonAsync(uri, pushRequest);
            response.EnsureSuccessStatusCode();

            var pushResponse = await this.ReadAsAsync<PushResponse>(response);
            return pushResponse;
        }

        public async Task<InvokeResponse> Invoke(InvokeRequest invokeRequest, InvokeOptions options = null)
        {
            var uri = new Uri("invoke", UriKind.Relative);
            var response = await this.PostAsJsonAsync(uri, invokeRequest);
            response.EnsureSuccessStatusCode();

            var invokeResponse = await this.ReadAsAsync<InvokeResponse>(response);
            return invokeResponse;
        }

        public async Task<InvokeResponse> Invoke(string service, object args)
        {
            var uri = new Uri(service + "pull", UriKind.Relative);
            var response = await this.PostAsJsonAsync(uri, args);
            response.EnsureSuccessStatusCode();

            var invokeResponse = await this.ReadAsAsync<InvokeResponse>(response);
            return invokeResponse;
        }

        public async Task<SecurityResponse> Security(SecurityRequest securityRequest)
        {
            var uri = new Uri("security", UriKind.Relative);
            var response = await this.PostAsJsonAsync(uri, securityRequest);
            response.EnsureSuccessStatusCode();

            var syncResponse = await this.ReadAsAsync<SecurityResponse>(response);
            return syncResponse;
        }

        public async Task<HttpResponseMessage> PostAsJsonAsync(Uri uri, object args) =>
            await this.Policy.ExecuteAsync(
                async () =>
                {
                    var json = JsonSerializer.Serialize(args);
                    return await this.HttpClient.PostAsync(
                        uri,
                        new StringContent(json, Encoding.UTF8, "application/json"));
                });

        public async Task<T> ReadAsAsync<T>(HttpResponseMessage response)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var json = await response.Content.ReadAsStringAsync();
            var deserializedObject = JsonSerializer.Deserialize<T>(json, options);
            return deserializedObject;
        }

        public async Task<bool> Login(Uri url, string username, string password)
        {
            var request = new { UserName = username, Password = password };
            using (var response = await this.PostAsJsonAsync(url, request))
            {
                response.EnsureSuccessStatusCode();
                var authResult = await this.ReadAsAsync<AuthenticationResult>(response);
                if (!authResult.Authenticated)
                {
                    return false;
                }

                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Token);
                this.UserId = authResult.UserId;

                return true;
            }
        }

    }
}
