// <copyright file="DomainTest.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the DomainTest type.</summary>

namespace Tests
{
    using System;
    using System.IO;
    using System.Reflection;
    using Allors;
    using Allors.Database.Adapters.Memory;
    using Allors.Domain;
    using Allors.Domain.Derivations;
    using Allors.Meta;
    using Allors.Services;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using Configuration = Allors.Database.Adapters.Memory.Configuration;
    using ObjectFactory = Allors.ObjectFactory;

    public class DomainTest : IDisposable
    {
        public DomainTest(bool populate = true) => this.Setup(populate);

        public virtual Config Config { get; } = new Config { SetupSecurity = false };

        public ISession Session { get; private set; }

        public ITimeService TimeService => this.Session.ServiceProvider.GetRequiredService<ITimeService>();

        public IDerivationService DerivationService => this.Session.ServiceProvider.GetRequiredService<IDerivationService>();

        public TimeSpan? TimeShift
        {
            get => this.TimeService.Shift;

            set => this.TimeService.Shift = value;
        }

        protected ObjectFactory ObjectFactory => new ObjectFactory(MetaPopulation.Instance, typeof(User));

        public Mock<IAccessControlLists> AclsMock
        {
            get
            {
                var aclMock = new Mock<IAccessControlList>();
                aclMock.Setup(acl => acl.CanRead(It.IsAny<IPropertyType>())).Returns(true);
                aclMock.Setup(acl => acl.CanRead(It.IsAny<IConcreteRoleType>())).Returns(true);
                var aclsMock = new Mock<IAccessControlLists>();
                aclsMock.Setup(acls => acls[It.IsAny<IObject>()]).Returns(aclMock.Object);
                return aclsMock;
            }
        }

        public void Dispose()
        {
            this.Session.Rollback();
            this.Session = null;
        }

        protected void Setup(bool populate)
        {
#if ALLORS_DERIVATION_DEBUG
            var derivationPersistent = true;
#else
            var environmentVariable = Environment.GetEnvironmentVariable("ALLORS_DERIVATION");
            var derivationPersistent = environmentVariable?.ToLowerInvariant().Equals("persistent") == true;
#endif

            var services = new ServiceCollection();
            if (derivationPersistent)
            {
                services.AddAllors((session) => new Allors.Domain.Derivations.Persistent.Derivation(session, new DerivationConfig { MaxCycles = 10, MaxIterations = 10, MaxPreparations = 10 }));
            }
            else
            {
                services.AddAllors((session) => new Allors.Domain.Derivations.Default.Derivation(session, new DerivationConfig { MaxCycles = 10, MaxIterations = 10, MaxPreparations = 10 }));
            }

            var serviceProvider = services.BuildServiceProvider();

            var database = new Database(
                serviceProvider,
                new Configuration
                {
                    ObjectFactory = this.ObjectFactory,
                });
            serviceProvider.GetRequiredService<IDatabaseService>().Database = database;
            this.Setup(database, populate);
        }

        protected void Setup(IDatabase database, bool populate)
        {
            database.Init();

            this.Session = database.CreateSession();

            if (populate)
            {
                new Setup(this.Session, this.Config).Apply();
                this.Session.Commit();
            }
        }

        protected Stream GetResource(string name)
        {
            var assembly = this.GetType().GetTypeInfo().Assembly;
            var resource = assembly.GetManifestResourceStream(name);
            return resource;
        }

        protected byte[] GetResourceBytes(string name)
        {
            var assembly = this.GetType().GetTypeInfo().Assembly;
            var resource = assembly.GetManifestResourceStream(name);
            using (var ms = new MemoryStream())
            {
                resource.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
