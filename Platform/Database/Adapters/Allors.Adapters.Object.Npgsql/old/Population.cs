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

        /// <summary>
        /// Assert that the unit is compatible with the IObjectType of the RoleType.
        /// </summary>
        /// <param name="unit">
        /// The unit .
        /// </param>
        /// <param name="roleType">
        /// The role type.
        /// </param>
        /// <returns>
        /// The normalize.
        /// </returns>
        public object Internalize(object unit, IRoleType roleType)
        {
            var unitType = (IUnit)roleType.ObjectType;
            var unitTypeTag = unitType.UnitTag;

            var normalizedUnit = unit;

            switch (unitTypeTag)
            {
                case UnitTags.String:
                    if (!(unit is string))
                    {
                        throw new ArgumentException("RoleType is not a String.");
                    }

                    var stringUnit = (string)unit;
                    var size = roleType.Size;
                    if (size > -1 && stringUnit.Length > size)
                    {
                        throw new ArgumentException("Size of relationType " + roleType + " is too big (" + stringUnit.Length + ">" + size + ").");
                    }

                    break;
                case UnitTags.Integer:
                    if (!(unit is int))
                    {
                        throw new ArgumentException("RoleType is not an Integer.");
                    }

                    break;
                case UnitTags.Decimal:
                    if (unit is int || unit is long || unit is float || unit is double)
                    {
                        normalizedUnit = Convert.ToDecimal(unit);
                    }
                    else if (!(unit is decimal))
                    {
                        throw new ArgumentException("RoleType is not a Decimal.");
                    }

                    break;
                case UnitTags.Float:
                    if (unit is int || unit is long || unit is float)
                    {
                        normalizedUnit = Convert.ToDouble(unit);
                    }
                    else if (!(unit is double))
                    {
                        throw new ArgumentException("RoleType is not a Double.");
                    }

                    break;
                case UnitTags.Boolean:
                    if (!(unit is bool))
                    {
                        throw new ArgumentException("RoleType is not a Boolean.");
                    }

                    break;
                case UnitTags.DateTime:
                    if (!(unit is DateTime))
                    {
                        throw new ArgumentException("RoleType is not a DateTime.");
                    }

                    var dateTime = (DateTime)unit;

                    if (dateTime == DateTime.MaxValue || dateTime == DateTime.MinValue)
                    {
                        normalizedUnit = unit;
                    }
                    else
                    {
                        normalizedUnit = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, DateTimeKind.Utc);
                    }

                    break;
                case UnitTags.Unique:
                    if (!(unit is Guid))
                    {
                        throw new ArgumentException("RoleType is not a Boolean.");
                    }

                    break;
                case UnitTags.Binary:
                    if (!(unit is byte[]))
                    {
                        throw new ArgumentException("RoleType is not a Boolean.");
                    }

                    break;
                default:
                    throw new ArgumentException("Unknown Unit IObjectType: " + unitTypeTag);
            }

            return normalizedUnit;
        }

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