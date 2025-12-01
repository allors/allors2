namespace Allors.Development.Repository.Tagged
{
    using System.Collections.Generic;
    
    using Meta;

    public abstract class TaggedOperandType : TaggedMetaObject
    {
        protected TaggedOperandType(TaggedMetaPopulation taggedMetaPopulation) : base(taggedMetaPopulation) { }

        protected abstract IOperandType OperandType { get; }
    }
}
