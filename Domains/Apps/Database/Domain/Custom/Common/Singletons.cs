namespace Allors.Domain
{ 
    public partial class Singletons
    {
        protected override void CustomSetup(Setup setup)
        {
            var singleton = this.Instance;
            singleton.DefaultLocale = new Locales(this.Session).DutchBelgium;
        }
    }
}
