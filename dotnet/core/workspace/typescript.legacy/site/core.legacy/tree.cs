namespace Allors.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Meta;

    public class Tree
    {
        public Tree(IComposite composite)
        {
            this.Composite = composite;
            this.Nodes = new TreeNodes(this.Composite);
        }

        public IComposite Composite { get; }

        public TreeNodes Nodes { get; }
        
        public Tree Add(IEnumerable<IRelationType> relationTypes) => this.Add(relationTypes.Select(v => v.RoleType));

        public Tree Add(IEnumerable<IRoleType> roleTypes)
        {
            new List<IRoleType>(roleTypes).ForEach(v => this.Add(v));
            return this;
        }

        public Tree Add(IRelationType relationType) => this.Add(relationType.RoleType);

        public Tree Add(IRoleType roleType)
        {
            var treeNode = new TreeNode(roleType);
            this.Nodes.Add(treeNode);
            return this;
        }

        public Tree Add(IConcreteRoleType concreteRoleType) => this.Add(concreteRoleType.RoleType);

        public Tree Add(IRelationType relationType, Tree tree) => this.Add(relationType.RoleType, tree);

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
    }
}
