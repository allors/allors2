//-------------------------------------------------------------------------------------------------
// <copyright file="Except.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
//-------------------------------------------------------------------------------------------------

namespace Allors.Workspace.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Protocol.Data;
    using Allors.Workspace.Meta;

    public class Except : IExtentOperator
    {
        public Except(params IExtent[] operands) => this.Operands = operands;

        public IComposite ObjectType => this.Operands?[0].ObjectType;

        public IExtent[] Operands { get; set; }

        public Sort[] Sorting { get; set; }

        public Extent ToJson() =>
            new Extent
            {
                Kind = ExtentKind.Except,
                Operands = this.Operands.Select(v => v.ToJson()).ToArray(),
                Sorting = this.Sorting.Select(v => new Protocol.Data.Sort { Descending = v.Descending, RoleType = v.RoleType.Id }).ToArray(),
            };

        bool IExtent.HasMissingArguments(IReadOnlyDictionary<string, object> arguments) => this.Operands.Any(v => v.HasMissingArguments(arguments));
    }
}
