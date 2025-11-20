// <copyright file="PushTests.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
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
            await this.SignIn(this.Administrator);

            var uri = new Uri("push", UriKind.Relative);

            var pushRequest = new PushRequest
            {
                newObjects = new[] { new PushRequestNewObject { t = M.Build.Class.IdAsString, ni = "-1" }, },
            };

            var response = await this.PostAsJsonAsync(uri, pushRequest);
            var pushResponse = await this.ReadAsAsync<PushResponse>(response);

            this.Session.Rollback();

            var build = (Build)this.Session.Instantiate(pushResponse.newObjects[0].i);

            Assert.Equal(new Guid("DCE649A4-7CF6-48FA-93E4-CDE222DA2A94"), build.Guid);
            Assert.Equal("Exist", build.String);
        }

        [Fact]
        public async void DeletedObject()
        {
            await this.SignIn(this.Administrator);

            var organisation = new OrganisationBuilder(this.Session).Build();
            this.Session.Commit();

            var organisationId = organisation.Id.ToString();
            var organisationVersion = organisation.Strategy.ObjectVersion.ToString();

            organisation.Delete();
            this.Session.Commit();

            var uri = new Uri("push", UriKind.Relative);

            var pushRequest = new PushRequest
            {
                objects = new[]
                {
                    new PushRequestObject
                {
                                                                                  i = organisationId,
                                                                                  v = organisationVersion,
                                                                                  roles = new[]
                                                                                      {
                                                                                          new PushRequestRole
                                                                                              {
                                                                                                  t = M.Organisation.Name.PropertyName,
                                                                                                  s = "Acme"
                                                                                              },
                                                                                      },
                                                                                  },
                                                          },
            };
            var response = await this.PostAsJsonAsync(uri, pushRequest);

            Assert.True(response.IsSuccessStatusCode);

            var pushResponse = await this.ReadAsAsync<PushResponse>(response);

            Assert.True(pushResponse.HasErrors);
            Assert.Single(pushResponse.missingErrors);
            Assert.Contains(organisationId, pushResponse.missingErrors);
        }
    }
}
