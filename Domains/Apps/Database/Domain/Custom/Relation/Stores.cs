namespace Allors.Domain
{
    using Allors.Meta;

    public partial class Stores
    {
        protected override void AppsPrepare(Setup config)
        {
            base.BasePrepare(config);

            config.AddDependency(this.ObjectType, M.InternalOrganisation.ObjectType);
            config.AddDependency(this.ObjectType, M.ProcessFlow.ObjectType);
        }

        protected override void CustomSetup(Setup setup)
        {
           
        }
    }
}
