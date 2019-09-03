// <copyright file="PullExtentTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Api.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Allors.Domain;
    using Allors.Meta;
    using Allors.Protocol.Data;
    using Allors.Protocol.Remote.Pull;

    using Xunit;

    [Collection("Api")]
    public class PullExtentTests : ApiTest
    {
        [Fact]
        public async void WithoutResult()
        {
            var administrator = new Users(this.Session).GetUser("administrator");
            await this.SignIn(administrator);

            var data = new DataBuilder(this.Session).WithString("First").Build();

            this.Session.Commit();

            var uri = new Uri(@"Pull/Pull", UriKind.Relative);

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

            var organisations = pullResponse.NamedCollections["Datas"];

            Assert.Single(organisations);

            var dataId = organisations.First();

            Assert.Equal(data.Id.ToString(), dataId);
        }

        [Fact]
        public async void WithResult()
        {
            var administrator = new Users(this.Session).GetUser("administrator");
            await this.SignIn(administrator);

            var data = new DataBuilder(this.Session).WithString("First").Build();

            this.Session.Commit();

            var uri = new Uri(@"Pull/Pull", UriKind.Relative);

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

            var organisations = pullResponse.NamedCollections["Datas"];

            Assert.Single(organisations);

            var dataId = organisations.First();

            Assert.Equal(data.Id.ToString(), dataId);
        }

        [Fact]
        public async void WithExtentRef()
        {
            var administrator = new Users(this.Session).GetUser("administrator");
            await this.SignIn(administrator);

            this.Session.Commit();

            var uri = new Uri(@"Pull/Pull", UriKind.Relative);

            var pullRequest = new PullRequest
            {
                P = new[]
                  {
                      new Pull
                          {
                              ExtentRef = Organisations.ExtentByName,
                              Arguments = new Dictionary<string, object> { ["name"] = "Acme" },
                          },
                  },
            };

            var response = await this.PostAsJsonAsync(uri, pullRequest);
            var pullResponse = await this.ReadAsAsync<PullResponse>(response);

            var organisations = pullResponse.NamedCollections["Organisations"];

            Assert.Single(organisations);
        }

        [Fact]
        public async void WithFetchRef()
        {
            var administrator = new Users(this.Session).GetUser("administrator");
            await this.SignIn(administrator);

            this.Session.Commit();

            var uri = new Uri(@"Pull/Pull", UriKind.Relative);

            var pullRequest = new PullRequest
            {
                P = new[]
                  {
                      new Pull
                          {
                              ExtentRef = Organisations.ExtentByName,
                              Arguments = new Dictionary<string, object> { ["name"] = "Acme" },
                          },
                  },
            };

            var response = await this.PostAsJsonAsync(uri, pullRequest);
            var pullResponse = await this.ReadAsAsync<PullResponse>(response);

            var organisations = pullResponse.NamedCollections["Organisations"];

            Assert.Single(organisations);
        }
    }
}
