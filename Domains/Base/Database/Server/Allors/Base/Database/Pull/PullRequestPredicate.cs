namespace Allors.Server
{
    using System;
    using System.Linq;

    using Allors.Domain.Query;
    using Allors.Meta;

    public class PullRequestPredicate
    {
        /// <summary>
        /// Predicate Type
        /// </summary>
        public string _T { get; set; }

        public PullRequestPredicate P { get; set; }

        public PullRequestPredicate[] PS { get; set; }

        public string RT { get; set; }

        public object V { get; set; }

        public Predicate Parse(MetaPopulation metaPopulation)
        {
            switch (this._T)
            {
                case "And": return new And { Predicates = this.PS.Select(v => v.Parse(metaPopulation)).ToList() };

                case "Equals": return this.Equals(metaPopulation);

                case "Like": return this.Like(metaPopulation);

                default:
                    throw new NotSupportedException($"Predicate {this._T} is not supported");
            }
        }

        private Predicate Equals(MetaPopulation metaPopulation)
        {
            var predicate = new Equals
                                {
                                    RoleType = (RoleType)metaPopulation.Find(new Guid(this.RT)),
                                    Value = this.V
                                };

            return predicate;
        }

        private Predicate Like(MetaPopulation metaPopulation)
        {
            var predicate = new Like
                                {
                                    RoleType = (RoleType)metaPopulation.Find(new Guid(this.RT)),
                                    Value = (string)this.V
                                };

            return predicate;
        }
    }
}