// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtentStatementChild.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Adapters.Object.Npgsql
{
    using Allors.Meta;

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
