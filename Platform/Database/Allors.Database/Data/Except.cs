// <copyright file="Except.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using Allors.Meta;
    using Allors.Protocol.Data;

    public class Except : IExtentOperator
    {
        public Except(params IExtent[] operands) => this.Operands = operands;

        public IComposite ObjectType => this.Operands?[0].ObjectType;

        public IExtent[] Operands { get; set; }

        public Sort[] Sorting { get; set; }

        public Protocol.Data.Extent Save() =>
            new Protocol.Data.Extent
            {
                kind = ExtentKind.Except,
                operands = this.Operands.Select(v => v.Save()).ToArray(),
                sorting = this.Sorting.Select(v => new Protocol.Data.Sort { @descending = v.Descending, roleType = v.RoleType?.Id }).ToArray(),
            };

        bool IExtent.HasMissingArguments(IDictionary<string, string> parameters) => this.Operands.Any(v => v.HasMissingArguments(parameters));

        Allors.Extent IExtent.Build(ISession session, IDictionary<string, string> parameters)
        {
            var extent = session.Except(this.Operands[0].Build(session, parameters), this.Operands[1].Build(session, parameters));
            foreach (var sort in this.Sorting)
            {
                sort.Build(extent);
            }

            return extent;
        }
    }
}
