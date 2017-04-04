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

using System;
using System.IO;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;
using Allors.Domain;

namespace Domain
{
    using Allors;
    using Allors.Meta;

    public class DomainTest : IDisposable
    {
        protected ISession Session { get; private set; }

        public DomainTest()
        {
            this.ObjectFactory = new ObjectFactory(MetaPopulation.Instance, typeof(IObject), typeof(User));
            var configuration = new Allors.Adapters.Memory.Configuration { ObjectFactory = this.ObjectFactory };
            var database = new Allors.Adapters.Memory.Database(configuration);

            this.SetUp(database, true);
        }

        protected ObjectFactory ObjectFactory { get; }

        protected void SetUp(IDatabase database, bool setup)
        {
            database.Init();
            this.Session = database.CreateSession();

            if (setup)
            {
                new Setup(this.Session, null).Apply();
                this.Session.Commit();
            }
        }

        public void Dispose()
        {
            this.Session.Rollback();
            this.Session = null;
        }

        protected IObject[] GetObjects(ISession session, Composite objectType)
        {
            return session.Extent(objectType);
        }

        protected Stream GetResource(string name)
        {
            var assembly = this.GetType().GetTypeInfo().Assembly;
            var resource = assembly.GetManifestResourceStream(name);
            return resource;
        }

        protected void SetIdentity(string identity)
        {
            ClaimsPrincipal.ClaimsPrincipalSelector = () => new GenericPrincipal(new GenericIdentity(identity, "Forms"), new string[0]);
        }
    }
}