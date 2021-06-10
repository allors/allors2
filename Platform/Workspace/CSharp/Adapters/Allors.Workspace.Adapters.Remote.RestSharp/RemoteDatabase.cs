// <copyright file="IDatabase.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Remote
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using Allors.Protocol.Remote.Invoke;
    using Allors.Protocol.Remote.Pull;
    using Allors.Protocol.Remote.Push;
    using Allors.Protocol.Remote.Sync;
    using Polly;
    using Polly.Retry;
    using Protocol.Remote.Security;
    using RestSharp;

    public class RemoteDatabase : IDatabase
    {
        private readonly IRestClient initialRestClient;

        public RemoteDatabase(IRestClient restClient)
        {
            this.initialRestClient = restClient;
            this.RestClient = this.initialRestClient;
        }

        public IRestClient RestClient { get; private set; }

        public string UserId { get; private set; }

        public int[] SecondsBeforeRetry { get; set; } = { 1, 2, 4, 8, 16 };

        public async Task<PushResponse> Push(PushRequest pushRequest)
        {
            var uri = new Uri("sync", UriKind.Relative);
            // TODO: Retry for network errors, but not for server errors
            return await this.PostOnce<PushResponse>(uri, pushRequest);
        }

        public async Task<PullResponse> Pull(PullRequest pullRequest)
        {
            var uri = new Uri("pull", UriKind.Relative);
            return await this.Post<PullResponse>(uri, pullRequest);
        }

        public async Task<PullResponse> Pull(string name, object pullRequest)
        {
            var uri = new Uri(name + "/pull", UriKind.Relative);
            return await this.Post<PullResponse>(uri, pullRequest);
        }

        public async Task<SyncResponse> Sync(SyncRequest syncRequest)
        {
            var uri = new Uri("sync", UriKind.Relative);
            return await this.Post<SyncResponse>(uri, syncRequest);
        }

        public async Task<InvokeResponse> Invoke(InvokeRequest invokeRequest, InvokeOptions options = null)
        {
            var uri = new Uri("sync", UriKind.Relative);
            return await this.Post<InvokeResponse>(uri, invokeRequest);
        }

        public async Task<InvokeResponse> Invoke(string service, object args)
        {
            var uri = new Uri(service + "pull", UriKind.Relative);
            return await this.Post<InvokeResponse>(uri, args);
        }

        public async Task<SecurityResponse> Security(SecurityRequest securityRequest)
        {
            var uri = new Uri("security", UriKind.Relative);
            return await this.Post<SecurityResponse>(uri, securityRequest);
        }

        public async Task<bool> Login(Uri url, string username, string password)
        {
            var data = new { UserName = username, Password = password };
            var result = await this.Post<AuthenticationResult>(url, data);

            if (!result.Authenticated)
            {
                return false;
            }

            this.RestClient = this.RestClient.AddDefaultHeader("Authorization", $"Bearer {result.Token}");
            this.UserId = result.UserId;

            return true;
        }

        public void Logoff()
        {
            this.RestClient = this.initialRestClient;
            this.UserId = null;
        }

        private async Task<T> Post<T>(Uri uri, object data)
        {
            if (this.SecondsBeforeRetry == null || this.SecondsBeforeRetry.Length == 0)
            {
                return await this.PostOnce<T>(uri, data);
            }

            Exception exception = null;

            foreach (var secondBeforeRetry in this.SecondsBeforeRetry)
            {
                try
                {
                    return await this.PostOnce<T>(uri, data);
                }
                catch (Exception e)
                {
                    exception = e;
                    await Task.Delay(secondBeforeRetry);
                }
            }

            throw exception ?? new Exception("Post not executed");
        }

        private async Task<T> PostOnce<T>(Uri uri, object data)
        {
            var request = new RestRequest(uri, Method.POST, DataFormat.Json).AddJsonBody(data);
            return await this.RestClient.PostAsync<T>(request);
        }
    }
}
