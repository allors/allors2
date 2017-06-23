namespace Allors.Server
{
    using System;
    using System.Linq;

    using Allors.Domain.Query;
    using Allors.Meta;

    public class PullRequestPredicate
    {
        public string _T { get; set; }

        public PullRequestPredicate P { get; set; }

        public PullRequestPredicate[] PS { get; set; }

        public PullRequestQuery Q { get; set; }

        public string OT { get; set; }

        public string AT { get; set; }

        public string RT { get; set; }

        public object V { get; set; }

        public object F { get; set; }

        public object S { get; set; }

        public string O { get; set; }

        public string[] OS { get; set; }

        public Predicate Parse(MetaPopulation metaPopulation)
        {
            switch (this._T)
            {
                case "And": return new And { Predicates = this.PS.Select(v => v.Parse(metaPopulation)).ToList() };

                case "Between": return this.Between(metaPopulation);

                case "ContainedIn": return this.ContainedIn(metaPopulation);

                case "Contains": return this.Contains(metaPopulation);

                case "Equals": return this.Equals(metaPopulation);

                case "Exists": return this.Exists(metaPopulation);

                case "GreaterThan": return this.GreaterThan(metaPopulation);

                case "Instanceof": return this.Instanceof(metaPopulation);

                case "LessThan": return this.LessThan(metaPopulation);

                case "Like": return this.Like(metaPopulation);

                case "Not": return new Not { Predicate = this.P.Parse(metaPopulation) };

                case "Or": return new Or { Predicates = this.PS.Select(v => v.Parse(metaPopulation)).ToList() };

                default:
                    throw new NotSupportedException($"Predicate {this._T} is not supported");
            }
        }

        private Predicate Between(MetaPopulation metaPopulation)
        {
            var predicate = new Between
                                {
                                    AssociationType = this.AT != null ? (AssociationType)metaPopulation.Find(new Guid(this.AT)) : null,
                                    RoleType = this.RT != null ? (RoleType)metaPopulation.Find(new Guid(this.RT)) : null,
                                    First = this.F,
                                    Second = this.S
                                };

            return predicate;
        }

        private Predicate ContainedIn(MetaPopulation metaPopulation)
        {
            var predicate = new ContainedIn
                                {
                                    AssociationType = this.AT != null ? (AssociationType)metaPopulation.Find(new Guid(this.AT)) : null,
                                    RoleType = this.RT != null ? (RoleType)metaPopulation.Find(new Guid(this.RT)) : null,
                                    Query = this.Q.Parse(metaPopulation),
                                    ObjectIds = this.OS?.Select(long.Parse).ToArray()
                                };

            return predicate;
        }

        private Predicate Contains(MetaPopulation metaPopulation)
        {
            var predicate = new Contains
                                {
                                    AssociationType = this.AT != null ? (AssociationType)metaPopulation.Find(new Guid(this.AT)) : null,
                                    RoleType = this.RT != null ? (RoleType)metaPopulation.Find(new Guid(this.RT)) : null,
                                    ObjectId = long.Parse(this.O)
                                };

            return predicate;
        }

        private Predicate Equals(MetaPopulation metaPopulation)
        {
            var predicate = new Equals
                                {
                                    AssociationType = this.AT != null ? (AssociationType)metaPopulation.Find(new Guid(this.AT)) : null,
                                    RoleType = this.RT != null ? (RoleType)metaPopulation.Find(new Guid(this.RT)) : null,
                                    Value = this.V
                                };

            return predicate;
        }

        private Predicate Exists(MetaPopulation metaPopulation)
        {
            var predicate = new Exists
                                {
                                    AssociationType = this.AT != null ? (AssociationType)metaPopulation.Find(new Guid(this.AT)) : null,
                                    RoleType = this.RT != null ? (RoleType)metaPopulation.Find(new Guid(this.RT)) : null,
                                };

            return predicate;
        }

        private Predicate GreaterThan(MetaPopulation metaPopulation)
        {
            var predicate = new GreaterThan
                                {
                                    RoleType = this.RT != null ? (RoleType)metaPopulation.Find(new Guid(this.RT)) : null,
                                    Value = this.V,
                               };

            return predicate;
        }

        private Predicate Instanceof(MetaPopulation metaPopulation)
        {
            var predicate = new Instanceof
                                {
                                    AssociationType = this.AT != null ? (AssociationType)metaPopulation.Find(new Guid(this.AT)) : null,
                                    RoleType = this.RT != null ? (RoleType)metaPopulation.Find(new Guid(this.RT)) : null,
                                    ObjectType = (IComposite)metaPopulation.Find(new Guid(this.OT)),
                                };

            return predicate;
        }

        private Predicate LessThan(MetaPopulation metaPopulation)
        {
            var predicate = new LessThan
            {
                                    RoleType = this.RT != null ? (RoleType)metaPopulation.Find(new Guid(this.RT)) : null,
                                    Value = this.V,
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