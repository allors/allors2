// <copyright file="SchemaTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Adapters.Object.SqlClient
{
    using Xunit;

    public abstract class SchemaTest : Object.SchemaTest
    {
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
