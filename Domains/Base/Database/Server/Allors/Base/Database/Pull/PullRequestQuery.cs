namespace Allors.Server
{
    using System;
    using System.Linq;

    using Allors.Domain.Query;
    using Allors.Meta;

    public class PullRequestQuery
    {
        /// <summary>
        /// The name of the Query
        /// </summary>
        public string N { get; set; }

        /// <summary>
        /// The ObjectType.
        /// </summary>
        public string OT { get; set; }

        /// <summary>
        /// The predicate
        /// </summary>
        public PullRequestPredicate P { get; set; }

        public PullRequestTreeNode[] I { get; set; }

        public PullRequestSort[] S { get; set; }

        public PullRequestPage PA { get; set; }

        public Query Parse(MetaPopulation metaPopulation)
        {
            var composite = (Composite)metaPopulation.Find(new Guid(this.OT));
            var predicate = this.P?.Parse(metaPopulation);

            Tree include = null;
            if (this.I != null)
            {
                include = new Tree(composite);
                foreach (var i in this.I)
                {
                    i.Parse(metaPopulation, out TreeNode treeNode);
                    include.Nodes.Add(treeNode);
                }
            }

            var sort = this.S?.Select(v => v.Parse(metaPopulation)).ToArray();

            var page = this.PA?.Parse();

            var query = new Query
                            {
                                Name = this.N,
                                ObjectType = composite,
                                Predicate = predicate,
                                Include = include,
                                Sort = sort,
                                Page = page
                            };

            return query;
        }
    }
}