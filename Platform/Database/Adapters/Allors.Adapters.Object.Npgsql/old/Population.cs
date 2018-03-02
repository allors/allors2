// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Population.cs" company="Allors bvba">
//   Copyright 2002-2013 Allors bvba.
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
    using System.Xml;

    using Allors.Meta;

    public abstract class Population
    {
        private readonly IObjectFactory objectFactory;

        private readonly Dictionary<IObjectType, HashSet<IObjectType>> concreteClassesByObjectType;

        protected Dictionary<string, object> Properties;

        protected Population(Configuration configuration)
        {
            this.objectFactory = configuration.ObjectFactory;
            this.concreteClassesByObjectType = new Dictionary<IObjectType, HashSet<IObjectType>>();
        }

        public IObjectFactory ObjectFactory
        {
            get
            {
                return this.objectFactory;
            }
        }

        public abstract string Id { get; }

        public abstract bool IsDatabase { get; }

        public abstract bool IsWorkspace { get; }

        public IMetaPopulation MetaPopulation 
        {
            get
            {
                return this.objectFactory.MetaPopulation;
            }
        }

        public object this[string name]
        {
            get
            {
                if (this.Properties == null)
                {
                    return null;
                }

                object value;
                this.Properties.TryGetValue(name, out value);
                return value;
            }

            set
            {
                if (this.Properties == null)
                {
                    this.Properties = new Dictionary<string, object>();
                }

                if (value == null)
                {
                    this.Properties.Remove(name);
                }
                else
                {
                    this.Properties[name] = value;
                }
            }
        }
        
        public abstract ISession CreateSession();
       
        public abstract void Load(XmlReader reader);

        public abstract void Save(XmlWriter writer);

        internal bool ContainsConcreteClass(IObjectType container, IObjectType containee)
        {
            if (container.IsClass)
            {
                return container.Equals(containee);
            }

            HashSet<IObjectType> concreteClasses;
            if (!this.concreteClassesByObjectType.TryGetValue(container, out concreteClasses))
            {
                concreteClasses = new HashSet<IObjectType>(((IInterface)container).Subclasses);
                this.concreteClassesByObjectType[container] = concreteClasses;
            }

            return concreteClasses.Contains(containee);
        }
   }
}