// <copyright file="Fixture.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Npgsql
{
    using System;
    using System.Collections.Generic;
    using MysticMind.PostgresEmbed;
    using Xunit;

    [CollectionDefinition(Fixture.Collection)]
    public class Collection : ICollectionFixture<Fixture>
    {
    }

    public class Fixture : IDisposable
    {
        public const string Collection = "Npgsql collection";

        public PgServer PgServer { get; private set; }

        public Fixture()
        {
            var usePgServerEnvironmentVariable = Environment.GetEnvironmentVariable("ALLORS_NPGSQL_USE_PGSERVER");
            if (!bool.TryParse(usePgServerEnvironmentVariable, out var usePgServer))
            {
                usePgServer = true;
            }

            if (usePgServer)
            {
                var pgServerParams = new Dictionary<string, string>
                {
                    { "timezone", "UTC" },
                    { "synchronous_commit", "off" },
                };

                this.PgServer = new PgServer(
                    pgVersion: "10.7.1",
                    addLocalUserAccessPermission: true,
                    // clearWorkingDirOnStart: true,
                    pgServerParams: pgServerParams,
                    locale: "English_Belgium.1252");

                this.PgServer.Start();
            }
        }

        public void Dispose()
        {
            this.PgServer?.Stop();
            this.PgServer = null;
        }
    }
}
