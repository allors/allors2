// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Profile.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Database.Adapters
{
    using System;

    using Allors;
    using Adapters;
    using Allors.Meta;

    public abstract class Profile : IDisposable
    {
        private ISession session;
        private ISession session2;

        public IObject[] CreateArray(ObjectType objectType, int count)
        {
            var type = objectType.ClrType;
            var allorsObjects = (IObject[])Array.CreateInstance(type, count);
            return allorsObjects;
        }

        public abstract IDatabase CreateMemoryPopulation();

        public ISession CreateSession()
        {
            return this.GetPopulation().CreateSession();
        }

        public ISession CreateSession2()
        {
            return this.GetPopulation2().CreateSession();
        }

        public virtual void Dispose()
        {
            if (this.session != null)
            {
                this.session.Rollback();
                this.session = null;
            }

            if (this.session2 != null)
            {
                this.session2.Rollback();
                this.session2 = null;
            }
        }

        public abstract IDatabase GetPopulation();

        public abstract IDatabase GetPopulation2();

        public ISession GetSession()
        {
            return this.session ?? (this.session = this.GetPopulation().CreateSession());
        }

        public ISession GetSession2()
        {
            return this.session2 ?? (this.session2 = this.GetPopulation2().CreateSession());
        }

        public abstract void Init();

        public abstract bool IsRollbackSupported();
    }
}
