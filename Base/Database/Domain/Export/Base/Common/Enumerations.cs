namespace Allors.Domain
{
    using Allors.Meta;

    public partial class Enumerations
    {
        protected override void BasePrepare(Setup setup) => setup.AddDependency(this.ObjectType, M.Singleton.ObjectType);
    }
}
