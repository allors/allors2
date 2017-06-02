// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultDerivationLogTests.cs" company="Allors bvba">
//   Copyright 2002-2016 Allors bvba.
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

namespace Domain
{
    using Allors;
    using Allors.Domain;

    using Xunit;

    
    public class DefaultDerivationLogTests : DomainTest
    {
        [Fact]
        public void DeletedUserinterfaceable()
        {
            var organisation = new OrganisationBuilder(this.Session).Build();
            
            var validation = this.Session.Derive(false);
            Assert.Equal(1, validation.Errors.Length);

            var error = validation.Errors[0];
            Assert.Equal("Organisation.Name is required", error.Message);
        }
    }
}