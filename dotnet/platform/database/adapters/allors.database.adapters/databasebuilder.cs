// <copyright file="DatabaseBuilder.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the AllorsStrategySql type.
// </summary>

namespace Allors.Database.Adapters
{
    using System;
    using System.Data;
    using Microsoft.Extensions.Configuration;

    public class DatabaseBuilder
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IConfiguration configuration;
        private readonly ObjectFactory objectFactory;
        private readonly IsolationLevel? isolationLevel;
        private readonly int? commandTimeout;

        public DatabaseBuilder(IServiceProvider serviceProvider, IConfiguration configuration, ObjectFactory objectFactory, IsolationLevel? isolationLevel = null, int? commandTimeout = null)
        {
            this.serviceProvider = serviceProvider;
            this.configuration = configuration;
            this.objectFactory = objectFactory;
            this.isolationLevel = isolationLevel;
            this.commandTimeout = commandTimeout;
        }

        public IDatabase Build()
        {
            var adapter = this.configuration["adapter"]?.Trim().ToUpperInvariant();
            var connectionString = this.configuration["ConnectionStrings:DefaultConnection"];

            switch (adapter)
            {
                case "MEMORY":
                    throw new NotImplementedException();

                case "NPGSQL":

                    return new Npgsql.Database(this.serviceProvider, new Npgsql.Configuration
                    {
                        ConnectionString = connectionString,
                        ObjectFactory = this.objectFactory,
                        IsolationLevel = this.isolationLevel,
                        CommandTimeout = this.commandTimeout,
                    });

                case "SQLCLIENT":
                default:

                    return new SqlClient.Database(this.serviceProvider, new SqlClient.Configuration
                    {
                        ConnectionString = connectionString,
                        ObjectFactory = this.objectFactory,
                        IsolationLevel = this.isolationLevel,
                        CommandTimeout = this.commandTimeout,
                    });
            }
        }
    }
}
