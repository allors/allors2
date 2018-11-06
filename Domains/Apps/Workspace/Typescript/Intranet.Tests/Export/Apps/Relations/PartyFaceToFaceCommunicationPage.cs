namespace Tests.Intranet.Relations
{
    using System;
    using Allors.Domain;
    using Allors.Meta;

    using Tests.Intranet;

    using OpenQA.Selenium;

    using Tests.Components.Html;
    using Tests.Components.Material;

    public class PartyFaceToFaceCommunicationPage : MainPage
    {
        public PartyFaceToFaceCommunicationPage(IWebDriver driver)
            : base(driver)
        {
        }

        public MaterialTextArea Comment => new MaterialTextArea(this.Driver, roleType: M.Person.Comment);

        public MaterialDatetimePicker ScheduledStart => new MaterialDatetimePicker(this.Driver, M.EmailCommunication.ScheduledStart);

        public Button Save => new Button(this.Driver, By.XPath("//button/span[contains(text(), 'SAVE')]"));
    }
}
