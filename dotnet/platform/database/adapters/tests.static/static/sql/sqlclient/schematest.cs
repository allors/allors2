// <copyright file="SchemaTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.SqlClient
{
    using System;
    using Meta;
    using Xunit;

    public abstract class SchemaTest : Adapters.Sql.SchemaTest, IDisposable
    {
        private readonly Profile profile = new Profile();

        protected override IProfile Profile => this.profile;

        public override void Dispose() => this.profile.Dispose();

        protected override IDatabase CreateDatabase(IMetaPopulation metaPopulation, bool init) => this.profile.CreateDatabase(metaPopulation, init);

        protected override void DropTable(string schema, string tableName) => this.profile.DropTable(schema, tableName);

        protected override bool ExistTable(string schema, string table) => this.profile.ExistTable(schema, table);

        protected override int ColumnCount(string schema, string table) => this.profile.ColumnCount(schema, table);

        protected override bool ExistColumn(string schema, string table, string column, ColumnTypes columnType) => this.profile.ExistColumn(schema, table, column, columnType);

        protected override bool ExistPrimaryKey(string schema, string table, string column) => this.profile.ExistPrimaryKey(schema, table, column);

        protected override bool ExistProcedure(string schema, string procedure) => this.profile.ExistProcedure(schema, procedure);

        protected override bool ExistIndex(string schema, string table, string column) => this.profile.ExistIndex(schema, table, column);

        protected override void DropProcedure(string schema, string procedure) => this.profile.DropProcedure(procedure);

        [Fact(Skip = "Explicit")]
        public void Recover()
        {
            // this.InitAndCreateSession();

            // this.DropProcedure("_GC");

            // var repository = this.CreateDatabase();

            // var exceptionThrown = false;
            // try
            // {
            //    repository.CreateSession();
            // }
            // catch
            // {
            //    exceptionThrown = true;
            // }

            // Assert.True(exceptionThrown);

            // ((Database.SqlClient.Database)repository).Schema.Recover();

            // Assert.True(this.ExistProcedure("_GC"));

            // exceptionThrown = false;
            // try
            // {
            //    repository.CreateSession();
            // }
            // catch
            // {
            //    exceptionThrown = true;
            // }

            // Assert.False(exceptionThrown);
        }
    }
}
