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

namespace Allors.Adapters.Relation
{
    using System;

    using Allors.Meta;

    using Xunit;

    public abstract class SchemaTest : Adapters.SchemaTest
    {
        //[Fact]
        //public void OneToOne()
        //{
        //    var relationTypeId = new Guid("89479CC3-2BE0-46A9-8008-E9D5F1377897");

        //    this.DropTable("allors", "_89479cc32be046a98008e9d5f1377897_string_256");
        //    this.DropTable("allors", "_89479cc32be046a98008e9d5f1377897_11");

        //    this.CreateDomainWithUnitRelationType(relationTypeId, UnitIds.StringId);
        //    this.CreateDatabase(this.domain.MetaPopulation, true);
            
        //    var relationType = this.CreateDomainWithCompositeRelationType(relationTypeId, Multiplicity.OneToOne);
        //    var Database = this.CreateDatabase(this.domain.MetaPopulation, false);

        //    Assert.True(this.CreateSessionThrowsException(Database));

        //    Database = this.CreateDatabase(this.domain.MetaPopulation, true);

        //    Assert.False(this.CreateSessionThrowsException(Database));

        //    var tableName = "_" + relationType.Id.ToString("n") + "_11";
        //    Assert.True(this.ExistTable("allors", tableName));
        //    Assert.Equal(2, this.ColumnCount("allors", tableName));
        //    Assert.True(this.ExistColumn("allors", tableName, "a", ColumnTypes.ObjectId));
        //    Assert.True(this.ExistColumn("allors", tableName, "r", ColumnTypes.ObjectId));

        //    this.DropTable("allors", tableName);

        //    Database = this.CreateDatabase(this.domain.MetaPopulation, false);

        //    Assert.True(this.CreateSessionThrowsException(Database));
        //}

    }
}