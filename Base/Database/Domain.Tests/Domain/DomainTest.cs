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

namespace Domain
{
    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using NUnit.Framework;

    public class DomainTest
    {
        protected ISession Session { get; private set; }

        [SetUp]
        public virtual void SetUp()
        {
            this.SetUp(true);
        }

        [TearDown]
        public virtual void TearDown()
        {
            this.Session.Rollback();
            this.Session = null;
        }

        protected void SetUp(bool setup)
        {
            Config.Default.Init();
            this.Session = Config.Default.CreateSession();
            
            if (setup)
            {
                new Setup(this.Session, null).Apply();
                this.Session.Commit();
            }
        }

        protected IObject[] GetObjects(ISession session, Composite objectType)
        {
            return session.Extent(objectType);
        }
    }
}