namespace src.allors.material.apps.objects.emailcommunication.edit
{
    using Allors.Meta;

    using Angular.Html;
    using Angular.Material;

    using OpenQA.Selenium;

    public partial class EmailCommunicationEditComponent 
    {
        public Button<EmailCommunicationEditComponent> Save => this.Button(By.XPath("//button/span[contains(text(), 'SAVE')]"));

        public Anchor<EmailCommunicationEditComponent> List => this.Anchor(By.CssSelector("a[href='/contacts/people']"));

        public Button<EmailCommunicationEditComponent> NewCommunicationEvent => this.Button(By.CssSelector("div[data-allors-class='communicationevent']"));
    }
}
