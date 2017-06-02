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
    using Allors.Meta;
    using System;
    using System.Reflection;
    using System.Security.Claims;
    using System.Security.Principal;

    using Allors.Adapters.Memory;
    using Allors.Domain;
    using Allors.Services.Base;

    public class DomainTest : IDisposable
    {
        public DomainTest()
        {
            var configuration = new Configuration
                                    {
                                        ObjectFactory = new ObjectFactory(MetaPopulation.Instance, typeof(User)),
                                    };

            var database = new Database(configuration);

            database.Init();

            var userService = new ClaimsPrincipalUserService();
            var timeService = new TimeService();
            var mailService = new MailService { DefaultSender = "noreply@example.com" };
            var securityService = new SecurityService();
            var serviceLocator = new ServiceLocator
                                     {
                                         UserServiceFactory = () => userService,
                                         TimeServiceFactory = () => timeService,
                                         MailServiceFactory = () => mailService,
                                         SecurityServiceFactory = () => securityService
                                     };
            database.SetServiceLocator(serviceLocator.Assert());

            Fixture.Setup(database);

            this.DatabaseSession = database.CreateSession();

            this.SetIdentity(Users.AdministratorUserName);
        }

        public ISession DatabaseSession { get; private set; }

        public void Dispose()
        {
            this.DatabaseSession.Rollback();
            this.DatabaseSession = null;
        }

        protected IObject[] GetObjects(ISession session, Composite objectType)
        {
            return session.Extent(objectType);
        }

        protected void SetIdentity(string identity)
        {
            ClaimsPrincipal.ClaimsPrincipalSelector = () => new GenericPrincipal(new GenericIdentity(identity, "Forms"), new string[0]);
        }
    }
}