// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TreeNode.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
//
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
//
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
//
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Data
{
    using System.Collections.Generic;

    using Allors.Meta;

    public class TreeNode
    {
        public TreeNode(IRoleType roleType, IComposite composite = null, TreeNodes nodes = null)
        {
            this.RoleType = roleType;
            this.Composite = composite ?? (roleType.ObjectType.IsComposite ? (IComposite)roleType.ObjectType : null);

            if (this.Composite != null)
            {
                this.Nodes = nodes ?? new TreeNodes(this.Composite);
            }
        }

        public IRoleType RoleType { get; }

        public IComposite Composite { get; }

        public TreeNodes Nodes { get; }
    }
}
