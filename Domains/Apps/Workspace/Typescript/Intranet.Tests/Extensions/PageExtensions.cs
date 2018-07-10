namespace Intranet.Tests
{
    using System.Threading.Tasks;

    using PuppeteerSharp;

    public static class PageExtensions
    {
        public static async Task WaitForAngularAsync(this Page page)
        {
            const string Function =
                @"
async () => await new Promise(resolve => {

    window.getAngularTestability(document.querySelector('app-root'))
      .whenStable(function(didWork) {
        resolve(didWork);
      });

})
";

            var didWork = true;
            while (didWork)
            {
                didWork = await page.EvaluateFunctionAsync<dynamic>(Function);
            }
        }
    }
}
