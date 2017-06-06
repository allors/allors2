namespace Allors.Server
{
    using System;
    using System.Linq;

    using Allors.Domain.Query;
    using Allors.Meta;

    using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;

    public class QueryRequestPredicate
    {
        /// <summary>
        /// Predicate Type
        /// </summary>
        public string _T { get; set; }

        public QueryRequestPredicate P { get; set; }

        public QueryRequestPredicate[] PS { get; set; }

        public string RT { get; set; }

        public object V { get; set; }

        public Predicate ToPredicate(MetaPopulation metaPopulation)
        {
            switch (this._T)
            {
                case "And": return new And { Predicates = this.PS.Select(v => v.ToPredicate(metaPopulation)).ToList() };

                case "Equals": return this.Equals(metaPopulation);

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
    }
}