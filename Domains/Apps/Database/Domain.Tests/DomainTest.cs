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
using System.Security.Claims;
using System.Security.Principal;

namespace Allors
{
    using System.IO;
    using System.Xml;

    using Allors.Meta;

    public class DomainTest : IDisposable
    {
        private ISession databaseSession;

        public DomainTest() : this(true)
        {
        }

        public DomainTest(bool setup)
        {
            if (setup)
            {
                var stringReader = new StringReader(Fixture.FullXml);
                var reader = XmlReader.Create(stringReader);
                Config.Default.Load(reader);
            }
            else
            {
                Config.Default.Init();
            }

            this.databaseSession = Config.Default.CreateSession();
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