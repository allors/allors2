namespace Allors.Development.Repository.Tagged
{
    using Meta;


    public class TaggedInterface : TaggedComposite
    {
        public TaggedInterface(TaggedMetaPopulation taggedMetaPopulation, IInterface @interface) : base(taggedMetaPopulation) => this.Interface = @interface;

        public IInterface Interface { get; }
        protected override IMetaObject MetaObject => this.Interface;
        protected override IObjectType ObjectType => this.Interface;
        protected override IComposite Composite => this.Interface;
    }
}
