// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SyncTests.cs" company="Allors bvba">
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
// <summary>
//   Defines the PersonTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Tests
{
    using Xunit;

    public class SyncTests : DomainTest
    {
        [Fact]
        public void IsSync()
        {
            var metaPopulation = this.Session.Database.MetaPopulation;
            foreach (var composite in metaPopulation.Composites)
            {
                switch (composite.Name)
                {
                    case "SyncDepthI1":
                    case "SyncDepthC1":
                    case "SyncDepth2":
                        Assert.True(composite.IsSynced);
                        break;

                    default:
                        Assert.False(composite.IsSynced);
                        break;
                }
            }
        }
    }
}
