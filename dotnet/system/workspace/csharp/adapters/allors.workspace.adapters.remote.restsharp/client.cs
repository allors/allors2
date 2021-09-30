// <copyright file="RemoteDatabase.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Remote.ResthSharp
{
    using System;
    using System.Threading.Tasks;
    using Allors.Protocol.Json.Auth;
    using RestSharp;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "RCS1090:Add call to 'ConfigureAwait' (or vice versa).", Justification = "<Pending>")]
    public class Client
    {
        public int[] SecondsBeforeRetry { get; set; } = { 1, 2, 4, 8, 16 };

        private readonly Func<IRestClient> restClientFactory;

        public Client(Func<IRestClient> restClientFactory) => this.restClientFactory = restClientFactory;

        public IRestClient RestClient { get; private set; }

        public string UserId { get; private set; }

        public async Task<bool> Login(Uri url, string username, string password)
        {
            this.RestClient = this.restClientFactory();

            var data = new AuthenticationTokenRequest { l = username, p = password };
            var result = await this.Post<AuthenticationTokenResponse>(url, data);

            if (!result.a)
            {
                this.Logoff();
                return false;
            }

            this.UserId = result.u;
            this.RestClient.AddDefaultHeader("Authorization", $"Bearer {result.t}");

            return true;
        }

        public void Logoff()
        {
            this.RestClient = null;
            this.UserId = null;
        }

        internal async Task<T> Post<T>(Uri uri, object data)
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

        internal async Task<T> PostOnce<T>(Uri uri, object data)
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
