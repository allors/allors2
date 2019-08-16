// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetupConfigurationTests.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using Meta;
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