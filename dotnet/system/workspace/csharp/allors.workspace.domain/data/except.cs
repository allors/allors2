// <copyright file="Except.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Data
{
    using Meta;

    public class Except : IExtentOperator
    {
        public Except(params Extent[] operands) => this.Operands = operands;

        public IComposite ObjectType => this.Operands?[0].ObjectType;

        public Extent[] Operands { get; set; }

        public Sort[] Sorting { get; set; }

        public void Accept(IVisitor visitor) => visitor.VisitExcept(this);
    }
}
