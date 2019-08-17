// <copyright file="DefaultConnectionFactory.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Adapters.Object.Npgsql
{
    public sealed class DefaultConnectionFactory : IConnectionFactory
    {
        public Connection Create(Database database) => new DefaultConnection(database);
    }
}
