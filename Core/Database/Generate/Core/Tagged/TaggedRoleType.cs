namespace Allors.Development.Repository.Tagged
{
    using Meta;


    public class TaggedRoleType : TaggedPropertyType
    {
        public TaggedRoleType(TaggedMetaPopulation taggedMetaPopulation, IRoleType roleType) : base(taggedMetaPopulation) => this.RoleType = roleType;

        public IRoleType RoleType { get; }
        protected override IMetaObject MetaObject => this.RoleType;
        protected override IOperandType OperandType => this.RoleType;
        protected override IPropertyType PropertyType => this.RoleType;

        // IRoleType
        public string SingularPropertyName => this.RoleType.SingularPropertyName;

        public string SingularFullName => this.RoleType.SingularFullName;

        public string PluralPropertyName => this.RoleType.PluralPropertyName;

        public string PluralFullName => this.RoleType.PluralFullName;

        public TaggedAssociationType AsociationType => this.TaggedMetaPopulation.Map(this.RoleType.AssociationType);

        public TaggedRelationType RelationType => this.TaggedMetaPopulation.Map(this.RoleType.RelationType);

        public int? Size => this.RoleType.Size;

        public int? Precision => this.RoleType.Precision;

        public int? Scale => this.RoleType.Scale;
    }
}
