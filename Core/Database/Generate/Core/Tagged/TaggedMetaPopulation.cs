namespace Allors.Development.Repository.Tagged
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Meta;


    public class TaggedMetaPopulation
    {
        private readonly Dictionary<IMetaObject, TaggedMetaObject> mapping;
        private readonly Dictionary<Guid, TaggedMetaObject> byId;

        public TaggedMetaPopulation(IMetaPopulation metaPopulation, ISet<string> tags)
        {
            this.MetaPopulation = metaPopulation;
            this.Tags = tags;

            this.mapping = new Dictionary<IMetaObject, TaggedMetaObject>();

            foreach (var unit in this.MetaPopulation.Units)
            {
                this.mapping.Add(unit, new TaggedUnit(this, unit));
            }

            foreach (var @interface in this.MetaPopulation.Interfaces.Where(v => tags.Overlaps(v.Tags)))
            {
                this.mapping.Add(@interface, new TaggedInterface(this, @interface));
            }

            foreach (var @class in this.MetaPopulation.Classes.Where(v => tags.Overlaps(v.Tags)))
            {
                this.mapping.Add(@class, new TaggedClass(this, @class));
            }

            foreach (var relationType in this.MetaPopulation.RelationTypes.Where(v => tags.Overlaps(v.Tags)))
            {
                this.mapping.Add(relationType, new TaggedRelationType(this, relationType));
                this.mapping.Add(relationType.AssociationType, new TaggedAssociationType(this, relationType.AssociationType));
                this.mapping.Add(relationType.RoleType, new TaggedRoleType(this, relationType.RoleType));
            }

            this.byId = this.mapping.Values.ToDictionary(v => v.Id, v => v);

        }

        public ISet<string> Tags { get; set; }

        public IMetaPopulation MetaPopulation { get; }
        
        public IEnumerable<TaggedUnit> Units => this.mapping.Values.OfType<TaggedUnit>();

        public IEnumerable<TaggedComposite> Composites => this.mapping.Values.OfType<TaggedComposite>();

        public IEnumerable<TaggedInterface> Interfaces => this.mapping.Values.OfType<TaggedInterface>();

        public IEnumerable<TaggedClass> Classes => this.mapping.Values.OfType<TaggedClass>();

        public IEnumerable<TaggedRelationType> RelationTypes => this.mapping.Values.OfType<TaggedRelationType>();

        #region Mappers
        public TaggedMetaObject Map(IMetaObject v) => this.mapping.GetValueOrDefault(v);

        public TaggedObjectType Map(IObjectType v) => (TaggedObjectType) this.mapping.GetValueOrDefault(v);

        public TaggedUnit Map(IUnit v) => (TaggedUnit) this.mapping.GetValueOrDefault(v);

        public TaggedComposite Map(IComposite v) => (TaggedComposite) this.mapping.GetValueOrDefault(v);

        public TaggedInterface Map(IInterface v) => (TaggedInterface) this.mapping.GetValueOrDefault(v);

        public TaggedClass Map(IClass v) => (TaggedClass) this.mapping.GetValueOrDefault(v);

        public TaggedOperandType Map(IOperandType v) => (TaggedOperandType) this.mapping.GetValueOrDefault(v);

        public TaggedRelationType Map(IRelationType v) => (TaggedRelationType) this.mapping.GetValueOrDefault(v);

        public TaggedPropertyType Map(IPropertyType v) => (TaggedPropertyType) this.mapping.GetValueOrDefault(v);

        public TaggedAssociationType Map(IAssociationType v) => (TaggedAssociationType) this.mapping.GetValueOrDefault(v);

        public TaggedRoleType Map(IRoleType v) => (TaggedRoleType) this.mapping.GetValueOrDefault(v);
        #endregion

        public ValidationLog Validate() => (ValidationLog)this.MetaPopulation.Validate();

        public object Find(Guid id) => this.byId[id];
    }
}
