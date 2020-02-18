namespace Allors.Database.Adapters.Memory.IO
{
    using System.Collections;
    using System.Collections.Generic;
    using Allors.Database.Adapters.Memory;
    using Allors.Serialization;

    public class ObjectData : IObjectData
    {
        private readonly Strategy strategy;

        public ObjectData(Strategy strategy) => this.strategy = strategy;

        public long Id => this.strategy.ObjectId;

        public long Version => this.strategy.ObjectVersion;

        public IEnumerator<IRoleData> GetEnumerator()
        {
            foreach (var roleType in this.strategy.Class.RoleTypes)
            {
                var relationType = roleType.RelationType;

                if (this.strategy.ExistRole(relationType))
                {
                    if (roleType.ObjectType.IsUnit)
                    {
                        yield return new UnitRoleData(this.strategy, relationType);
                    }
                    else
                    {
                        if (roleType.IsOne)
                        {
                            yield return new CompositeRoleData(this.strategy, relationType);
                        }
                        else
                        {
                            yield return new CompositesRoleData(this.strategy, relationType);
                        }
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
