// <copyright file="SignOutTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server.Tests
{
    using System;
    using Domain;
    using Server;

    using Xunit;

    [Collection("Api")]
    public class SignOutTests : ApiTest
    {
        public SignOutTests()
        {
            var jane = new PersonBuilder(this.Session)
                .WithFirstName("Jane")
                .WithLastName("Doe")
                .WithUserName("jane@example.com")
                .Build()
                .SetPassword("p@ssw0rd");

            this.Session.Derive();
            this.Session.Commit();
        }

        [Fact]
        public async void Successful()
        {
            var args = new AuthenticationTokenRequest
            {
                UserName = "jane@example.com",
                Password = "p@ssw0rd",
            };

            var signInUri = new Uri("Authentication/Token", UriKind.Relative);
            await this.PostAsJsonAsync(signInUri, args);

            var signOutUri = new Uri("Authentication/SignOut", UriKind.Relative);
            await this.PostAsJsonAsync(signOutUri, null);
        }
    }
}
