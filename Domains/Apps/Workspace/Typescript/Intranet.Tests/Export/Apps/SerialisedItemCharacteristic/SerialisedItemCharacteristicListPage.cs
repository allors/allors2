namespace Tests.Intranet.SerialisedItemCharacteristicTest
{
    using Tests.Intranet;

    using OpenQA.Selenium;

    using Tests.Components.Html;

    public class SerialisedItemCharacteristicListPage : MainPage
    {
        public SerialisedItemCharacteristicListPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Input Name => new Input(this.Driver, formControlName: "name");

        public Anchor AddNew => new Anchor(this.Driver, By.LinkText("Add New"));
    }
}
