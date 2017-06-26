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
            var roleType = this.RT != null ? (RoleType)metaPopulation.Find(new Guid(this.RT)) : null;
            var first = this.GetUnitRole(roleType, this.F);
            var second = this.GetUnitRole(roleType, this.S);

            var predicate = new Between
                                {
                                    RoleType = roleType,
                                    First = first,
                                    Second = second
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
            var roleType = this.GetRoleType(metaPopulation);
            var value = this.GetUnitRole(roleType, this.V);

            var predicate = new Equals
                                {
                                    AssociationType = this.AT != null ? (AssociationType)metaPopulation.Find(new Guid(this.AT)) : null,
                                    RoleType = roleType,
                                    Value = value
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
            var roleType = this.RT != null ? (RoleType)metaPopulation.Find(new Guid(this.RT)) : null;
            var value = this.GetUnitRole(roleType, this.V);

            var predicate = new GreaterThan
                                {
                                    RoleType = roleType,
                                    Value = value
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
            var roleType = this.RT != null ? (RoleType)metaPopulation.Find(new Guid(this.RT)) : null;
            var value = this.GetUnitRole(roleType, this.V);

            var predicate = new LessThan
            {
                                    RoleType = roleType,
                                    Value = value,
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

        private RoleType GetRoleType(MetaPopulation metaPopulation)
        {
            return this.RT != null ? (RoleType)metaPopulation.Find(new Guid(this.RT)) : null;
        }

        private object GetUnitRole(RoleType roleType, object value)
        {
            if (value != null)
            {
                var unit = roleType?.ObjectType as Unit;
                if (unit != null)
                {
                    var stringValue = value as string;
                    if (stringValue != null)
                    {
                        switch (unit.UnitTag)
                        {
                            case UnitTags.Binary: return Convert.FromBase64String(stringValue);
                            case UnitTags.Boolean: return bool.Parse(stringValue);
                            case UnitTags.DateTime: return Convert.ToDateTime(stringValue);
                            case UnitTags.Decimal: return Convert.ToDecimal(stringValue);
                            case UnitTags.Float: return Convert.ToDouble(stringValue);
                            case UnitTags.Integer: return Convert.ToInt32(stringValue);
                            case UnitTags.Unique: return Guid.Parse(stringValue);
                        }
                    }
                    else
                    {
                        switch (unit.UnitTag)
                        {
                            case UnitTags.DateTime: return Convert.ToDateTime(value);
                            case UnitTags.Decimal: return Convert.ToDecimal(value);
                            case UnitTags.Float: return Convert.ToDouble(value);
                            case UnitTags.Integer: return Convert.ToInt32(value);
                        }
                    }
                }
            }

            return value;
        }
    }
}