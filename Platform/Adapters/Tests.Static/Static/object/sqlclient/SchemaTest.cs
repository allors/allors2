// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SchemaTest.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
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

namespace Allors.Adapters.Object.SqlClient
{
    using Xunit;

    public abstract class SchemaTest : Object.SchemaTest
    {
        [Fact(Skip = "Explicit")]
        public void Recover()
        {
            //this.InitAndCreateSession();

            //this.DropProcedure("_GC");

            //var repository = this.CreateDatabase();

            //var exceptionThrown = false;
            //try
            //{
            //    repository.CreateSession();
            //}
            //catch
            //{
            //    exceptionThrown = true;
            //}

            //Assert.True(exceptionThrown);

            //((Database.SqlClient.Database)repository).Schema.Recover();

            //Assert.True(this.ExistProcedure("_GC"));

            //exceptionThrown = false;
            //try
            //{
            //    repository.CreateSession();
            //}
            //catch
            //{
            //    exceptionThrown = true;
            //}

            //Assert.False(exceptionThrown);
        }
    }
}
