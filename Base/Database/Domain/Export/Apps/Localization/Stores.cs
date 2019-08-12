namespace Allors.Domain
{
    using Allors.Meta;

    public partial class Stores
    {
        protected override void BasePrepare(Setup setup)
        {
            setup.AddDependency(this.ObjectType, M.BillingProcess.ObjectType);
            setup.AddDependency(this.ObjectType, M.InternalOrganisation.ObjectType);
        }
    }
}
