namespace Allors.Server
{
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

        public Query ToQuery(MetaPopulation metaPopulation)
        {
            var type = metaPopulation.FindByName(this.T);
            var predicate = this.P?.ToPredicate(metaPopulation); 

            var query = new Query
                            {
                                Name = this.N,
                                Type = type,
                                Predicate = predicate
                            };

            return query;
        }
    }
}