// <copyright file="ManagementSession.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.SqlClient
{
    using System;

    internal class ManagementSession : IDisposable
    {
        internal ManagementSession(Database database, IConnectionFactory connectionFactory)
        {
            this.Database = database;
            this.Connection = connectionFactory.Create(database);
        }

        ~ManagementSession() => this.Dispose();

        public Database Database { get; }

        public Connection Connection { get; }

        public void Dispose() => this.Rollback();

        internal void Commit() => this.Connection.Commit();

        internal void Rollback() => this.Connection.Rollback();
    }
}
