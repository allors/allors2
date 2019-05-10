// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Tree.cs" company="Allors bvba">
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

using Allors.Workspace.Meta;

namespace Allors.Workspace.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Tree
    {
        public Tree(IComposite composite)
        {
            this.Composite = composite;
            this.Nodes = new TreeNodes(this.Composite);
        }

        public IComposite Composite { get; }

        public TreeNodes Nodes { get; }
        
        public Tree Add(IEnumerable<IRelationType> relationTypes)
        {
            return this.Add(relationTypes.Select(v => v.RoleType));
        }

        public Tree Add(IEnumerable<IRoleType> roleTypes)
        {
            new List<IRoleType>(roleTypes).ForEach(v => this.Add(v));
            return this;
        }

        public Tree Add(IRelationType relationType)
        {
            return this.Add(relationType.RoleType);
        }

        public Tree Add(IRoleType roleType)
        {
            var treeNode = new TreeNode(roleType);
            this.Nodes.Add(treeNode);
            return this;
        }

        public Tree Add(IConcreteRoleType concreteRoleType)
        {
            return this.Add(concreteRoleType.RoleType);
        }

        public Tree Add(IRelationType relationType, Tree tree)
        {
            return this.Add(relationType.RoleType, tree);
        }

        public Tree Add(IRoleType roleType, Tree tree)
        {
            var treeNode = new TreeNode(roleType, tree.Composite, tree.Nodes);
            this.Nodes.Add(treeNode);
            return this;
        }

        public Tree Add(IConcreteRoleType concreteRoleType, Tree tree)
        {
            var treeNode = new TreeNode(concreteRoleType.RoleType, tree.Composite, tree.Nodes);
            this.Nodes.Add(treeNode);
            return this;
        }

        public Protocol.Data.Tree ToJson()
        {
            return new Protocol.Data.Tree
            {
                Composite = this.Composite.Id,
                Nodes = this.Nodes.Select(v => v.ToJson()).ToArray()
            };
        }

        public string DebugView
        {
            get
            {
                var toString = new StringBuilder();
                toString.Append(this.Composite.Name + "\n");
                this.DebugNodeView(toString, this.Nodes, 1);
                return toString.ToString();
            }
        }

        private void DebugNodeView(StringBuilder toString, TreeNodes nodes, int level)
        {
            foreach (var node in nodes)
            {
                var indent = new string(' ', level * 2);
                toString.Append(indent + "- " + node.RoleType + "\n");
                this.DebugNodeView(toString, node.Nodes, level + 1);
            }
        }
    }
}