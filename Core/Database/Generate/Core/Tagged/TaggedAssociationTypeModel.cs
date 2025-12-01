namespace Allors.Development.Repository.Tagged
{
    using Meta;


    public class TaggedAssociationType(TaggedMetaPopulation taggedMetaPopulation, IAssociationType associationType)
        : TaggedPropertyType(taggedMetaPopulation)
    {
        public IAssociationType AssociationType { get; } = associationType;
        protected override IMetaObject MetaObject => this.AssociationType;
        protected override IOperandType OperandType => this.AssociationType;
        protected override IPropertyType PropertyType => this.AssociationType;

        // IAssociationType
        private string SingularPropertyName => this.AssociationType.SingularPropertyName;

        private string SingularFullName => this.AssociationType.SingularFullName;

        private string PluralPropertyName => this.AssociationType.PluralPropertyName;

        private string PluralFullName => this.AssociationType.PluralFullName;

        public TaggedRelationType TaggedRelationType => this.TaggedMetaPopulation.Map(this.AssociationType.RelationType);

        public TaggedRoleType TaggedRoleType => this.TaggedMetaPopulation.Map(this.AssociationType.RoleType);

        new TaggedComposite ObjectType => this.TaggedMetaPopulation.Map(this.AssociationType.ObjectType);
    }
}
