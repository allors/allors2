//-------------------------------------------------------------------------------------------------
// <copyright file="Like.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
//-------------------------------------------------------------------------------------------------

namespace Allors.Data
{
    using System.Collections.Generic;

    using Allors.Meta;
    using Allors.Protocol.Data;

    public class Like : IRolePredicate
    {
        public Like(IRoleType roleType = null) => this.RoleType = roleType;

        public IRoleType RoleType { get; set; }

        public string Value { get; set; }

        public string Parameter { get; set; }

        public Predicate Save() =>
            new Predicate
            {
                Kind = PredicateKind.Like,
                RoleType = this.RoleType?.Id,
                Value = DataConvert.ToString(this.Value),
                Parameter = this.Parameter,
            };

        bool IPredicate.ShouldTreeShake(IReadOnlyDictionary<string, object> arguments) => ((IPredicate)this).HasMissingArguments(arguments);

        bool IPredicate.HasMissingArguments(IReadOnlyDictionary<string, object> arguments) => this.Parameter != null && (arguments == null || !arguments.ContainsKey(this.Parameter));

        void IPredicate.Build(ISession session, IReadOnlyDictionary<string, object> arguments, Allors.ICompositePredicate compositePredicate)
        {
            var value = this.Parameter != null ? (string)arguments[this.Parameter] : this.Value;

            compositePredicate.AddLike(this.RoleType, value);
        }
    }
}
