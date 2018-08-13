//------------------------------------------------------------------------------------------------- 
// <copyright file="Predicate.cs" company="Allors bvba">
// Copyright 2002-2017 Allors bvba.
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
//-------------------------------------------------------------------------------------------------

namespace Allors.Data.Schema
{
    using System;
    using System.Linq;

    using Allors.Meta;

    public class Predicate
    {
        public string Kind { get; set; }

        public Guid PropertyType { get; set; }

        public Guid ObjectType { get; set; }

        public string Parameter { get; set; }

        public Predicate Operand { get; set; }

        public Predicate[] Operands { get; set; }

        public string Object { get; set; }

        public string[] Objects { get; set; }

        public string Value { get; set; }

        public string[] Values { get; set; }

        public Extent Extent { get; set; }

        public IPredicate Load(ISession session)
        {
            switch (this.Kind)
            {
                case PredicateKind.And:
                    return new And
                    {
                        Operands = this.Operands.Select(v => v.Load(session)).ToArray()
                    };

                case PredicateKind.Or:
                    return new Or
                    {
                        Operands = this.Operands.Select(v => v.Load(session)).ToArray()
                    };

                case PredicateKind.Not:
                    return new Not
                    {
                        Operand = this.Operand.Load(session)
                    };

                default:
                    var propertyType = (IPropertyType)session.Database.ObjectFactory.MetaPopulation.Find(this.PropertyType);
                    switch (this.Kind)
                    {
                        case PredicateKind.Instanceof:

                            return new Instanceof((IComposite)session.Database.MetaPopulation.Find(this.ObjectType))
                            {
                                PropertyType = propertyType
                            };

                        case PredicateKind.Exists:

                            return new Exists
                            {
                                PropertyType = propertyType
                            };

                        case PredicateKind.Equals:

                            var equals = new Equals(propertyType) { Parameter = this.Parameter };
                            if (this.Object != null)
                            {
                                equals.Object = session.Instantiate(this.Object);
                            }
                            else if (this.Value != null)
                            {
                                var roleType = (IRoleType)propertyType;
                                var value = Convert.ToValue((IUnit)roleType.ObjectType, this.Value);
                                equals.Value = value;
                            }

                            return equals;

                        case PredicateKind.Contains:

                            return new Contains
                            {
                                PropertyType = propertyType,
                                Parameter = this.Parameter,
                                Object = session.Instantiate(this.Object)
                            };

                        case PredicateKind.ContainedIn:

                            var containedIn = new ContainedIn(propertyType) { Parameter = this.Parameter };
                            if (this.Objects != null)
                            {
                                containedIn.Objects = this.Objects.Select(session.Instantiate).ToArray();
                            }
                            else if (this.Extent != null)
                            {
                                containedIn.Extent = this.Extent.Load(session);
                            }

                            return containedIn;


                        case PredicateKind.Between:

                            return new Between((IRoleType)propertyType)
                            {
                                Parameter = this.Parameter,
                                Values = this.Values.Select(v => Convert.ToValue((IUnit)((IRoleType)propertyType).ObjectType, v)).ToArray()
                            };

                        case PredicateKind.GreaterThan:

                            return new GreaterThan((IRoleType)propertyType)
                            {
                                Parameter = this.Parameter,
                                Value = Convert.ToValue((IUnit)((IRoleType)propertyType).ObjectType, this.Value)
                            };

                        case PredicateKind.LessThan:

                            return new LessThan((IRoleType)propertyType)
                            {
                                Parameter = this.Parameter,
                                Value = Convert.ToValue((IUnit)((IRoleType)propertyType).ObjectType, this.Value)
                            };

                        case PredicateKind.Like:

                            return new Like((IRoleType)propertyType)
                            {
                                Parameter = this.Parameter,
                                Value = this.Value
                            };

                        default:
                            throw new Exception("Unknown predicate kind " + this.Kind);
                    }
            }
        }
    }
}