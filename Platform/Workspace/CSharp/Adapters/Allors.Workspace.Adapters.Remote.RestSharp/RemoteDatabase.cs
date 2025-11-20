// <copyright file="IDatabase.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
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
    using Protocol.Remote.Security;
    using RestSharp;

    public class RemoteDatabase : IDatabase
    {
        private readonly Func<IRestClient> restClientFactory;

        public RemoteDatabase(Func<IRestClient> restClientFactory) => this.restClientFactory = restClientFactory;

        public IRestClient RestClient { get; private set; }

        public string UserId { get; private set; }

        public int[] SecondsBeforeRetry { get; set; } = { 1, 2, 4, 8, 16 };

        public async Task<PushResponse> Push(PushRequest pushRequest)
        {
            var uri = new Uri("push", UriKind.Relative);
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
            var uri = new Uri("invoke", UriKind.Relative);
            return await this.Post<InvokeResponse>(uri, invokeRequest);
        }

        public async Task<InvokeResponse> Invoke(string service, object args)
        {
            var uri = new Uri(service + "invoke", UriKind.Relative);
            return await this.Post<InvokeResponse>(uri, args);
        }

        public async Task<SecurityResponse> Security(SecurityRequest securityRequest)
        {
            var uri = new Uri("security", UriKind.Relative);
            return await this.Post<SecurityResponse>(uri, securityRequest);
        }

        public async Task<bool> Login(Uri url, string username, string password)
        {
            this.RestClient = this.restClientFactory();

            var data = new { UserName = username, Password = password };
            var result = await this.Post<AuthenticationResult>(url, data);

            if (!result.Authenticated)
            {
                this.RestClient = null;
                return false;
            }

            this.RestClient.AddDefaultHeader("Authorization", $"Bearer {result.Token}");
            this.UserId = result.UserId;

            return true;
        }

        public void Logoff()
        {
            this.RestClient = null;
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
            var request = new RestRequest(uri, Method.POST, DataFormat.Json);
            if (data != null)
            {
                request.AddJsonBody(data);
            }

            return await this.RestClient.PostAsync<T>(request);
        }
    }
}
