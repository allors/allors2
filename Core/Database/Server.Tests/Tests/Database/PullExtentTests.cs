// <copyright file="PullExtentTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Allors.Domain;
    using Allors.Meta;
    using Allors.Protocol.Data;
    using Allors.Protocol.Remote.Pull;
    using Protocol.Remote;
    using Xunit;

    [Collection("Api")]
    public class PullExtentTests : ApiTest
    {
        private Func<IAccessControlList, string> PrintAccessControls =>
            acl => string.Join(Encoding.Separator, acl.AccessControls.OrderBy(v => v).Select(v => v.Id.ToString()));

        private Func<IAccessControlList, string> PrintDeniedPermissions =>
            acl => string.Join(Encoding.Separator, acl.DeniedPermissionIds.OrderBy(v => v).Select(v => v.ToString()));

        [Fact]
        public async void WithDeniedPermissions()
        {
            await this.SignIn(this.Administrator);

            var data = new DataBuilder(this.Session).WithString("First").Build();
            var permission = new Permissions(this.Session).Extent().First(v => v.ConcreteClass == M.Data.Class);
            data.AddDeniedPermission(permission);

            this.Session.Commit();

            var uri = new Uri(@"allors/pull", UriKind.Relative);

            var extent = new Allors.Data.Filter(M.Data.ObjectType);

            var pullRequest = new PullRequest
            {
                P = new[]
                {
                    new Pull
                    {
                        Extent = extent.Save(),
                    },
                },
            };

            var response = await this.PostAsJsonAsync(uri, pullRequest);
            var pullResponse = await this.ReadAsAsync<PullResponse>(response);

            var namedCollection = pullResponse.NamedCollections["Datas"];

            Assert.Single(namedCollection);

            var namedObject = namedCollection.First();

            Assert.Equal(data.Id.ToString(), namedObject);

            var objects = pullResponse.Objects;

            Assert.Single(objects);

            var @object = objects[0];

            var acls = new AccessControlLists(this.Administrator);
            var acl = acls[data];

            Assert.Equal(4, @object.Length);

            Assert.Equal(data.Strategy.ObjectId.ToString(), @object[0]);
            Assert.Equal(data.Strategy.ObjectVersion.ToString(), @object[1]);
            Assert.Equal($"{this.PrintAccessControls(acl)}", @object[2]);
            Assert.Equal($"{this.PrintDeniedPermissions(acl)}", @object[3]);
        }

        [Fact]
        public async void WithExtentRef()
        {
            await this.SignIn(this.Administrator);

            this.Session.Commit();

            var uri = new Uri(@"allors/pull", UriKind.Relative);

            var pullRequest = new PullRequest
            {
                P = new[]
                  {
                      new Pull
                          {
                              ExtentRef = Organisations.ExtentByName,
                              Parameters = new Dictionary<string, string> { ["name"] = "Acme" },
                          },
                  },
            };

            var response = await this.PostAsJsonAsync(uri, pullRequest);

            Assert.True(response.IsSuccessStatusCode);

            var pullResponse = await this.ReadAsAsync<PullResponse>(response);

            var organisations = pullResponse.NamedCollections["Organisations"];

            Assert.Single(organisations);
        }

        [Fact]
        public async void WithFetchRef()
        {
            await this.SignIn(this.Administrator);

            this.Session.Commit();

            var uri = new Uri(@"allors/pull", UriKind.Relative);

            var pullRequest = new PullRequest
            {
                P = new[]
                  {
                      new Pull
                          {
                              ExtentRef = Organisations.ExtentByName,
                              Parameters = new Dictionary<string, string> { ["name"] = "Acme" },
                          },
                  },
            };

            var response = await this.PostAsJsonAsync(uri, pullRequest);

            Assert.True(response.IsSuccessStatusCode);

            var pullResponse = await this.ReadAsAsync<PullResponse>(response);

            var organisations = pullResponse.NamedCollections["Organisations"];

            Assert.Single(organisations);
        }

        [Fact]
        public async void WithoutDeniedPermissions()
        {
            await this.SignIn(this.Administrator);

            var data = new DataBuilder(this.Session).WithString("First").Build();

            this.Session.Commit();

            var uri = new Uri(@"allors/pull", UriKind.Relative);

            var extent = new Allors.Data.Filter(M.Data.ObjectType);

            var pullRequest = new PullRequest
            {
                P = new[]
                      {
                          new Pull
                              {
                                  Extent = extent.Save(),
                              },
                      },
            };

            var response = await this.PostAsJsonAsync(uri, pullRequest);
            var pullResponse = await this.ReadAsAsync<PullResponse>(response);

            var namedCollection = pullResponse.NamedCollections["Datas"];

            Assert.Single(namedCollection);

            var namedObject = namedCollection.First();

            Assert.Equal(data.Id.ToString(), namedObject);

            var objects = pullResponse.Objects;

            Assert.Single(objects);

            var @object = objects[0];

            var acls = new AccessControlLists(this.Administrator);
            var acl = acls[data];

            Assert.Equal(3, @object.Length);

            Assert.Equal(data.Strategy.ObjectId.ToString(), @object[0]);
            Assert.Equal(data.Strategy.ObjectVersion.ToString(), @object[1]);
            Assert.Equal($"{this.PrintAccessControls(acl)}", @object[2]);
        }

        [Fact]
        public async void WithResult()
        {
            await this.SignIn(this.Administrator);

            var data = new DataBuilder(this.Session).WithString("First").Build();

            this.Session.Commit();

            var uri = new Uri(@"allors/pull", UriKind.Relative);

            var extent = new Allors.Data.Filter(M.Data.ObjectType);

            var pullRequest = new PullRequest
            {
                P = new[]
                      {
                          new Pull
                              {
                                  Extent = extent.Save(),
                                  Results = new[]
                                                {
                                                    new Result { Name = "Datas" },
                                                },
                              },
                      },
            };

            var response = await this.PostAsJsonAsync(uri, pullRequest);
            var pullResponse = await this.ReadAsAsync<PullResponse>(response);

            var namedCollection = pullResponse.NamedCollections["Datas"];

            Assert.Single(namedCollection);

            var namedObject = namedCollection.First();

            Assert.Equal(data.Id.ToString(), namedObject);

            var objects = pullResponse.Objects;

            Assert.Single(objects);

            var @object = objects[0];

            var acls = new AccessControlLists(this.Administrator);
            var acl = acls[data];

            Assert.Equal(3, @object.Length);

            Assert.Equal(data.Strategy.ObjectId.ToString(), @object[0]);
            Assert.Equal(data.Strategy.ObjectVersion.ToString(), @object[1]);
            Assert.Equal($"{this.PrintAccessControls(acl)}", @object[2]);
        }
    }
}
