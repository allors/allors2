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

        public Input<SerialisedItemCharacteristicListPage> Name => this.Input(formControlName: "name");

        public Anchor<SerialisedItemCharacteristicListPage> AddNew => this.Anchor(By.LinkText("Add New"));
    }
}
