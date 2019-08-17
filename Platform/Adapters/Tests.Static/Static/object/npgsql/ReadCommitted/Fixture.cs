// <copyright file="Fixture.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using System.Collections.Generic;
using MysticMind.PostgresEmbed;
using Xunit;

namespace Allors.Adapters.Object.Npgsql.ReadCommitted
{
    using System;

    public class Fixture : IDisposable
    {
        public const string Collection = "Npgsql collection";

        public PgServer Server { get; private set; }

        public Fixture()
        {
            var pgServerParams = new Dictionary<string, string>
            {
                {"timezone", "UTC"},
                {"synchronous_commit", "off"},
            };

            this.Server = new PgServer(
                pgVersion: "10.7.1",
                addLocalUserAccessPermission: true,
                //clearWorkingDirOnStart: true,
                pgServerParams: pgServerParams,
                locale: "English_Belgium.1252");

            this.Server.Start();
        }

        public void Dispose()
        {
            this.Server?.Stop();
            this.Server = null;
        }
    }

    [CollectionDefinition(Fixture.Collection)]
    public class Collection : ICollectionFixture<Fixture>
    {
    }

}
