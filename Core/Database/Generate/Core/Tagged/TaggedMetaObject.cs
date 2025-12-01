namespace Allors.Development.Repository.Tagged
{
    using System;
    using Meta;

    public abstract class TaggedMetaObject
    {
        protected TaggedMetaObject(TaggedMetaPopulation taggedMetaPopulation) => this.TaggedMetaPopulation = taggedMetaPopulation;

        public TaggedMetaPopulation TaggedMetaPopulation { get; }

        protected abstract IMetaObject MetaObject { get; }

        public Guid Id => this.MetaObject.Id;

        public string IdAsString => this.MetaObject.IdAsString;

        public override string ToString() => this.MetaObject.ToString();

    }
}
