// <copyright file="RemoteTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.Remote
{
    using System;
    using System.Net.Http;

    using Allors.Workspace;
    using Allors.Workspace.Remote;
    using Allors.Workspace.Domain;
    using Allors.Workspace.Meta;

    using Xunit;

    [Collection("Remote")]
    public class RemoteTest : IDisposable
    {
        public const string Url = "http://localhost:5000/allors/";

        public const string InitUrl = "Test/Init";
        public const string SetupUrl = "Test/Setup?population=full";
        public const string LoginUrl = "Test/Login";

        public Workspace Workspace { get; set; }

        public RemoteDatabase Database { get; set; }

        public RemoteTest()
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(Url),
            };

            this.Database = new RemoteDatabase(client);

            var objectFactory = new ObjectFactory(MetaPopulation.Instance, typeof(User));
            this.Workspace = new Workspace(objectFactory);

            this.Init();
        }

        public void Dispose()
        {
        }

        public void Login(string user)
        {
            var uri = new Uri("TestAuthentication/Token", UriKind.Relative);
            var result = this.Database.Login(uri, user, null).Result;
        }

        private void Init()
        {
            var httpResponseMessage = this.Database.HttpClient.GetAsync(SetupUrl).Result;
            this.Login("administrator");
        }
    }
}
