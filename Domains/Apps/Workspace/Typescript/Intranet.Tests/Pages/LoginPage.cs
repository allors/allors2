namespace Intranet.Pages
{
    using System.Threading.Tasks;

    public class LoginPage : BasePage
    {
        public LoginPage(PuppeteerSharp.Page page) : base(page)
        {
        }

        public async Task Login(string userName = "administrator")
        {
            await this.TypeAsync("input[formcontrolname='userName']", userName);
            await this.ClickAsync("button");
        }
    }
}
