namespace Allors.Development.Repository.Tagged
{
    using Meta;

    public class TaggedUnit : TaggedObjectType
    {
        public TaggedUnit(TaggedMetaPopulation taggedMetaPopulation, IUnit unit) : base(taggedMetaPopulation) => this.Unit = unit;

        public IUnit Unit { get; }

        protected override IMetaObject MetaObject => this.Unit;

        protected override IObjectType ObjectType => this.Unit;

        // IUnit
        public bool IsBinary => this.Unit.IsBinary;

        public bool IsBoolean => this.Unit.IsBoolean;

        public bool IsDateTime => this.Unit.IsDateTime;

        public bool IsDecimal => this.Unit.IsDecimal;

        public bool IsFloat => this.Unit.IsFloat;

        public bool IsInteger => this.Unit.IsInteger;

        public bool IsString => this.Unit.IsString;

        public bool IsUnique => this.Unit.IsUnique;
    }
}
