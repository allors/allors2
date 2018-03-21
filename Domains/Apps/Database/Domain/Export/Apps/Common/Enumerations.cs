namespace Allors.Domain
{
    using Allors.Meta;

    public partial class Enumerations
    {
        protected override void AppsPrepare(Setup config)
        {
            config.AddDependency(this.ObjectType, M.Singleton.ObjectType);
        }
    }
}
