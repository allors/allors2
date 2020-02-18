namespace Allors.Database.Adapters.Memory.IO
{
    using System.Linq;
    using Allors.Database.Adapters.Memory;
    using Allors.Serialization;
    using Allors.Meta;

    public class CompositesRoleData : IRoleData
    {
        private readonly Strategy strategy;
        private readonly IRelationType relationType;

        public CompositesRoleData(Strategy strategy, IRelationType relationType)
        {
            this.strategy = strategy;
            this.relationType = relationType;
        }

        public object Value => this.strategy.GetCompositeRoles(this.relationType).ToArray().Select(v => v.Id).ToArray();
    }
}
