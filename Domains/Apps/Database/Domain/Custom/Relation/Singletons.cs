namespace Allors.Domain
{
    using Allors.Meta;

    public partial class Singletons
    {
        protected override void CustomPrepare(Setup config)
        {
            config.AddDependency(this.ObjectType, M.Locale.ObjectType);
        }
    }
}
