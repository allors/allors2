// <copyright file="PushTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server.Tests
{
    using System;
    using Allors.Domain;
    using Allors.Meta;
    using Allors.Protocol.Remote.Push;
    using Protocol;
    using Xunit;

    [Collection("Api")]
    public class PushTests : ApiTest
    {
        [Fact]
        public async void WorkspaceNewObject()
        {
            var compressor = new Compressor();

            var administrator = new Users(this.Session).GetUser("administrator");
            await this.SignIn(administrator);

            var uri = new Uri(@"allors/push", UriKind.Relative);

            var pushRequest = new PushRequest
            {
                NewObjects = new[] { new PushRequestNewObject { T = compressor.Write(M.Build.Class.IdAsString), NI = "-1" }, },
            };

            var response = await this.PostAsJsonAsync(uri, pushRequest);
            var pushResponse = await this.ReadAsAsync<PushResponse>(response);

            this.Session.Rollback();

            var build = (Build)this.Session.Instantiate(pushResponse.NewObjects[0].I);

            Assert.Equal(new Guid("DCE649A4-7CF6-48FA-93E4-CDE222DA2A94"), build.Guid);
            Assert.Equal("Exist", build.String);
        }

        [Fact]
        public async void DeletedObject()
        {
            var administrator = new Users(this.Session).GetUser("administrator");
            await this.SignIn(administrator);

            var organisation = new OrganisationBuilder(this.Session).Build();
            this.Session.Commit();

            var organisationId = organisation.Id.ToString();
            var organisationVersion = organisation.Strategy.ObjectVersion.ToString();

            organisation.Delete();
            this.Session.Commit();

            var uri = new Uri(@"allors/push", UriKind.Relative);

            var pushRequest = new PushRequest
            {
                Objects = new[]
                {
                    new PushRequestObject
                {
                                                                                  I = organisationId,
                                                                                  V = organisationVersion,
                                                                                  Roles = new[]
                                                                                      {
                                                                                          new PushRequestRole
                                                                                              {
                                                                                                  T = M.Organisation.Name.PropertyName,
                                                                                                  S = "Acme"
                                                                                              },
                                                                                      },
                                                                                  },
                                                          },
            };
            var response = await this.PostAsJsonAsync(uri, pushRequest);

            Assert.True(response.IsSuccessStatusCode);

            var pushResponse = await this.ReadAsAsync<PushResponse>(response);

            Assert.True(pushResponse.HasErrors);
            Assert.Single(pushResponse.MissingErrors);
            Assert.Contains(organisationId, pushResponse.MissingErrors);
        }
    }
}
