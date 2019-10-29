// <copyright file="SyncTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server.Tests
{
    using System;

    using Allors.Domain;
    using Allors.Protocol.Remote.Sync;
    using Meta;
    using Protocol;
    using Xunit;

    [Collection("Api")]
    public class SyncTests : ApiTest
    {
        [Fact]
        public async void DeletedObject()
        {
            await this.SignIn(this.Administrator);

            var organisation = new OrganisationBuilder(this.Session).Build();
            this.Session.Commit();

            var uri = new Uri(@"allors/sync", UriKind.Relative);

            var syncRequest = new SyncRequest
            {
                Objects = new[] { organisation.Id.ToString() },
            };
            var response = await this.PostAsJsonAsync(uri, syncRequest);

            organisation.Strategy.Delete();

            this.Session.Commit();

            response = await this.PostAsJsonAsync(uri, syncRequest);
            var syncResponse = await this.ReadAsAsync<SyncResponse>(response);

            Assert.Empty(syncResponse.Objects);
        }

        [Fact]
        public async void ExistingObject()
        {
            await this.SignIn(this.Administrator);

            var people = new People(this.Session).Extent();
            var person = people[0];

            var uri = new Uri(@"allors/sync", UriKind.Relative);

            var syncRequest = new SyncRequest
            {
                Objects = new[] { person.Id.ToString() },
            };
            var response = await this.PostAsJsonAsync(uri, syncRequest);

            Assert.True(response.IsSuccessStatusCode);

            var syncResponse = await this.ReadAsAsync<SyncResponse>(response);

            Assert.Single(syncResponse.Objects);
            var syncObject = syncResponse.Objects[0];

            Assert.Equal(person.Id.ToString(), syncObject.I);
            Assert.Equal($"{M.Person.Class.IdAsString}", syncObject.T);
            Assert.Equal(person.Strategy.ObjectVersion.ToString(), syncObject.V);
        }


        [Fact]
        public async void WithoutAccessControl()
        {
            var user = new Users(this.Session).GetUser("noacl");
            await this.SignIn(user);

            var people = new People(this.Session).Extent();
            var person = people[0];

            var uri = new Uri(@"allors/sync", UriKind.Relative);

            var syncRequest = new SyncRequest
            {
                Objects = new[] { person.Id.ToString() },
            };
            var response = await this.PostAsJsonAsync(uri, syncRequest);

            Assert.True(response.IsSuccessStatusCode);

            var syncResponse = await this.ReadAsAsync<SyncResponse>(response);

            Assert.Single(syncResponse.Objects);
            var syncObject = syncResponse.Objects[0];

            Assert.Null(syncObject.A);
            Assert.Null(syncObject.D);
        }
    }
}
