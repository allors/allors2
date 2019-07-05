namespace Allors.Domain
{
    using Allors.Meta;

    public partial class Users
    {
        protected override void CustomPrepare(Setup setup)
        {
            setup.AddDependency(this.ObjectType, M.Locale.ObjectType);
            setup.AddDependency(this.ObjectType, M.Singleton.ObjectType);
            setup.AddDependency(this.ObjectType, M.ContactMechanismPurpose.ObjectType);
        }
    }
}
