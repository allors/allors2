namespace Pages.SerialisedItemCharacteristicTest
{

    using OpenQA.Selenium;

    using Angular.Html;

    using Pages.ApplicationTests;

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
