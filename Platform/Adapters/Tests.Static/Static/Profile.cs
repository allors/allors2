// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Profile.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Adapters
{
    using System;
    using System.Collections.Generic;

    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    public abstract class Profile : IProfile
    {
        private readonly ObjectFactory objectFactory;

        private IDatabase database;
        private ISession session;

        protected Profile()
        {
            this.objectFactory = this.CreateObjectFactory(MetaPopulation.Instance);
        }

        public IObjectFactory ObjectFactory => this.objectFactory;

        public ISession Session => this.session;

        public IDatabase Database => this.database;

        public abstract Action[] Markers { get; }

        public virtual Action[] Inits
        {
            get
            {
                var inits = new List<Action> { this.Init };

                if (Settings.ExtraInits)
                {
                    inits.Add(this.Init);
                }

                return inits.ToArray();
            }
        }

        public void SwitchDatabase()
        {
            this.session.Rollback();
            this.database = this.CreateDatabase();
            this.session = this.database.CreateSession();
            this.session.Commit();
        }

        public virtual void Dispose()
        {
            this.session?.Rollback();

            this.session = null;
            this.database = null;
        }

        public abstract IDatabase CreatePopulation();

        public abstract IDatabase CreateDatabase();

        internal ISession CreateSession()
        {
            return this.database.CreateSession();
        }

        protected internal void Init()
        {
            try
            {
                this.session?.Rollback();

                this.database = this.CreateDatabase();
                this.database.Init();
                this.session = this.database.CreateSession();
                this.session.Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
        }

        protected ObjectFactory CreateObjectFactory(IMetaPopulation metaPopulation)
        {
            return new ObjectFactory(metaPopulation, typeof(C1));
        }
    }
}