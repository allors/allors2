// <copyright file="Predicate.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Data
{
    using System;

    public class Predicate
    {
        public PredicateKind Kind { get; set; }

        public Guid? PropertyType { get; set; }

        public Guid? RoleType { get; set; }

        public Guid? ObjectType { get; set; }

        public string Parameter { get; set; }

        public Predicate Operand { get; set; }

        public Predicate[] Operands { get; set; }

        public string Object { get; set; }

        public string[] Objects { get; set; }

        public string Value { get; set; }

        public string[] Values { get; set; }

        public Extent Extent { get; set; }
    }
}
