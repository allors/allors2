namespace Allors.Development.Repository.Tagged
{
    using System;
    
    using Meta;

    public abstract class TaggedObjectType : TaggedMetaObject
    {
        protected TaggedObjectType(TaggedMetaPopulation taggedMetaPopulation) : base(taggedMetaPopulation)
        {
        }

        protected abstract IObjectType ObjectType { get; }

        // IMetaObject
        public Guid Id => this.ObjectType.Id;

        // IObjectType
        public bool IsUnit => this.ObjectType.IsUnit;

        public bool IsComposite => this.ObjectType.IsComposite;

        public bool IsInterface => this.ObjectType.IsInterface;

        public bool IsClass => this.ObjectType.IsClass;

        public string SingularName => this.ObjectType.SingularName;

        public string PluralName => this.ObjectType.PluralName;

        public string Name => this.ObjectType.Name;

        public Type ClrType => this.ObjectType.ClrType;
    }
}
