namespace Intranet.Pages.Relations
{
    using System;
    using Allors.Domain;
    using Allors.Meta;

    using Intranet.Tests;

    using OpenQA.Selenium;

    public class PartyFaceToFaceCommunicationPage : MainPage
    {
        public PartyFaceToFaceCommunicationPage(IWebDriver driver)
            : base(driver)
        {
        }

        public MaterialTextArea Comment => new MaterialTextArea(this.Driver, roleType: M.Person.Comment);

        public Button Save => new Button(this.Driver, By.XPath("//button/span[contains(text(), 'SAVE')]"));
    }
}
