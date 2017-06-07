namespace Allors.Server
{
    using System;
    using System.Linq;

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
        public string OT { get; set; }

        /// <summary>
        /// The predicate
        /// </summary>
        public QueryRequestPredicate P { get; set; }

        public QueryRequestTreeNode[] F { get; set; }

        public QueryRequestSort[] S { get; set; }

        public QueryRequestPage PA { get; set; }

        public Query Parse(MetaPopulation metaPopulation)
        {
            var composite = (Composite)metaPopulation.Find(new Guid(this.OT));
            var predicate = this.P?.Parse(metaPopulation);
            var fetch = new Tree(composite);
            if (this.F != null)
            {
                foreach (var treeNode in this.F)
                {
                    treeNode.Parse(fetch);
                }
            }

            var sort = this.S?.Select(v => v.Parse(metaPopulation)).ToArray();

            var page = this.PA?.Parse();

            var query = new Query
                            {
                                Name = this.N,
                                ObjectType = composite,
                                Predicate = predicate,
                                Fetch = fetch,
                                Sort = sort,
                                Page = page
                            };

            return query;
        }
    }
}