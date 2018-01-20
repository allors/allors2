namespace Allors.Domain
{
    using Allors.Meta;

    public partial class Singletons
    {
        protected override void AppsPrepare(Setup config)
        {
            config.AddDependency(this.ObjectType, M.Locale.ObjectType);
        }

        protected override void AppsSetup(Setup setup)
        {
            var singleton = this.Instance;
            singleton.DefaultLocale = new Locales(this.Session).EnglishGreatBritain;
            singleton.PreferredCurrency = new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR");
        }
    }
}
