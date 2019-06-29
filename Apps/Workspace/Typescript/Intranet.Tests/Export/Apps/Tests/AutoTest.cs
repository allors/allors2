namespace Tests.ApplicationTests
{
    using System.Reflection;
    using Components;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Linq;
    using Xunit;

    [Collection("Test collection")]
    public class AutoTest : Test
    {
        private readonly MethodInfo[] navigateTos;

        public AutoTest(TestFixture fixture)
            : base(fixture)
        {
            this.navigateTos = this.Sidenav.GetType()
                .GetMethods()
                .Where(v => v.Name.StartsWith("NavigateTo"))
                .ToArray();
        }

        [Fact]
        public async void Navigate()
        {
            this.Login();

            foreach (var navigateTo in this.navigateTos)
            {
                var page = (Component)navigateTo.Invoke(this.Sidenav, null);
            }
        }

        [Fact]
        public async void Create()
        {
            this.Login();
            
            foreach (var navigateTo in this.navigateTos)
            {
                this.Logger.LogTrace($"{navigateTo.Name}");

                var page = (Component)navigateTo.Invoke(this.Sidenav, null);

                var createMethod = page.GetType().GetMethods().FirstOrDefault(v => v.Name.StartsWith("Create"));
                this.Logger.LogTrace($"{createMethod?.Name}");
                var dialog = (Component)createMethod?.Invoke(page, null);

                dialog?.Driver.WaitForAngular();

                var cancelProperty = dialog?.GetType().GetProperties().FirstOrDefault(v => v.Name.Equals("cancel", StringComparison.InvariantCultureIgnoreCase));
                dynamic cancel = cancelProperty?.GetGetMethod().Invoke(dialog, null);

                dialog?.Driver.WaitForAngular();

                cancel?.Click();
            }
        }
    }
}
