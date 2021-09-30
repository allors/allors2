// <copyright file="ExtentStatementChild.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql
{
    using Meta;

    internal class ExtentStatementChild : ExtentStatement
    {
        private readonly ExtentStatementRoot root;

        internal ExtentStatementChild(ExtentStatementRoot root, SqlExtent extent, IRoleType roleType)
            : base(extent)
        {
            this.root = root;
            this.RoleType = roleType;
        }

        internal ExtentStatementChild(ExtentStatementRoot root, SqlExtent extent, IAssociationType associationType)
            : base(extent)
        {
            this.root = root;
            this.AssociationType = associationType;
        }

        internal IAssociationType AssociationType { get; }

        internal override bool IsRoot => false;

        internal IRoleType RoleType { get; }

        public override string ToString() => this.root.ToString();

        internal override string AddParameter(object obj) => this.root.AddParameter(obj);

        internal override void Append(string part) => this.root.Append(part);

        internal override string CreateAlias() => this.root.CreateAlias();

        internal override ExtentStatement CreateChild(SqlExtent extent, IAssociationType association) => new ExtentStatementChild(this.root, extent, association);

        internal override ExtentStatement CreateChild(SqlExtent extent, IRoleType role) => new ExtentStatementChild(this.root, extent, role);
    }
}
