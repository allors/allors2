//------------------------------------------------------------------------------------------------- 
// <copyright file="DomainTest.cs" company="Allors bvba">
// Copyright 2002-2009 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>Defines the DomainTest type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors
{
    using System;
    using System.IO;

    using Allors.Database.Adapters.Memory;
    using Allors.Domain;
    using Allors.Meta;
    using Allors.Services;

    using Microsoft.Extensions.DependencyInjection;

    public class DomainTest : IDisposable
    {
        public DomainTest(bool populate = true)
        {
            this.Setup(populate);
        }

        public virtual Config Config { get; } = new Config { SetupSecurity = false };

        public ISession Session { get; private set; }

        public ITimeService TimeService => this.Session.ServiceProvider.GetRequiredService<ITimeService>();

        public TimeSpan? TimeShift
        {
            get => this.TimeService.Shift;

            set => this.TimeService.Shift = value;
        }

        protected Person Administrator => this.GetUser("administrator");

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
                Fixture.Setup(database);

                this.Session.Commit();
            }
        }

        protected void SetIdentity(string identity)
        {
            var users = new Users(this.Session);
            var user = users.GetUser(identity) ?? new AutomatedAgents(this.Session).Guest;
            this.Session.SetUser(user);
        }

        private Person GetUser(string userName) => (Person)new Users(this.Session).GetUser(userName);
    }
}