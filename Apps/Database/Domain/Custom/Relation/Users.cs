namespace Allors.Domain
{
    using Allors.Meta;

    public partial class Users
    {
        protected override void CustomPrepare(Setup config)
        {
            base.CustomPrepare(config);

            config.AddDependency(this.ObjectType, M.Locale.ObjectType);
            config.AddDependency(this.ObjectType, M.Singleton.ObjectType);
            config.AddDependency(this.ObjectType, M.ContactMechanismPurpose.ObjectType);
        }

        protected override void CustomSetup(Setup setup)
        {
            
        }
    }
}
