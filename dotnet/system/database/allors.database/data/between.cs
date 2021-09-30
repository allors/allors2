// <copyright file="Between.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using Meta;

    public class Between : IRolePredicate
    {
        public string[] Dependencies { get; set; }

        public Between(IRoleType roleType = null) => this.RoleType = roleType;

        public IRoleType RoleType { get; set; }

        public IEnumerable<object> Values { get; set; }

        public IEnumerable<IRoleType> Paths { get; set; }

        public string Parameter { get; set; }

        bool IPredicate.ShouldTreeShake(IArguments arguments) => this.HasMissingDependencies(arguments) || ((IPredicate)this).HasMissingArguments(arguments);

        bool IPredicate.HasMissingArguments(IArguments arguments) => this.Parameter != null && (arguments == null || !arguments.HasArgument(this.Parameter));

        void IPredicate.Build(ITransaction transaction, IArguments arguments, Database.ICompositePredicate compositePredicate)
        {
            if (this.Paths?.Count() == 2)
            {
                var paths = this.Paths.ToArray();
                compositePredicate.AddBetween(this.RoleType, paths[0], paths[1]);
            }
            else
            {
                var values = this.Parameter != null ? arguments.ResolveUnits(this.RoleType.ObjectType.Tag, this.Parameter) : this.Values?.ToArray();
                compositePredicate.AddBetween(this.RoleType, values[0], values[1]);
            }
        }

        public void Accept(IVisitor visitor) => visitor.VisitBetween(this);
    }
}
