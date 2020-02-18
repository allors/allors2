namespace Allors.Database.Adapters.Memory.IO
{
    using Allors.Database.Adapters.Memory;
    using Allors.Serialization;
    using Allors.Meta;

    public class CompositeRoleData : IRoleData
    {
        private readonly Strategy strategy;
        private readonly IRelationType relationType;

        public CompositeRoleData(Strategy strategy, IRelationType relationType)
        {
            this.strategy = strategy;
            this.relationType = relationType;
        }

        public object Value => this.strategy.GetCompositeRole(this.relationType).Id;
    }
}
