namespace Allors.Server
{
    using System;

    using Allors.Domain.Query;
    using Allors.Meta;

    public class QueryRequestQuery
    {
        /// <summary>
        /// The name of the Query
        /// </summary>
        public string N { get; set; }

        /// <summary>
        /// The ObjectType.
        /// </summary>
        public string T { get; set; }

        /// <summary>
        /// The predicate
        /// </summary>
        public QueryRequestPredicate P { get; set; }

        public QueryRequestTreeNode[] TN { get; set; }

        public Query Parse(MetaPopulation metaPopulation)
        {
            var composite = (Composite)metaPopulation.Find(new Guid(this.T));
            var predicate = this.P?.ToPredicate(metaPopulation);
            var tree = new Tree(composite);
            if (this.TN != null)
            {
                foreach (var treeNode in this.TN)
                {
                    treeNode.Parse(tree);
                }
            }

            var query = new Query
                            {
                                Name = this.N,
                                Type = composite,
                                Predicate = predicate,
                                Tree = tree
                            };

            return query;
        }
    }
}