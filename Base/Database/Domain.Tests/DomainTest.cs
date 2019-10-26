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
    using Bogus;
    using Microsoft.Extensions.DependencyInjection;
    using Person = Domain.Person;

    public class DomainTest : IDisposable
    {
        public DomainTest(bool populate = true) => this.Setup(populate);

        public virtual Config Config { get; } = new Config { SetupSecurity = false };

        public ISession Session { get; private set; }

        public ITimeService TimeService => this.Session.ServiceProvider.GetRequiredService<ITimeService>();

        public TimeSpan? TimeShift
        {
            get => this.TimeService.Shift;

            set => this.TimeService.Shift = value;
        }

        protected Organisation InternalOrganisation => this.Session.Extent<Organisation>().First(v => v.IsInternalOrganisation);

        protected Person Administrator => this.GetUser("administrator");

        protected Person OrderProcessor => this.GetUser("orderProcessor");

        protected Person SalesRep => this.GetUser("salesRep");

        protected Person Purchaser => this.GetUser("purchaser");

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
            services.AddSingleton<Faker>();
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

        private Person GetUser(string userName) => (Person)new Users(this.Session).GetUser(userName);
    }
}
