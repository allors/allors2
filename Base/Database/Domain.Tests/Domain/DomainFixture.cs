// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DomainFixture.cs" company="Allors bvba">
//   Copyright 2002-2009 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
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
    using NUnit.Framework;

    [SetUpFixture]
    public class DomainFixture
    {
        [SetUp]
        public void SetUp()
        {
            var configuration = new Allors.Adapters.Memory.Configuration { ObjectFactory = Config.ObjectFactory };
            Config.Default = new Allors.Adapters.Memory.Database(configuration);

            //var configuration = new Allors.Adapters.Object.SqlClient.Configuration { ObjectFactory = Config.ObjectFactory };
            //Config.Default = new Allors.Adapters.Object.SqlClient.Database(configuration);
        }
    }
}