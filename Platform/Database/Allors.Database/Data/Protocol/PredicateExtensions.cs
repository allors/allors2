// <copyright file="PredicateExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Data
{
    using System;
    using System.Linq;

    using Allors.Data;
    using Allors.Meta;

    public static class PredicateExtensions
    {
        public static IPredicate Load(this Predicate @this, ISession session)
        {
            switch (@this.kind)
            {
                case PredicateKind.And:
                    return new And
                    {
                        Dependencies = @this.dependencies,
                        Operands = @this.operands.Select(v => v.Load(session)).ToArray(),
                    };

                case PredicateKind.Or:
                    return new Or
                    {
                        Dependencies = @this.dependencies,
                        Operands = @this.operands.Select(v => v.Load(session)).ToArray(),
                    };

                case PredicateKind.Not:
                    return new Not
                    {
                        Dependencies = @this.dependencies,
                        Operand = @this.operand.Load(session),
                    };

                default:
                    var propertyType = @this.propertyType != null ? (IPropertyType)session.Database.ObjectFactory.MetaPopulation.Find(@this.propertyType.Value) : null;
                    var roleType = @this.roleType != null ? (IRoleType)session.Database.ObjectFactory.MetaPopulation.Find(@this.roleType.Value) : null;

                    switch (@this.kind)
                    {
                        case PredicateKind.InstanceOf:

                            return new Instanceof(@this.objectType != null ? (IComposite)session.Database.MetaPopulation.Find(@this.objectType.Value) : null)
                            {
                                Dependencies = @this.dependencies,
                                PropertyType = propertyType,
                            };

                        case PredicateKind.Exists:

                            return new Exists
                            {
                                Dependencies = @this.dependencies,
                                PropertyType = propertyType,
                                Parameter = @this.parameter,
                            };

                        case PredicateKind.Contains:

                            return new Contains
                            {
                                Dependencies = @this.dependencies,
                                PropertyType = propertyType,
                                Parameter = @this.parameter,
                                Object = session.Instantiate(@this.@object),
                            };

                        case PredicateKind.ContainedIn:

                            var containedIn = new ContainedIn(propertyType)
                            {
                                Dependencies = @this.dependencies,
                                Parameter = @this.parameter
                            };

                            if (@this.objects != null)
                            {
                                containedIn.Objects = @this.objects.Select(session.Instantiate).ToArray();
                            }
                            else if (@this.extent != null)
                            {
                                containedIn.Extent = @this.extent.Load(session);
                            }

                            return containedIn;

                        case PredicateKind.Equals:

                            var equals = new Equals(propertyType)
                            {
                                Dependencies = @this.dependencies,
                                Parameter = @this.parameter
                            };

                            if (@this.@object != null)
                            {
                                equals.Object = session.Instantiate(@this.@object);
                            }
                            else if (@this.value != null)
                            {
                                var value = UnitConvert.Parse(((IRoleType)propertyType).ObjectType.Id, @this.value);
                                equals.Value = value;
                            }

                            return equals;

                        case PredicateKind.Between:

                            return new Between(roleType)
                            {
                                Dependencies = @this.dependencies,
                                Parameter = @this.parameter,
                                Values = @this.values?.Select(v => UnitConvert.Parse(roleType.ObjectType.Id, v)).ToArray(),
                            };

                        case PredicateKind.GreaterThan:

                            return new GreaterThan(roleType)
                            {
                                Dependencies = @this.dependencies,
                                Parameter = @this.parameter,
                                Value = UnitConvert.Parse(roleType.ObjectType.Id, @this.value),
                            };

                        case PredicateKind.LessThan:

                            return new LessThan(roleType)
                            {
                                Dependencies = @this.dependencies,
                                Parameter = @this.parameter,
                                Value = UnitConvert.Parse(roleType.ObjectType.Id, @this.value),
                            };

                        case PredicateKind.Like:

                            return new Like(roleType)
                            {
                                Dependencies = @this.dependencies,
                                Parameter = @this.parameter,
                                Value = UnitConvert.Parse(roleType.ObjectType.Id, @this.value)?.ToString(),
                            };

                        default:
                            throw new Exception("Unknown predicate kind " + @this.kind);
                    }
            }
        }
    }
}
