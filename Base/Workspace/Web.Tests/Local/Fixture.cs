// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServerFixture.cs" company="Allors bvba">
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

namespace Tests.Local
{
    using System.Globalization;
    using System.Threading;

    using Allors;
    using Allors.Adapters.Memory;

    using NUnit.Framework;

    using Configuration = Allors.Adapters.Memory.Configuration;

    [SetUpFixture]
    public class Fixture
    {
        [SetUp]
        public void SetUp()
        {
            var configuration = new Configuration { ObjectFactory = Config.ObjectFactory };
            Config.Default = new Database(configuration);

            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        }
    }
}