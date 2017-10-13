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

namespace Tests
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Security.Claims;
    using System.Security.Principal;

    using Allors;
    using Allors.Adapters.Memory;
    using Allors.Domain;
    using Allors.Meta;
    using Allors.Services;

    using Microsoft.Extensions.DependencyInjection;

    using Configuration = Allors.Adapters.Memory.Configuration;
    using Extent = Allors.Extent;
    using ObjectFactory = Allors.ObjectFactory;

    public class DomainTest : IDisposable
    {
        public DomainTest(bool populate = true)
        {
            this.Setup(populate);
        }

        public ISession Session { get; private set; }

        public ITimeService TimeService => this.Session.ServiceProvider.GetRequiredService<ITimeService>();

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
            services.AddAllors(Directory.GetCurrentDirectory());
            var serviceProvider = services.BuildServiceProvider();

            var configuration = new Configuration
                                    {
                                        ObjectFactory = this.ObjectFactory,
                                    };

            var database = new Database(serviceProvider, configuration);
            this.Setup(database, populate);
        }

        protected void Setup(IDatabase database, bool populate)
        {
            database.Init();
            
            this.Session = database.CreateSession();

            if (populate)
            {
                new Setup(this.Session, null).Apply();
                this.Session.Commit();
            }
        }

        protected void SetIdentity(string identity)
        {
            ClaimsPrincipal.ClaimsPrincipalSelector = () => new GenericPrincipal(new GenericIdentity(identity, "Forms"), new string[0]);
        }

        protected Stream GetResource(string name)
        {
            var assembly = this.GetType().GetTypeInfo().Assembly;
            var resource = assembly.GetManifestResourceStream(name);
            return resource;
        }

        protected IObject[] GetObjects(ISession session, Composite objectType)
        {
            return session.Extent(objectType);
        }

        protected void Derive(Extent extent, bool throwExceptionOnError = true)
        {
            var derivation = new Allors.Domain.NonLogging.Derivation(this.Session, extent.ToArray());
            var validation = derivation.Derive();
            if (validation.HasErrors)
            {
                throw new Exception("Derivation Error");
            }
        }

        protected void Derive(IObject @object, bool throwExceptionOnError = true)
        {
            var derivation = new Allors.Domain.NonLogging.Derivation(this.Session, new[] { @object });
            var validation = derivation.Derive();
            if (validation.HasErrors)
            {
                throw new Exception("Derivation Error");
            }
        }
    }
}