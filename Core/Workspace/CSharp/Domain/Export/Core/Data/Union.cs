// <copyright file="Union.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Protocol.Data;
    using Allors.Workspace.Meta;

    public class Union : IExtentOperator
    {
        public Union(params IExtent[] operands) => this.Operands = operands;

        public IComposite ObjectType => this.Operands?[0].ObjectType;

        public IExtent[] Operands { get; set; }

        public Sort[] Sorting { get; set; }

        bool IExtent.HasMissingArguments(IReadOnlyDictionary<string, object> arguments) => this.Operands.Any(v => v.HasMissingArguments(arguments));

        public Extent ToJson() =>
            new Extent
            {
                Kind = ExtentKind.Union,
                Operands = this.Operands.Select(v => v.ToJson()).ToArray(),
                Sorting = this.Sorting.Select(v => new Protocol.Data.Sort { Descending = v.Descending, RoleType = v.RoleType?.Id }).ToArray(),
            };
    }
}
