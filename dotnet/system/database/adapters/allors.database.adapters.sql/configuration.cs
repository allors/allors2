// <copyright file="Configuration.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql
{
    using System.Data;
    using Caching;

    public class Configuration : Adapters.Configuration
    {
        public ICacheFactory CacheFactory { get; set; }

        public IConnectionFactory ConnectionFactory { get; set; }

        public IConnectionFactory ManagementConnectionFactory { get; set; }

        public string ConnectionString { get; set; }

        public int? CommandTimeout { get; set; }

        public IsolationLevel? IsolationLevel { get; set; }

        public string SchemaName { get; set; }
    }
}
