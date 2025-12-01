namespace Allors.Development.Repository.Tagged
{
    using System.Collections.Generic;
    using System.Linq;

    using Meta;

    public abstract class TaggedComposite(TaggedMetaPopulation taggedMetaPopulation)
        : TaggedObjectType(taggedMetaPopulation)
    {
        protected abstract IComposite Composite { get; }

        // IComposite
        public IEnumerable<TaggedAssociationType> AssociationTypes => ((Composite)this.Composite).AssociationTypes.Select(this.TaggedMetaPopulation.Map).Where(v => v != null);

        public IEnumerable<TaggedRoleType> RoleTypes => ((Composite)this.Composite).RoleTypes.Select(this.TaggedMetaPopulation.Map).Where(v => v != null);

        public IEnumerable<TaggedInterface> DirectSupertypes => ((Composite)this.Composite).DirectSupertypes.Select(this.TaggedMetaPopulation.Map).Where(v => v != null);

        public IEnumerable<TaggedInterface> Supertypes => this.Composite.Supertypes.Select(this.TaggedMetaPopulation.Map).Where(v => v != null);

        public IEnumerable<TaggedClass> Classes => this.Composite.Classes.Select(this.TaggedMetaPopulation.Map).Where(v => v != null);

        public TaggedClass ExclusiveClass => this.TaggedMetaPopulation.Map(this.Composite.ExclusiveClass);

        public bool ExistClass => ((Composite)this.Composite).ExistClass;

        public bool ExistExclusiveClass => this.Composite.ExistExclusiveClass;

        public bool IsSynced => this.Composite.IsSynced;

        public bool AssignedIsSynced => this.Composite.AssignedIsSynced;

        // IComposite Extra
        public IEnumerable<TaggedAssociationType> ExclusiveAssociationTypes => ((Composite)this.Composite).ExclusiveAssociationTypes.Select(this.TaggedMetaPopulation.Map).Where(v => v != null);


        public IEnumerable<TaggedRoleType> ExclusiveRoleTypes => ((Composite)this.Composite).ExclusiveRoleTypes.Select(this.TaggedMetaPopulation.Map).Where(v => v != null);


        public IEnumerable<TaggedRoleType> UnitRoleTypes => this.RoleTypes.Where(roleType => roleType.ObjectType.IsUnit).ToArray();

        public IEnumerable<TaggedRoleType> CompositeRoleTypes => this.RoleTypes.Where(roleType => roleType.ObjectType.IsComposite).ToArray();


        public IEnumerable<TaggedRoleType> SortedExclusiveRoleTypes => this.ExclusiveRoleTypes.OrderBy(v => v.Name);


        public IEnumerable<TaggedRoleType> ExclusiveCompositeRoleTypes => this.ExclusiveRoleTypes.Where(roleType => roleType.ObjectType.IsComposite);


        public bool ExistSupertypes => this.Supertypes.Any();

        public bool ExistDirectSupertypes => this.DirectSupertypes.Any();


        public bool ExistAssociationTypes => this.AssociationTypes.Any();

        public bool ExistRoleTypes => this.AssociationTypes.Any();


        public IEnumerable<TaggedComposite> RelatedComposites =>
            this
                .Supertypes
                .Union(this.RoleTypes.Where(m => m.ObjectType.IsComposite).Select(v => v.ObjectType))
                .Union(this.AssociationTypes.Select(v => v.ObjectType)).Distinct()
                .Except(new[] { this })
                .Cast<TaggedComposite>();

    }
}
