// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SchemaTest.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Allors;

namespace Allors.Adapters.Object.SqlClient.ReadCommitted
{
    using Adapters;
    using Adapters.Object.SqlClient;

    using Allors.Meta;

    using NUnit.Framework;

    [TestFixture]
    public class SchemaTest : SchemaIntegerIdTest
    {
        private readonly Profile profile = new Profile();

        protected override IProfile Profile => this.profile;

        protected override IDatabase CreateDatabase(IMetaPopulation metaPopulation, bool init)
        {
            return this.profile.CreateDatabase(metaPopulation, init);
        }

        [TearDown]
        protected void Dispose()
        {
            this.profile.Dispose();
        }

        protected override void DropTable(string schema, string tableName)
        {
            this.profile.DropTable(schema, tableName);
        }

        protected override bool ExistTable(string schema, string table)
        {
            return this.profile.ExistTable(schema, table);
        }

        protected override int ColumnCount(string schema, string table)
        {
            return this.profile.ColumnCount(schema, table);
        }

        protected override bool ExistColumn(string schema, string table, string column, ColumnTypes columnType)
        {
            return this.profile.ExistColumn(schema, table, column, columnType);
        }

        protected override bool ExistPrimaryKey(string schema, string table, string column)
        {
            return this.profile.ExistPrimaryKey(schema, table, column);
        }

        protected override bool ExistProcedure(string schema, string procedure)
        {
            return this.profile.ExistProcedure(schema, procedure);
        }

        protected override bool ExistIndex(string schema, string table, string column)
        {
            return this.profile.ExistIndex(schema, table, column);
        }

        protected override void DropProcedure(string schema, string procedure)
        {
            this.profile.DropProcedure(procedure);
        }
    }
}