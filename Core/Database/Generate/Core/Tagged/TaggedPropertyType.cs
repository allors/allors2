namespace Allors.Development.Repository.Tagged
{
    using Meta;

    public abstract class TaggedPropertyType : TaggedOperandType
    {
        protected TaggedPropertyType(TaggedMetaPopulation taggedMetaPopulation) : base(taggedMetaPopulation)
        {
        }

        protected abstract IPropertyType PropertyType { get; }

        // IPropertyType
        public TaggedObjectType ObjectType => this.TaggedMetaPopulation.Map(this.PropertyType.ObjectType);

        public string Name => this.PropertyType.Name;

        public string SingularName => this.PropertyType.SingularName;


        public string PluralName => this.PropertyType.PluralName;

        public bool IsOne => this.PropertyType.IsOne;

        public bool IsMany => this.PropertyType.IsMany;
    }
}
