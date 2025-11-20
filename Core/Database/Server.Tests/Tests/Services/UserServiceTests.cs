// <copyright file="UserServiceTests.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
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
            await this.SignIn(this.Administrator);

            var uri = new Uri("TestSession/UserName", UriKind.Relative);
            var response = await this.HttpClient.PostAsync(uri, null);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal("administrator", content);
        }

        [Fact]
        public async void SignedOut()
        {
            await this.SignIn(this.Administrator);

            this.SignOut();

            var uri = new Uri("TestSession/UserName", UriKind.Relative);
            var response = await this.HttpClient.PostAsync(uri, null);
            var content = await response.Content.ReadAsStringAsync();

            Assert.Equal("Guest", content);
        }
    }
}
