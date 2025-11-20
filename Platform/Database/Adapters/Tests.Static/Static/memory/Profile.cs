// <copyright file="Profile.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory
{
    using System;
    using System.Collections.Generic;

    using Adapters;

    using Microsoft.Extensions.DependencyInjection;

    public class Profile : Adapters.Profile
    {
        public Profile()
        {
            var services = new ServiceCollection();
            this.ServiceProvider = services.BuildServiceProvider();
        }

        public override Action[] Markers
        {
            get
            {
                var markers = new List<Action>
                {
                    () => { },
                    () => this.Session.Commit(),
                };

                if (Settings.ExtraMarkers)
                {
                    markers.Add(
                        () =>
                        {
                            this.Session.Checkpoint();
                        });
                }

                return markers.ToArray();
            }
        }

        public ServiceProvider ServiceProvider { get; set; }

        public override IDatabase CreatePopulation() => this.CreateDatabase();

        public override IDatabase CreateDatabase() => new Database(this.ServiceProvider, new Configuration { ObjectFactory = this.ObjectFactory });
    }
}
