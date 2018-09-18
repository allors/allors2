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

namespace Allors.Data.Protocol
{
    using System;
    using System.Linq;

    using Allors.Data;
    using Allors.Meta;

    public class Predicate
    {
        public string Kind { get; set; }

        public Guid? PropertyType { get; set; }

        public Guid? ObjectType { get; set; }

        public string Parameter { get; set; }

        public Predicate Operand { get; set; }

        public Predicate[] Operands { get; set; }

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
                    if (!this.PropertyType.HasValue)
                    {
                        throw new Exception("Missing PropertyType");
                    }

                    var propertyType = (IPropertyType)session.Database.ObjectFactory.MetaPopulation.Find(this.PropertyType.Value);
                    var roleType = propertyType as IRoleType;

                    switch (this.Kind)
                    {
                        case PredicateKind.Instanceof:

                            return new Instanceof(this.ObjectType != null ? (IComposite)session.Database.MetaPopulation.Find(this.ObjectType.Value) : null)
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
                            if (this.Value != null)
                            {
                                if (propertyType.ObjectType.IsUnit)
                                {
                                    var value = Convert.ToValue((IUnit)((IRoleType)propertyType)?.ObjectType, this.Value);
                                    equals.Value = value;
                                }
                                else
                                {
                                    equals.Object = session.Instantiate(this.Value);
                                }
                            }

                            return equals;

                        case PredicateKind.Contains:

                            return new Contains
                            {
                                PropertyType = propertyType,
                                Parameter = this.Parameter,
                                Object = session.Instantiate(this.Value)
                            };

                        case PredicateKind.ContainedIn:

                            var containedIn = new ContainedIn(propertyType) { Parameter = this.Parameter };
                            if (this.Value != null)
                            {
                                containedIn.Objects = this.Values.Select(session.Instantiate).ToArray();
                            }
                            else if (this.Extent != null)
                            {
                                containedIn.Extent = this.Extent.Load(session);
                            }

                            return containedIn;


                        case PredicateKind.Between:

                            return new Between(roleType)
                            {
                                Parameter = this.Parameter,
                                Values = this.Values.Select(v => Convert.ToValue((IUnit)roleType?.ObjectType, v)).ToArray()
                            };

                        case PredicateKind.GreaterThan:

                            return new GreaterThan(roleType)
                            {
                                Parameter = this.Parameter,
                                Value = Convert.ToValue((IUnit)roleType?.ObjectType, this.Value)
                            };

                        case PredicateKind.LessThan:

                            return new LessThan(roleType)
                            {
                                Parameter = this.Parameter,
                                Value = Convert.ToValue((IUnit)roleType?.ObjectType, this.Value)
                            };

                        case PredicateKind.Like:

                            return new Like(roleType)
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