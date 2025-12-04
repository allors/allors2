namespace Allors.Development.Repository.Tagged
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Meta;


    public class TaggedRelationType : TaggedMetaObject
    {
        public TaggedRelationType(TaggedMetaPopulation taggedMetaPopulation, IRelationType relationType) : base(taggedMetaPopulation) => this.RelationType = relationType;

        public IRelationType RelationType { get; }
        protected override IMetaObject MetaObject => this.RelationType;

        // IMetaObject
        public Guid Id => this.RelationType.Id;


        // IRelationType
        public TaggedAssociationType TaggedAssociationType => this.TaggedMetaPopulation.Map(this.RelationType.AssociationType);

        public TaggedRoleType TaggedRoleType => this.TaggedMetaPopulation.Map(this.RelationType.RoleType);

        public Multiplicity Multiplicity => this.RelationType.Multiplicity;

        public bool IsOneToOne => this.TaggedAssociationType.IsOne && this.TaggedRoleType.IsOne;

        public bool IsOneToMany => this.TaggedAssociationType.IsOne && this.TaggedRoleType.IsMany;

        public bool IsManyToOne => this.TaggedAssociationType.IsMany && this.TaggedRoleType.IsOne;

        public bool IsManyToMany => this.TaggedAssociationType.IsMany && this.TaggedRoleType.IsMany;

        public bool IsIndexed => this.RelationType.IsIndexed;

        public bool IsDerived => this.RelationType.IsDerived;

        public string Name => ((RelationType)this.RelationType).Name;

        public bool IsSynced => this.RelationType.IsSynced;

        public string[] SortedTags => this.RelationType.Tags?.OrderBy(v => v).ToArray() ?? Array.Empty<string>();
    }
}
