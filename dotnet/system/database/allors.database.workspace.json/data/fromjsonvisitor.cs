// <copyright file="FromJsonVisitor.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Protocol.Json
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Protocol.Json;
    using Allors.Protocol.Json.Data;
    using Meta;
    using Extent = Data.Extent;
    using Node = Data.Node;
    using Pull = Data.Pull;
    using Result = Data.Result;
    using Select = Data.Select;
    using Sort = Data.Sort;
    using Procedure = Data.Procedure;

    public class FromJsonVisitor : Allors.Protocol.Json.Data.IVisitor
    {
        private readonly ITransaction transaction;
        private readonly IUnitConvert unitConvert;
        private readonly IMetaPopulation metaPopulation;

        private readonly Stack<Data.IExtent> extents;
        private readonly Stack<Data.IPredicate> predicates;
        private readonly Stack<Result> results;
        private readonly Stack<Select> selects;
        private readonly Stack<Node> nodes;
        private readonly Stack<Sort> sorts;

        public FromJsonVisitor(ITransaction transaction, IUnitConvert unitConvert)
        {
            this.transaction = transaction;
            this.unitConvert = unitConvert;
            this.metaPopulation = this.transaction.Database.ObjectFactory.MetaPopulation;

            this.extents = new Stack<Data.IExtent>();
            this.predicates = new Stack<Data.IPredicate>();
            this.results = new Stack<Result>();
            this.selects = new Stack<Select>();
            this.nodes = new Stack<Node>();
            this.sorts = new Stack<Sort>();
        }

        public Pull Pull { get; private set; }

        public Data.IExtent Extent => this.extents?.Peek();

        public Select Select => this.selects?.Peek();

        public Procedure Procedure { get; private set; }

        public void VisitExtent(Allors.Protocol.Json.Data.Extent visited)
        {
            Data.IExtentOperator extentOperator = null;
            Data.IExtent sortable = null;

            switch (visited.k)
            {
                case ExtentKind.Filter:
                    if (string.IsNullOrWhiteSpace(visited.t))
                    {
                        throw new Exception("Unknown object type for " + visited.k);
                    }

                    var objectType = (IComposite)this.metaPopulation.FindByTag(visited.t);
                    var extent = new Extent(objectType);
                    sortable = extent;

                    this.extents.Push(extent);

                    if (visited.p != null)
                    {
                        visited.p.Accept(this);
                        extent.Predicate = this.predicates.Pop();
                    }

                    break;

                case ExtentKind.Union:
                    extentOperator = new Data.Union();
                    break;

                case ExtentKind.Except:
                    extentOperator = new Data.Except();
                    break;

                case ExtentKind.Intersect:
                    extentOperator = new Data.Intersect();
                    break;

                default:
                    throw new Exception("Unknown extent kind " + visited.k);
            }

            sortable ??= extentOperator;

            if (visited.s?.Length > 0)
            {
                var length = visited.s.Length;

                sortable.Sorting = new Data.Sort[length];
                for (var i = 0; i < length; i++)
                {
                    var sorting = visited.s[i];
                    sorting.Accept(this);
                    sortable.Sorting[i] = this.sorts.Pop();
                }
            }

            if (extentOperator != null)
            {
                this.extents.Push(extentOperator);

                if (visited.o?.Length > 0)
                {
                    var length = visited.o.Length;

                    extentOperator.Operands = new Data.IExtent[length];
                    for (var i = 0; i < length; i++)
                    {
                        var operand = visited.o[i];
                        operand.Accept(this);
                        extentOperator.Operands[i] = this.extents.Pop();
                    }
                }
            }
        }

        public void VisitSelect(Allors.Protocol.Json.Data.Select visited)
        {
            var @select = new Select
            {
                PropertyType = (IPropertyType)this.metaPopulation.FindAssociationType(visited.a) ?? this.metaPopulation.FindRoleType(visited.r)
            };

            this.selects.Push(@select);

            if (visited.n != null)
            {
                visited.n.Accept(this);
                @select.Next = this.selects.Pop();
            }

            if (visited.i?.Length > 0)
            {
                @select.Include = new Node[visited.i.Length];
                for (var i = 0; i < visited.i.Length; i++)
                {
                    visited.i[i].Accept(this);
                    @select.Include[i] = this.nodes.Pop();
                }
            }
        }

        public void VisitNode(Allors.Protocol.Json.Data.Node visited)
        {
            var propertyType = (IPropertyType)this.metaPopulation.FindAssociationType(visited.a) ?? this.metaPopulation.FindRoleType(visited.r);
            var node = new Node(propertyType);

            this.nodes.Push(node);

            if (visited.n?.Length > 0)
            {
                foreach (var childNode in visited.n)
                {
                    childNode.Accept(this);
                    node.Add(this.nodes.Pop());
                }
            }
        }

        public void VisitPredicate(Predicate visited)
        {
            switch (visited.k)
            {
                case PredicateKind.And:
                    var and = new Data.And
                    {
                        Dependencies = visited.d,
                    };

                    this.predicates.Push(and);

                    if (visited.ops?.Length > 0)
                    {
                        var length = visited.ops.Length;

                        and.Operands = new Data.IPredicate[length];
                        for (var i = 0; i < length; i++)
                        {
                            var operand = visited.ops[i];
                            operand.Accept(this);
                            and.Operands[i] = this.predicates.Pop();
                        }
                    }

                    break;

                case PredicateKind.Or:
                    var or = new Data.Or
                    {
                        Dependencies = visited.d
                    };

                    this.predicates.Push(or);

                    if (visited.ops?.Length > 0)
                    {
                        var length = visited.ops.Length;

                        or.Operands = new Data.IPredicate[length];
                        for (var i = 0; i < length; i++)
                        {
                            var operand = visited.ops[i];
                            operand.Accept(this);
                            or.Operands[i] = this.predicates.Pop();
                        }
                    }

                    break;

                case PredicateKind.Not:
                    var not = new Data.Not
                    {
                        Dependencies = visited.d,
                    };

                    this.predicates.Push(not);

                    if (visited.op != null)
                    {
                        visited.op.Accept(this);
                        not.Operand = this.predicates.Pop();
                    }

                    break;

                default:
                    var associationType = this.metaPopulation.FindAssociationType(visited.a);
                    var roleType = this.metaPopulation.FindRoleType(visited.r);
                    var propertyType = (IPropertyType)associationType ?? roleType;

                    switch (visited.k)
                    {
                        case PredicateKind.InstanceOf:

                            var instanceOf = new Data.Instanceof(propertyType)
                            {
                                Dependencies = visited.d,
                                ObjectType = visited.o != null ? (IComposite)this.transaction.Database.MetaPopulation.FindByTag(visited.o) : null
                            };

                            this.predicates.Push(instanceOf);
                            break;

                        case PredicateKind.Exists:

                            var exists = new Data.Exists(propertyType)
                            {
                                Dependencies = visited.d,
                                Parameter = visited.p,
                            };

                            this.predicates.Push(exists);
                            break;

                        case PredicateKind.Contains:

                            var contains = new Data.Contains(propertyType)
                            {
                                Dependencies = visited.d,
                                Parameter = visited.p,
                                Object = visited.ob.HasValue ? this.transaction.Instantiate(visited.ob.Value) : null,
                            };

                            this.predicates.Push(contains);
                            break;

                        case PredicateKind.ContainedIn:

                            var containedIn = new Data.ContainedIn(propertyType)
                            {
                                Dependencies = visited.d,
                                Parameter = visited.p
                            };

                            this.predicates.Push(containedIn);

                            if (visited.obs != null)
                            {
                                containedIn.Objects = visited.obs.Select(this.transaction.Instantiate).ToArray();
                            }
                            else if (visited.e != null)
                            {
                                visited.e.Accept(this);
                                containedIn.Extent = this.extents.Pop();
                            }

                            break;

                        case PredicateKind.Equals:

                            var equals = new Data.Equals(propertyType)
                            {
                                Dependencies = visited.d,
                                Parameter = visited.p,
                                Path = this.metaPopulation.FindRoleType(visited.pa)
                            };

                            this.predicates.Push(equals);

                            if (visited.ob != null)
                            {
                                equals.Object = visited.ob.HasValue
                                    ? this.transaction.Instantiate(visited.ob.Value)
                                    : null;
                            }
                            else if (visited.v != null)
                            {
                                equals.Value = this.unitConvert.FromJson(((IRoleType)propertyType).ObjectType.Tag, visited.v);
                            }

                            break;

                        case PredicateKind.Between:

                            var between = new Data.Between(roleType)
                            {
                                Dependencies = visited.d,
                                Parameter = visited.p,
                                Values = visited.vs?.Select(v => this.unitConvert.FromJson(roleType.ObjectType.Tag, v)).ToArray(),
                                Paths = visited.pas?.Select(v => this.metaPopulation.FindRoleType(v)).ToArray()
                            };

                            this.predicates.Push(between);

                            break;

                        case PredicateKind.GreaterThan:

                            var greaterThan = new Data.GreaterThan(roleType)
                            {
                                Dependencies = visited.d,
                                Parameter = visited.p,
                                Value = this.unitConvert.FromJson(roleType.ObjectType.Tag, visited.v),
                                Path = this.metaPopulation.FindRoleType(visited.pa)
                            };

                            this.predicates.Push(greaterThan);

                            break;

                        case PredicateKind.LessThan:

                            var lessThan = new Data.LessThan(roleType)
                            {
                                Dependencies = visited.d,
                                Parameter = visited.p,
                                Value = this.unitConvert.FromJson(roleType.ObjectType.Tag, visited.v),
                                Path = this.metaPopulation.FindRoleType(visited.pa)
                            };

                            this.predicates.Push(lessThan);

                            break;

                        case PredicateKind.Like:

                            var like = new Data.Like(roleType)
                            {
                                Dependencies = visited.d,
                                Parameter = visited.p,
                                Value = this.unitConvert.FromJson(roleType.ObjectType.Tag, visited.v)?.ToString(),
                            };

                            this.predicates.Push(like);

                            break;

                        default:
                            throw new Exception("Unknown predicate kind " + visited.k);
                    }

                    break;
            }
        }

        public void VisitPull(Allors.Protocol.Json.Data.Pull visited)
        {
            var pull = new Pull
            {
                ExtentRef = visited.er,
                ObjectType = !string.IsNullOrWhiteSpace(visited.t) ? (IObjectType)this.transaction.Database.MetaPopulation.FindByTag(visited.t) : null,
                Object = visited.o != null ? this.transaction.Instantiate(visited.o.Value) : null,
                Arguments = visited.a != null ? new Arguments(visited.a, this.unitConvert) : null,
            };

            if (visited.e != null)
            {
                visited.e.Accept(this);
                pull.Extent = this.extents.Pop();
            }

            if (visited.r?.Length > 0)
            {
                var length = visited.r.Length;

                pull.Results = new Result[length];
                for (var i = 0; i < length; i++)
                {
                    var result = visited.r[i];
                    result.Accept(this);
                    pull.Results[i] = this.results.Pop();
                }
            }

            this.Pull = pull;
        }

        public void VisitResult(Allors.Protocol.Json.Data.Result visited)
        {
            var result = new Result
            {
                SelectRef = visited.r,
                Name = visited.n,
                Skip = visited.k,
                Take = visited.t,
            };

            if (visited.s != null)
            {
                visited.s.Accept(this);
                result.Select = this.selects.Pop();
            }

            if (visited.i?.Length > 0)
            {
                result.Include = new Node[visited.i.Length];
                for (var i = 0; i < visited.i.Length; i++)
                {
                    visited.i[i].Accept(this);
                    result.Include[i] = this.nodes.Pop();
                }
            }
            this.results.Push(result);
        }

        public void VisitSort(Allors.Protocol.Json.Data.Sort visited)
        {
            var sort = new Sort
            {
                SortDirection = visited.d,
                RoleType = !string.IsNullOrWhiteSpace(visited.r) ? (IRoleType)this.transaction.Database.ObjectFactory.MetaPopulation.FindByTag(visited.r) : null,
            };

            this.sorts.Push(sort);
        }

        public void VisitProcedure(Allors.Protocol.Json.Data.Procedure procedure) =>
            this.Procedure = new Procedure(procedure.n)
            {
                Collections = procedure.c?.ToDictionary(kvp => kvp.Key, kvp => this.transaction.Instantiate(kvp.Value)),
                Objects = procedure.o?.ToDictionary(kvp => kvp.Key, kvp => this.transaction.Instantiate(kvp.Value)),
                Values = procedure.v,
                Pool = procedure.p?.ToDictionary(v => this.transaction.Instantiate(v[0]), v => v[1])
            };
    }
}
