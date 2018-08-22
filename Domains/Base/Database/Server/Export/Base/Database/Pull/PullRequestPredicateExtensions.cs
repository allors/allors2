// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PullRequestPredicateExtensions.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
//
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
//
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
//
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Server
{
    using System;
    using System.Linq;

    using Allors.Domain.Query;
    using Allors.Meta;
    using Allors.Protocol.Remote;
    using Allors.Protocol.Remote.Pull;

    using Predicate = Allors.Domain.Query.Predicate;

    public static class PullRequestPredicateExtensions
    {
        public static Predicate Parse(this PullRequestPredicate @this, MetaPopulation metaPopulation)
        {
            switch (@this._T)
            {
                case "And": return new And { Predicates = @this.PS.Select(v => v.Parse(metaPopulation)).ToList() };

                case "Between": return @this.Between(metaPopulation);

                case "ContainedIn": return @this.ContainedIn(metaPopulation);

                case "Contains": return @this.Contains(metaPopulation);

                case "Equals": return @this.Equalz(metaPopulation);

                case "Exists": return @this.Exists(metaPopulation);

                case "GreaterThan": return @this.GreaterThan(metaPopulation);

                case "Instanceof": return @this.Instanceof(metaPopulation);

                case "LessThan": return @this.LessThan(metaPopulation);

                case "Like": return @this.Like(metaPopulation);

                case "Not": return new Not { Predicate = @this.P.Parse(metaPopulation) };

                case "Or": return new Or { Predicates = @this.PS.Select(v => v.Parse(metaPopulation)).ToList() };

                default:
                    throw new NotSupportedException($"Predicate {@this._T} is not supported");
            }
        }

        private static Predicate Between(this PullRequestPredicate @this, MetaPopulation metaPopulation)
        {
            var roleType = @this.RT != null ? (RoleType)metaPopulation.Find(new Guid(@this.RT)) : null;
            var first = @this.GetUnitRole(roleType, @this.F);
            var second = @this.GetUnitRole(roleType, @this.S);

            var predicate = new Between
                                {
                                    RoleType = roleType,
                                    First = first,
                                    Second = second
                                };

            return predicate;
        }

        private static Predicate ContainedIn(this PullRequestPredicate @this, MetaPopulation metaPopulation)
        {
            var predicate = new ContainedIn
                                {
                                    AssociationType = @this.AT != null ? (AssociationType)metaPopulation.Find(new Guid(@this.AT)) : null,
                                    RoleType = @this.RT != null ? (RoleType)metaPopulation.Find(new Guid(@this.RT)) : null,
                                    Query = @this.Q.Parse(metaPopulation),
                                    ObjectIds = @this.OS?.Select(long.Parse).ToArray()
                                };

            return predicate;
        }

        private static Predicate Contains(this PullRequestPredicate @this, MetaPopulation metaPopulation)
        {
            var predicate = new Contains
                                {
                                    AssociationType = @this.AT != null ? (AssociationType)metaPopulation.Find(new Guid(@this.AT)) : null,
                                    RoleType = @this.RT != null ? (RoleType)metaPopulation.Find(new Guid(@this.RT)) : null,
                                    ObjectId = @this.O != null ? long.Parse(@this.O) : (long?)null
                                };

            return predicate;
        }

        private static Predicate Equalz(this PullRequestPredicate @this, MetaPopulation metaPopulation)
        {
            var roleType = @this.GetRoleType(metaPopulation);
            var value = @this.GetUnitRole(roleType, @this.V);

            long? objectIdNullable = null;
            if (long.TryParse(@this.O, out var objectId))
            {
                objectIdNullable = objectId;
            }

            var predicate = new Equals
                                {
                                    AssociationType = @this.AT != null ? (AssociationType)metaPopulation.Find(new Guid(@this.AT)) : null,
                                    RoleType = roleType,
                                    Value = value,
                                    ObjectId = objectIdNullable
            };

            return predicate;
        }

        private static Predicate Exists(this PullRequestPredicate @this, MetaPopulation metaPopulation)
        {
            var predicate = new Exists
                                {
                                    AssociationType = @this.AT != null ? (AssociationType)metaPopulation.Find(new Guid(@this.AT)) : null,
                                    RoleType = @this.RT != null ? (RoleType)metaPopulation.Find(new Guid(@this.RT)) : null,
                                };

            return predicate;
        }

        private static Predicate GreaterThan(this PullRequestPredicate @this, MetaPopulation metaPopulation)
        {
            var roleType = @this.RT != null ? (RoleType)metaPopulation.Find(new Guid(@this.RT)) : null;
            var value = @this.GetUnitRole(roleType, @this.V);

            var predicate = new GreaterThan
                                {
                                    RoleType = roleType,
                                    Value = value
                               };

            return predicate;
        }

        private static Predicate Instanceof(this PullRequestPredicate @this, MetaPopulation metaPopulation)
        {
            var predicate = new Instanceof
                                {
                                    AssociationType = @this.AT != null ? (AssociationType)metaPopulation.Find(new Guid(@this.AT)) : null,
                                    RoleType = @this.RT != null ? (RoleType)metaPopulation.Find(new Guid(@this.RT)) : null,
                                    ObjectType = (IComposite)metaPopulation.Find(new Guid(@this.OT)),
                                };

            return predicate;
        }

        private static Predicate LessThan(this PullRequestPredicate @this, MetaPopulation metaPopulation)
        {
            var roleType = @this.RT != null ? (RoleType)metaPopulation.Find(new Guid(@this.RT)) : null;
            var value = @this.GetUnitRole(roleType, @this.V);

            var predicate = new LessThan
            {
                                    RoleType = roleType,
                                    Value = value,
                                };

            return predicate;
        }

        private static Predicate Like(this PullRequestPredicate @this, MetaPopulation metaPopulation)
        {
            var predicate = new Like
                                {
                                    RoleType = (RoleType)metaPopulation.Find(new Guid(@this.RT)),
                                    Value = (string)@this.V
                                };

            return predicate;
        }

        private static RoleType GetRoleType(this PullRequestPredicate @this, MetaPopulation metaPopulation)
        {
            return @this.RT != null ? (RoleType)metaPopulation.Find(new Guid(@this.RT)) : null;
        }

        private static object GetUnitRole(this PullRequestPredicate @this, RoleType roleType, object value)
        {
            if (value != null)
            {
                var unit = roleType?.ObjectType as Unit;
                if (unit != null)
                {
                    var stringValue = value as string;
                    if (stringValue != null)
                    {
                        switch (unit.UnitTag)
                        {
                            case UnitTags.Binary: return Convert.FromBase64String(stringValue);
                            case UnitTags.Boolean: return bool.Parse(stringValue);
                            case UnitTags.DateTime: return Convert.ToDateTime(stringValue);
                            case UnitTags.Decimal: return Convert.ToDecimal(stringValue);
                            case UnitTags.Float: return Convert.ToDouble(stringValue);
                            case UnitTags.Integer: return Convert.ToInt32(stringValue);
                            case UnitTags.Unique: return Guid.Parse(stringValue);
                        }
                    }
                    else
                    {
                        switch (unit.UnitTag)
                        {
                            case UnitTags.DateTime: return Convert.ToDateTime(value);
                            case UnitTags.Decimal: return Convert.ToDecimal(value);
                            case UnitTags.Float: return Convert.ToDouble(value);
                            case UnitTags.Integer: return Convert.ToInt32(value);
                        }
                    }
                }
            }

            return value;
        }
    }
}