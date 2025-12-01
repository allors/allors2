namespace Allors.Development.Repository.Tagged
{
    using System.Collections.Generic;
    using System.Linq;
    using Meta;


    public sealed class TaggedClass(TaggedMetaPopulation taggedMetaPopulation, IClass @class)
        : TaggedComposite(taggedMetaPopulation)
    {
        public IClass Class { get; } = @class;
        protected override IMetaObject MetaObject => this.Class;
        protected override IObjectType ObjectType => this.Class;
        protected override IComposite Composite => this.Class;
    }
}
