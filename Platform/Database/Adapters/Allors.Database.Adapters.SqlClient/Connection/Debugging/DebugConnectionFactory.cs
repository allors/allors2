// <copyright file="DebugConnectionFactory.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.SqlClient.Debug
{
    using System.Collections.Generic;

    public class DebugConnectionFactory : IConnectionFactory
    {
        public List<DebugConnection> Connections { get; } = new List<DebugConnection>();

        public Connection Create(Database database)
        {
            var connection = new DebugConnection(database);
            this.Connections.Add(connection);
            return connection;
        }
    }
}
