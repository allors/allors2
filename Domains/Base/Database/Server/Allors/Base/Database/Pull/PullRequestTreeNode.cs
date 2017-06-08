namespace Allors.Server
{
    using System;

    using Allors.Meta;

    public class PullRequestTreeNode
    {
        /// <summary>
        /// The RoleType.
        /// </summary>
        public string RT { get; set; }

        /// <summary>
        /// The TreeNodes
        /// </summary>
        public PullRequestTreeNode[] N { get; set; }

        public void Parse(MetaPopulation metaPopulation, out TreeNode treeNode)
        {
            var roleType = (RoleType)metaPopulation.Find(new Guid(this.RT));
            treeNode = new TreeNode(roleType);

            if (this.N != null)
            {
                foreach (var n in this.N)
                {
                    n.Parse(metaPopulation, out TreeNode childTreeNode);
                    treeNode.Nodes.Add(childTreeNode);
                }
            }
        }
    }
}