//------------------------------------------------------------------------------------------------- 
// <copyright file="AllorsExtentStatementChildSql.cs" company="Allors bvba">
// Copyright 2002-2009 Allors bvba.
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
// <summary>Defines the AllorsExtentStatementSql type.</summary>
//-------------------------------------------------------------------------------------------------

using Allors.Meta;

namespace Allors.Adapters.Relation.SqlClient
{
    internal class AllorsExtentStatementChildSql : AllorsExtentStatementSql
    {
        private readonly IAssociationType association;
        private readonly IRoleType role;
        private readonly AllorsExtentStatementRootSql root;

        internal AllorsExtentStatementChildSql(AllorsExtentStatementRootSql root, AllorsExtentSql extent, IRoleType role)
            : base(extent)
        {
            this.root = root;
            this.role = role;
        }

        internal AllorsExtentStatementChildSql(AllorsExtentStatementRootSql root, AllorsExtentSql extent, IAssociationType association)
            : base(extent)
        {
            this.root = root;
            this.association = association;
        }

        public IAssociationType Association
        {
            get { return this.association; }
        }

        internal override bool IsRoot
        {
            get { return false; }
        }

        public IRoleType Role
        {
            get { return role; }
        }

        public override string ToString()
        {
            return this.root.ToString();
        }

        internal override string AddParameter(object obj)
        {
            return this.root.AddParameter(obj);
        }

        internal override void Append(string part)
        {
            this.root.Append(part);
        }

        internal override string CreateAlias()
        {
            return this.root.CreateAlias();
        }

        internal override AllorsExtentStatementSql CreateChild(AllorsExtentSql extent, IAssociationType associationType)
        {
            return new AllorsExtentStatementChildSql(this.root, extent, associationType);
        }

        internal override AllorsExtentStatementSql CreateChild(AllorsExtentSql extent, IRoleType roleType)
        {
            return new AllorsExtentStatementChildSql(this.root, extent, roleType);
        }
    }
}