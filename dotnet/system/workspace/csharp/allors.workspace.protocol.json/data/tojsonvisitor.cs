// <copyright file="ToJsonVisitor.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Protocol.Json
{
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Protocol.Json;
    using Allors.Protocol.Json.Data;
    using Data;
    using Meta;
    using Extent = Allors.Protocol.Json.Data.Extent;
    using IVisitor = Data.IVisitor;
    using Node = Allors.Protocol.Json.Data.Node;
    using Procedure = Allors.Protocol.Json.Data.Procedure;
    using Pull = Allors.Protocol.Json.Data.Pull;
    using Result = Allors.Protocol.Json.Data.Result;
    using Select = Allors.Protocol.Json.Data.Select;
    using Sort = Allors.Protocol.Json.Data.Sort;

    public class ToJsonVisitor : IVisitor
    {
        private readonly IUnitConvert unitConvert;

        private readonly Stack<Extent> extents;
        private readonly Stack<Predicate> predicates;
        private readonly Stack<Result> results;
        private readonly Stack<Select> selects;
        private readonly Stack<Node> nodes;
        private readonly Stack<Sort> sorts;

        public ToJsonVisitor(IUnitConvert unitConvert)
        {
            this.unitConvert = unitConvert;
            this.extents = new Stack<Extent>();
            this.predicates = new Stack<Predicate>();
            this.results = new Stack<Result>();
            this.selects = new Stack<Select>();
            this.nodes = new Stack<Node>();
            this.sorts = new Stack<Sort>();
        }

        public Pull Pull { get; private set; }

        public Extent Extent => this.extents?.Peek();

        public Select Select => this.selects?.Peek();

        public Procedure Procedure { get; private set; }

        public void VisitAnd(And visited)
        {
            var predicate = new Predicate
            {
                k = PredicateKind.And,
                d = visited.Dependencies,
            };

            this.predicates.Push(predicate);

            if (visited.Operands != null)
            {
                var length = visited.Operands.Length;
                predicate.ops = new Predicate[length];
                for (var i = 0; i < length; i++)
                {
                    var operand = visited.Operands[i];
                    operand.Accept(this);
                    predicate.ops[i] = this.predicates.Pop();
                }
            }
        }

        public void VisitBetween(Between visited)
        {
            var predicate = new Predicate
            {
                k = PredicateKind.Between,
                d = visited.Dependencies,
                r = visited.RoleType?.RelationType.Tag,
                vs = visited.Values?.Select(this.unitConvert.ToJson).ToArray(),
                pas = visited.Paths?.Select(v => v.RelationType.Tag).ToArray(),
                p = visited.Parameter,
            };

            this.predicates.Push(predicate);
        }

        public void VisitContainedIn(ContainedIn visited)
        {
            var predicate = new Predicate
            {
                k = PredicateKind.ContainedIn,
                d = visited.Dependencies,
                a = (visited.PropertyType as IAssociationType)?.RelationType.Tag,
                r = (visited.PropertyType as IRoleType)?.RelationType.Tag,
                vs = visited.Objects?.Select(v => v.Id as object).ToArray(),
                p = visited.Parameter,
            };

            this.predicates.Push(predicate);

            if (visited.Extent != null)
            {
                visited.Extent.Accept(this);
                predicate.e = this.extents.Pop();
            }
        }

        public void VisitContains(Contains visited)
        {
            var predicate = new Predicate
            {
                k = PredicateKind.Contains,
                d = visited.Dependencies,
                a = (visited.PropertyType as IAssociationType)?.RelationType.Tag,
                r = (visited.PropertyType as IRoleType)?.RelationType.Tag,
                ob = visited.Object?.Id,
                p = visited.Parameter,
            };

            this.predicates.Push(predicate);
        }

        public void VisitEquals(Equals visited)
        {
            var predicate = new Predicate
            {
                k = PredicateKind.Equals,
                d = visited.Dependencies,
                a = (visited.PropertyType as IAssociationType)?.RelationType.Tag,
                r = (visited.PropertyType as IRoleType)?.RelationType.Tag,
                ob = visited.Object?.Id,
                v = this.unitConvert.ToJson(visited.Value),
                pa = visited.Path?.RelationType.Tag,
                p = visited.Parameter,
            };

            this.predicates.Push(predicate);
        }

        public void VisitExcept(Except visited)
        {
            var extent = new Extent
            {
                k = ExtentKind.Except,
            };

            this.extents.Push(extent);

            if (visited.Operands != null)
            {
                var length = visited.Operands.Length;
                extent.o = new Extent[length];
                for (var i = 0; i < length; i++)
                {
                    var operand = visited.Operands[i];
                    operand.Accept(this);
                    extent.o[i] = this.extents.Pop();
                }
            }

            if (visited.Sorting?.Length > 0)
            {
                var length = visited.Sorting.Length;
                extent.s = new Sort[length];
                for (var i = 0; i < length; i++)
                {
                    var sorting = visited.Sorting[i];
                    sorting.Accept(this);
                    extent.s[i] = this.sorts.Pop();
                }
            }
        }

        public void VisitExists(Exists visited)
        {
            var predicate = new Predicate
            {
                k = PredicateKind.Exists,
                d = visited.Dependencies,
                a = (visited.PropertyType as IAssociationType)?.RelationType.Tag,
                r = (visited.PropertyType as IRoleType)?.RelationType.Tag,
                p = visited.Parameter,
            };

            this.predicates.Push(predicate);
        }

        public void VisitFilter(Data.Filter visited)
        {
            var extent = new Extent
            {
                k = ExtentKind.Filter,
                t = visited.ObjectType?.Tag,
                s = visited.Sorting?.Select(v => new Sort
                {
                    d = v.SortDirection,
                    r = v.RoleType?.RelationType.Tag
                }).ToArray(),
            };

            this.extents.Push(extent);

            if (visited.Predicate != null)
            {
                visited.Predicate.Accept(this);
                extent.p = this.predicates.Pop();
            }
        }

        public void VisitSelect(Data.Select visited)
        {
            var @select = new Select
            {
                a = (visited.PropertyType as IAssociationType)?.RelationType.Tag,
                r = (visited.PropertyType as IRoleType)?.RelationType.Tag,
            };

            this.selects.Push(@select);

            if (visited.Next != null)
            {
                visited.Next.Accept(this);
                select.n = this.selects.Pop();
            }

            if (visited.Include?.Count() > 0)
            {
                var includes = visited.Include.ToArray();
                var length = includes.Length;
                @select.i = new Node[length];
                for (var i = 0; i < length; i++)
                {
                    var node = includes[i];
                    node.Accept(this);
                    @select.i[i] = this.nodes.Pop();
                }
            }
        }

        public void VisitGreaterThan(GreaterThan visited)
        {
            var predicate = new Predicate
            {
                k = PredicateKind.GreaterThan,
                d = visited.Dependencies,
                r = visited.RoleType?.RelationType.Tag,
                v = this.unitConvert.ToJson(visited.Value),
                pa = visited.Path?.RelationType.Tag,
                p = visited.Parameter,
            };

            this.predicates.Push(predicate);
        }

        public void VisitInstanceOf(Instanceof visited)
        {
            var predicate = new Predicate
            {
                k = PredicateKind.InstanceOf,
                d = visited.Dependencies,
                o = visited.ObjectType?.Tag,
                a = (visited.PropertyType as IAssociationType)?.RelationType.Tag,
                r = (visited.PropertyType as IRoleType)?.RelationType.Tag,
            };

            this.predicates.Push(predicate);
        }

        public void VisitIntersect(Intersect visited)
        {
            var extent = new Extent
            {
                k = ExtentKind.Intersect,
            };

            this.extents.Push(extent);

            if (visited.Operands != null)
            {
                var length = visited.Operands.Length;
                extent.o = new Extent[length];
                for (var i = 0; i < length; i++)
                {
                    var operand = visited.Operands[i];
                    operand.Accept(this);
                    extent.o[i] = this.extents.Pop();
                }
            }

            if (visited.Sorting?.Length > 0)
            {
                var length = visited.Sorting.Length;
                extent.s = new Sort[length];
                for (var i = 0; i < length; i++)
                {
                    var sorting = visited.Sorting[i];
                    sorting.Accept(this);
                    extent.s[i] = this.sorts.Pop();
                }
            }
        }

        public void VisitLessThan(LessThan visited)
        {
            var predicate = new Predicate
            {
                k = PredicateKind.LessThan,
                d = visited.Dependencies,
                r = visited.RoleType?.RelationType.Tag,
                v = this.unitConvert.ToJson(visited.Value),
                pa = visited.Path?.RelationType.Tag,
                p = visited.Parameter,
            };

            this.predicates.Push(predicate);
        }

        public void VisitLike(Like visited)
        {
            var predicate = new Predicate
            {
                k = PredicateKind.Like,
                d = visited.Dependencies,
                r = visited.RoleType?.RelationType.Tag,
                v = this.unitConvert.ToJson(visited.Value),
                p = visited.Parameter,
            };

            this.predicates.Push(predicate);
        }

        public void VisitNode(Data.Node visited)
        {
            var node = new Node
            {
                a = (visited.PropertyType as IAssociationType)?.RelationType.Tag,
                r = (visited.PropertyType as IRoleType)?.RelationType.Tag,
            };

            this.nodes.Push(node);

            if (visited.Nodes?.Length > 0)
            {
                var length = visited.Nodes.Length;
                node.n = new Node[length];
                for (var i = 0; i < length; i++)
                {
                    visited.Nodes[i].Accept(this);
                    node.n[i] = this.nodes.Pop();
                }
            }
        }

        public void VisitNot(Not visited)
        {
            var predicate = new Predicate()
            {
                k = PredicateKind.Not,
                d = visited.Dependencies,
            };

            this.predicates.Push(predicate);

            if (visited.Operand != null)
            {
                visited.Operand.Accept(this);
                predicate.op = this.predicates.Pop();
            }
        }

        public void VisitOr(Or visited)
        {
            var predicate = new Predicate()
            {
                k = PredicateKind.Or,
                d = visited.Dependencies,
            };

            this.predicates.Push(predicate);

            if (visited.Operands != null)
            {
                var length = visited.Operands.Length;
                predicate.ops = new Predicate[length];
                for (var i = 0; i < length; i++)
                {
                    var operand = visited.Operands[i];
                    operand.Accept(this);
                    predicate.ops[i] = this.predicates.Pop();
                }
            }
        }

        public void VisitPull(Data.Pull visited)
        {
            var pull = new Pull
            {
                er = visited.ExtentRef,
                t = visited.ObjectType?.Tag,
                o = visited.ObjectId ?? visited.Object?.Id,
            };

            if (visited.Extent != null)
            {
                visited.Extent.Accept(this);
                pull.e = this.extents.Pop();
            }

            if (visited.Results?.Length > 0)
            {
                var length = visited.Results.Length;

                pull.r = new Result[length];
                for (var i = 0; i < length; i++)
                {
                    var result = visited.Results[i];
                    result.Accept(this);
                    pull.r[i] = this.results.Pop();
                }
            }

            this.Pull = pull;
        }

        public void VisitResult(Data.Result visited)
        {
            var result = new Result
            {
                r = visited.SelectRef,
                n = visited.Name,
                k = visited.Skip,
                t = visited.Take,
            };

            this.results.Push(result);

            if (visited.Select != null)
            {
                visited.Select.Accept(this);
                result.s = this.selects.Pop();
            }

            if (visited.Include?.Count() > 0)
            {
                var includes = visited.Include.ToArray();
                var length = includes.Length;
                result.i = new Node[length];
                for (var i = 0; i < length; i++)
                {
                    var node = includes[i];
                    node.Accept(this);
                    result.i[i] = this.nodes.Pop();
                }
            }
        }

        public void VisitSort(Data.Sort visited)
        {
            var sort = new Sort
            {
                d = visited.SortDirection,
                r = visited.RoleType?.RelationType.Tag,
            };

            this.sorts.Push(sort);
        }

        public void VisitUnion(Union visited)
        {
            var extent = new Extent
            {
                k = ExtentKind.Union,
            };

            this.extents.Push(extent);

            if (visited.Operands != null)
            {
                var length = visited.Operands.Length;
                extent.o = new Extent[length];
                for (var i = 0; i < length; i++)
                {
                    var operand = visited.Operands[i];
                    operand.Accept(this);
                    extent.o[i] = this.extents.Pop();
                }
            }

            if (visited.Sorting?.Length > 0)
            {
                var length = visited.Sorting.Length;
                extent.s = new Sort[length];
                for (var i = 0; i < length; i++)
                {
                    var sorting = visited.Sorting[i];
                    sorting.Accept(this);
                    extent.s[i] = this.sorts.Pop();
                }
            }
        }

        public void VisitProcedure(Data.Procedure procedure) => this.Procedure = new Procedure
        {
            n = procedure.Name,
            c = procedure.Collections?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Select(v => v.Id).ToArray()),
            o = procedure.Objects?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Id),
            v = procedure.Values,
            p = procedure.Pool?.Select(kvp => new long[] { kvp.Key.Id, kvp.Value }).ToArray(),
        };
    }
}
