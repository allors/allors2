// <copyright file="DefaultConnectionFactory.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.SqlClient
{
    public sealed class DefaultConnectionFactory : IConnectionFactory
    {
        public Connection Create(Database database) => new DefaultConnection(database);
    }
}
