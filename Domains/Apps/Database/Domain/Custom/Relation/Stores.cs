namespace Allors.Domain
{
    using Allors.Meta;

    public partial class Stores
    {
        protected override void AppsPrepare(Setup config)
        {
            base.BasePrepare(config);

            config.AddDependency(this.ObjectType, M.InternalOrganisation.ObjectType);
        }

        protected override void CustomSetup(Setup setup)
        {
           
        }
    }
}
