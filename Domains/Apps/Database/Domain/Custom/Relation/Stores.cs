namespace Allors.Domain
{
    using Allors.Meta;

    public partial class Stores
    {
        protected override void AppsPrepare(Setup config)
        {
            config.AddDependency(this.ObjectType, M.ProcessFlow.ObjectType);
        }
    }
}
