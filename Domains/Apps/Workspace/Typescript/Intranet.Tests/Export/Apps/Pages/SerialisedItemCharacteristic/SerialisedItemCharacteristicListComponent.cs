using src.app.main;

namespace Pages.SerialisedItemCharacteristicTest
{

    using OpenQA.Selenium;

    using Components;

    using Pages.ApplicationTests;

    public class SerialisedItemCharacteristicListComponent : MainComponent
    {
        public SerialisedItemCharacteristicListComponent(IWebDriver driver)
            : base(driver)
        {
        }

        public Input<SerialisedItemCharacteristicListComponent> Name => this.Input(formControlName: "name");

        public Anchor<SerialisedItemCharacteristicListComponent> AddNew => this.Anchor(By.LinkText("Add New"));
    }
}
