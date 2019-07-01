namespace Tests.ApplicationTests
{
    using System.Reflection;
    using Components;
    using OpenQA.Selenium;
    using System;
    using System.Linq;
    using Xunit;
    using Allors;

    [Collection("Test collection")]
    public class ListPagesTest : Test
    {
        private readonly MethodInfo[] navigateTos;

        public ListPagesTest(TestFixture fixture)
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
                var createMethods = page.GetType().GetMethods().Where(v => v.Name.StartsWith("Create"));
                foreach (var createMethod in createMethods)
                {
                    var dialog = (Component)createMethod?.Invoke(page, null);
                    Cancel(dialog);
                }
            }
        }

        [Fact]
        public async void Edit()
        {
            this.Login();

            foreach (var navigateTo in this.navigateTos)
            {
                var page = (Component)navigateTo.Invoke(this.Sidenav, null);

                var tableProperty = page.GetType().GetProperties().FirstOrDefault(v => v.PropertyType == typeof(MatTable));
                if (tableProperty != null)
                {
                    var table = (MatTable)tableProperty?.GetGetMethod().Invoke(page, null);
                    var action = table.Actions.FirstOrDefault(v => v.Equals("edit"));

                    if (action != null)
                    {
                        var objects = this.Session.Instantiate(table.ObjectIds);
                        foreach (IObject @object in objects)
                        {
                            table.Action(@object, action);

                            this.Driver.WaitForAngular();
                            var dialogElement = this.Driver.FindElement(By.CssSelector("mat-dialog-container ng-component[data-test-scope]"));
                            var testScope = dialogElement.GetAttribute("data-test-scope");
                            var type = Assembly.GetExecutingAssembly().GetTypes().First(v => v.Name.Equals(testScope));
                            var dialog = (Component)Activator.CreateInstance(type, this.Driver);

                            Cancel(dialog);
                        }
                    }
                }
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

        private static void Cancel(Component dialog)
        {
            var cancelProperty = dialog?.GetType().GetProperties().FirstOrDefault(v => v.Name.Equals("cancel", StringComparison.InvariantCultureIgnoreCase));
            dynamic cancel = cancelProperty?.GetGetMethod().Invoke(dialog, null);

            cancel?.Click();
        }
    }
}
