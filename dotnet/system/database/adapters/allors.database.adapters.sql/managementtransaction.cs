// <copyright file="ManagementTransaction.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql
{
    using System;

    public class ManagementTransaction : IDisposable
    {
        public ManagementTransaction(Database database, IConnectionFactory connectionFactory)
        {
            this.Database = database;
            this.Connection = connectionFactory.Create();
        }

        ~ManagementTransaction() => this.Dispose();

        public Database Database { get; }

        public IConnection Connection { get; }

        public void Dispose() => this.Rollback();

        internal void Commit() => this.Connection.Commit();

        public void Rollback() => this.Connection.Rollback();
    }
}
