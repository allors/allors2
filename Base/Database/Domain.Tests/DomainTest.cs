// <copyright file="DomainTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the DomainTest type.</summary>

namespace Allors
{
    using System;
    using System.Linq;
    using Allors.Database.Adapters.Memory;
    using Allors.Domain;
    using Allors.Meta;
    using Allors.Services;
    using Microsoft.Extensions.DependencyInjection;

    public class DomainTest : IDisposable
    {
        public DomainTest(bool populate = true) => this.Setup(populate);

        public virtual Config Config { get; } = new Config { SetupSecurity = false };

        public ISession Session { get; private set; }

        public ITimeService TimeService => this.Session.ServiceProvider.GetRequiredService<ITimeService>();

        protected Organisation InternalOrganisation => this.Session.Extent<Organisation>().First(v => v.IsInternalOrganisation);

        public TimeSpan? TimeShift
        {
            get => this.TimeService.Shift;

            set => this.TimeService.Shift = value;
        }

        protected ObjectFactory ObjectFactory => new ObjectFactory(MetaPopulation.Instance, typeof(User));

        public void Dispose()
        {
            this.Session.Rollback();
            this.Session = null;
        }

        protected void Setup(bool populate)
        {
            var services = new ServiceCollection();
            services.AddAllors();
            var serviceProvider = services.BuildServiceProvider();

            var configuration = new Configuration
            {
                ObjectFactory = this.ObjectFactory,
            };

            var database = new Database.Adapters.Memory.Database(serviceProvider, configuration);
            this.Setup(database, populate);
        }

        protected void Setup(IDatabase database, bool populate)
        {
            database.Init();

            this.Session = database.CreateSession();

            if (populate)
            {
                Fixture.Setup(database, this.Config);
                this.Session.Commit();
            }
        }

        protected void SetIdentity(string identity)
        {
            var users = new Users(this.Session);
            var user = users.GetUser(identity) ?? this.Session.GetSingleton().Guest;
            this.Session.SetUser(user);
        }
    }
}
