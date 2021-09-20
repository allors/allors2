// <copyright file="RemoteDatabase.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Remote.SystemText
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Allors.Protocol.Json;
    using Allors.Protocol.Json.Api.Invoke;
    using Allors.Protocol.Json.Api.Pull;
    using Allors.Protocol.Json.Api.Push;
    using Allors.Protocol.Json.Api.Security;
    using Allors.Protocol.Json.Api.Sync;
    using Allors.Protocol.Json.Auth;
    using Allors.Protocol.Json.SystemTextJson;
    using Ranges;
    using Polly;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "RCS1090:Add call to 'ConfigureAwait' (or vice versa).", Justification = "<Pending>")]
    public class DatabaseConnection : Remote.DatabaseConnection
    {
        private string userId;

        public DatabaseConnection(Configuration configuration, Func<IWorkspaceServices> servicesBuilder, HttpClient httpClient, IdGenerator idGenerator, IRanges<long> ranges) : base(configuration, idGenerator, servicesBuilder, ranges)
        {
            this.HttpClient = httpClient;
            this.HttpClient.DefaultRequestHeaders.Accept.Clear();
            this.HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            this.UnitConvert = new UnitConvert();
        }

        public override IUnitConvert UnitConvert { get; }

        protected override string UserId => this.userId;

        public IAsyncPolicy Policy { get; set; } = Polly.Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        public HttpClient HttpClient { get; }

        public async Task<bool> Login(Uri url, string username, string password)
        {
            var request = new AuthenticationTokenRequest { l = username, p = password };
            using var response = await this.PostAsJsonAsync(url, request);
            response.EnsureSuccessStatusCode();
            var authResult = await this.ReadAsAsync<AuthenticationTokenResponse>(response);
            if (!authResult.a)
            {
                return false;
            }

            this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.t);
            this.userId = authResult.u;

            return true;
        }

        public override async Task<PullResponse> Pull(PullRequest pullRequest)
        {
            var uri = new Uri("pull", UriKind.Relative);
            var response = await this.PostAsJsonAsync(uri, pullRequest);
            response.EnsureSuccessStatusCode();
            return await this.ReadAsAsync<PullResponse>(response);
        }

        public override async Task<SyncResponse> Sync(SyncRequest syncRequest)
        {
            var uri = new Uri("sync", UriKind.Relative);
            var response = await this.PostAsJsonAsync(uri, syncRequest);
            response.EnsureSuccessStatusCode();

            return await this.ReadAsAsync<SyncResponse>(response);
        }

        public override async Task<PushResponse> Push(PushRequest pushRequest)
        {
            var uri = new Uri("push", UriKind.Relative);
            var response = await this.PostAsJsonAsync(uri, pushRequest);
            response.EnsureSuccessStatusCode();

            return await this.ReadAsAsync<PushResponse>(response);
        }

        public override async Task<InvokeResponse> Invoke(InvokeRequest invokeRequest)
        {
            var uri = new Uri("invoke", UriKind.Relative);
            var response = await this.PostAsJsonAsync(uri, invokeRequest);
            response.EnsureSuccessStatusCode();

            return await this.ReadAsAsync<InvokeResponse>(response);
        }

        public override async Task<AccessResponse> Access(AccessRequest accessRequest)
        {
            var uri = new Uri("access", UriKind.Relative);
            var response = await this.PostAsJsonAsync(uri, accessRequest);
            response.EnsureSuccessStatusCode();

            return await this.ReadAsAsync<AccessResponse>(response);
        }

        public override async Task<PermissionResponse> Permission(PermissionRequest permissionRequest)
        {
            var uri = new Uri("permission", UriKind.Relative);
            var response = await this.PostAsJsonAsync(uri, permissionRequest);
            response.EnsureSuccessStatusCode();

            return await this.ReadAsAsync<PermissionResponse>(response);
        }

        private async Task<HttpResponseMessage> PostAsJsonAsync(Uri uri, object args) =>
            await this.Policy.ExecuteAsync(
                async () =>
                {
                    // TODO: use SerializeToUtf8Bytes()
                    var json = JsonSerializer.Serialize(args);
                    return await this.HttpClient.PostAsync(
                        uri,
                        new StringContent(json, Encoding.UTF8, "application/json"));
                });

        private async Task<T> ReadAsAsync<T>(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
