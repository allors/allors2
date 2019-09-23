// <copyright file="UserServiceTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server.Tests
{
    using System;

    using Allors.Domain;

    using Xunit;

    [Collection("Api")]
    public class UserServiceTests : ApiTest
    {
        [Fact]
        public async void SignedIn()
        {
            var administrator = new Users(this.Session).GetUser("administrator");
            await this.SignIn(administrator);

            var uri = new Uri("TestSession/UserName", UriKind.Relative);
            var response = await this.HttpClient.PostAsync(uri, null);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal("Administrator", content);
        }

        [Fact]
        public async void SignedOut()
        {
            var administrator = new Users(this.Session).GetUser("administrator");
            await this.SignIn(administrator);

            this.SignOut();

            var uri = new Uri("TestSession/UserName", UriKind.Relative);
            var response = await this.HttpClient.PostAsync(uri, null);
            var content = await response.Content.ReadAsStringAsync();

            Assert.Equal("Guest", content);
        }
    }
}
