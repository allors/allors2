namespace Allors.Domain
{
    using Allors.Meta;

    public partial class Users
    {
        protected override void BasePrepare(Setup config)
        {
            base.BasePrepare(config);

            config.AddDependency(this.ObjectType, M.Locale.ObjectType);
            config.AddDependency(this.ObjectType, M.Singleton.ObjectType);
            config.AddDependency(this.ObjectType, M.ContactMechanismPurpose.ObjectType);
        }

        protected override void CustomSetup(Setup setup)
        {
            
        }
    }
}
