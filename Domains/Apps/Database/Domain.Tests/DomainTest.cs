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
    using Allors.Domain;

    public class DomainTest : IDisposable
    {
        protected ObjectFactory ObjectFactory => new ObjectFactory(MetaPopulation.Instance, typeof(Singleton).GetTypeInfo().Assembly, typeof(Singleton).Namespace);
        
        private ISession databaseSession;

        public DomainTest() : this(true)
        {
        }

        public DomainTest(bool setup)
        {
            var configuration = new Allors.Adapters.Memory.Configuration { ObjectFactory = this.ObjectFactory };
            var database = new Allors.Adapters.Memory.Database(configuration);
            database.Init();

            if (setup)
            {
                Fixture.Setup(database);
            }

            this.databaseSession = database.CreateSession();
        }

        public void Dispose()
        {
            this.databaseSession.Rollback();
            this.databaseSession = null;
        }

        public ISession DatabaseSession => this.databaseSession;

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