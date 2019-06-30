namespace Tests.ApplicationTests
{
    using System.Reflection;
    using Components;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Linq;
    using Xunit;
    using Allors;

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
                var page = (Component)navigateTo.Invoke(this.Sidenav, null);

                var createMethod = page.GetType().GetMethods().FirstOrDefault(v => v.Name.StartsWith("Create"));
                this.Logger.LogTrace($"{createMethod?.Name}");
                var dialog = (Component)createMethod?.Invoke(page, null);

                var cancelProperty = dialog?.GetType().GetProperties().FirstOrDefault(v => v.Name.Equals("cancel", StringComparison.InvariantCultureIgnoreCase));
                dynamic cancel = cancelProperty?.GetGetMethod().Invoke(dialog, null);

                cancel?.Click();
            }
        }

        [Fact]
        public async void Overview()
        {
            this.Login();

            foreach (var navigateTo in this.navigateTos)
            {
                var page = (Component)navigateTo.Invoke(this.Sidenav, null);

                var tableProperty = page.GetType().GetProperties().FirstOrDefault(v => v.PropertyType == typeof(MatTable));
                if (tableProperty != null)
                {
                    var table = (MatTable)tableProperty?.GetGetMethod().Invoke(page, null);
                    var action = table.Actions.FirstOrDefault(v => v.Equals("overview"));

                    if (action != null)
                    {
                        var objects = this.Session.Instantiate(table.ObjectIds);
                        foreach (IObject @object in objects)
                        {
                            table.Action(@object, action);
                            this.Driver.Navigate().Back();
                        }
                    }
                }
            }
        }
    }
}
