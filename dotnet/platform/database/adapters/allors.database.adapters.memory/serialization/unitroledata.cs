namespace Allors.Database.Adapters.Memory.IO
{
    using Allors.Database.Adapters.Memory;
    using Allors.Meta;
    using Allors.Serialization;

    public class UnitRoleData : IRoleData
    {
        private readonly Strategy strategy;
        private readonly IRelationType relationType;

        public UnitRoleData(Strategy strategy, IRelationType relationType)
        {
            this.strategy = strategy;
            this.relationType = relationType;
        }

        public object Value => this.strategy.GetUnitRole(this.relationType);
    }
}
