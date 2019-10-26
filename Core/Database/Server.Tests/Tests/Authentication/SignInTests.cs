// <copyright file="SignInTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server.Tests
{
    using System;

    using Allors.Domain;
    using Server;

    using Xunit;

    [Collection("Api")]

    public class SignInTests : ApiTest
    {
        public SignInTests()
        {
            new PersonBuilder(this.Session).WithFirstName("John").WithLastName("Doe").Build();

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
        public async void CorrectUserAndPassword()
        {
            var args = new AuthenticationTokenRequest
            {
                UserName = "Jane",
                Password = "p@ssw0rd",
            };

            var uri = new Uri("Authentication/Token", UriKind.Relative);
            var response = await this.PostAsJsonAsync(uri, args);
            var siginInResponse = await this.ReadAsAsync<AuthenticationTokenResponse>(response);

            Assert.True(siginInResponse.Authenticated);
        }

        [Fact]
        public async void NonExistingUser()
        {
            var args = new AuthenticationTokenRequest
            {
                UserName = "Jeff",
                Password = "p@ssw0rd",
            };

            var uri = new Uri("Authentication/Token", UriKind.Relative);
            var response = await this.PostAsJsonAsync(uri, args);
            var siginInResponse = await this.ReadAsAsync<AuthenticationTokenResponse>(response);

            Assert.False(siginInResponse.Authenticated);
        }

        [Fact]
        public async void EmptyStringPassword()
        {
            var args = new AuthenticationTokenRequest
            {
                UserName = "John",
                Password = "",
            };

            var uri = new Uri("Authentication/Token", UriKind.Relative);
            var response = await this.PostAsJsonAsync(uri, args);
            var siginInResponse = await this.ReadAsAsync<AuthenticationTokenResponse>(response);

            Assert.False(siginInResponse.Authenticated);
        }

        [Fact]
        public async void NoPassword()
        {
            var args = new AuthenticationTokenRequest
            {
                UserName = "John",
            };

            var uri = new Uri("Authentication/Token", UriKind.Relative);
            var response = await this.PostAsJsonAsync(uri, args);
            var siginInResponse = await this.ReadAsAsync<AuthenticationTokenResponse>(response);

            Assert.False(siginInResponse.Authenticated);
        }
    }
}
