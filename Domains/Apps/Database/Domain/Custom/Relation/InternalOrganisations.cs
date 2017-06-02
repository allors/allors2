namespace Allors.Domain
{
    using Allors.Meta;
    using System.IO;

    public partial class InternalOrganisations
    {
        protected override void BasePrepare(Setup config)
        {
            base.BasePrepare(config);

            config.AddDependency(this.ObjectType, M.Locale.ObjectType);
            config.AddDependency(this.ObjectType, M.Singleton.ObjectType);
        }

        protected override void CustomSetup(Setup setup)
        {
         
        }
    }
}
