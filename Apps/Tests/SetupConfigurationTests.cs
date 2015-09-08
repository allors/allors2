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

    using NUnit.Framework;

    [TestFixture]
    public class SetupConfigurationTests : DomainTest
    {
        [Test]
        public void GivenSetConfiguration_WhenApplied_ThenCountryEuMemberStateIsSet()
        {
            var finland = new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "FI");
            Assert.IsTrue(finland.EuMemberState.Value);

            var norway = new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "NO");
            Assert.IsFalse(norway.EuMemberState.Value);
        }

        [Test]
        public void GivenSetConfiguration_WhenApplied_ThenCountryIbanDataIsSet()
        {
            var finland = new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "FI");
            Assert.AreEqual(18, finland.IbanLength);
            Assert.AreEqual(@"\d{14}", finland.IbanRegex);

            var norway = new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "NO");
            Assert.AreEqual(15, norway.IbanLength);
            Assert.AreEqual(@"\d{11}", norway.IbanRegex);
        }
    }
}