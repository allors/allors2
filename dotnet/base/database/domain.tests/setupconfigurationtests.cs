// <copyright file="SetupConfigurationTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;
    using Xunit;

    public class SetupConfigurationTests : DomainTest
    {
        [Fact]
        public void GivenSetConfiguration_WhenApplied_ThenCountryEuMemberStateIsSet()
        {
            var finland = new Countries(this.Session).FindBy(M.Country.IsoCode, "FI");
            Assert.True(finland.EuMemberState.Value);

            var norway = new Countries(this.Session).FindBy(M.Country.IsoCode, "NO");
            Assert.False(norway.EuMemberState.Value);
        }

        [Fact]
        public void GivenSetConfiguration_WhenApplied_ThenCountryIbanDataIsSet()
        {
            var finland = new Countries(this.Session).FindBy(M.Country.IsoCode, "FI");
            Assert.Equal(18, finland.IbanLength);
            Assert.Equal(@"\d{14}", finland.IbanRegex);

            var norway = new Countries(this.Session).FindBy(M.Country.IsoCode, "NO");
            Assert.Equal(15, norway.IbanLength);
            Assert.Equal(@"\d{11}", norway.IbanRegex);
        }
    }
}
