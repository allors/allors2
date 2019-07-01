namespace Tests.ApplicationTests
{
    using System.Collections.Generic;
    using OpenQA.Selenium;
    using System.Reflection;
    using Components;
    using System;
    using System.Linq;
    using Xunit;
    using Allors;

    [Collection("Test collection")]
    public class OverviewPagesTest : Test
    {
        private readonly MethodInfo[] navigateTos;

        public OverviewPagesTest(TestFixture fixture)
            : base(fixture)
        {
            this.navigateTos = this.Sidenav.GetType()
                .GetMethods()
                .Where(v => v.Name.StartsWith("NavigateTo"))
                .ToArray();
        }

        [Fact]
        public async void Detail()
        {
            foreach (var page in this.OverviewPages())
            {
                var detailProperty = page.GetType().GetProperties().FirstOrDefault(v => v.Name.ToUpperInvariant().EndsWith("DETAIL"));
                dynamic detail = detailProperty.GetGetMethod().Invoke(page, null);
                detail.Click();
                Cancel(detail);
            }
        }

        private IEnumerable<Component> OverviewPages()
        {
            this.Login();

            foreach (var navigateTo in this.navigateTos)
            {
                var listPage = (Component)navigateTo.Invoke(this.Sidenav, null);

                var tableProperty = listPage.GetType().GetProperties().FirstOrDefault(v => v.PropertyType == typeof(MatTable));
                if (tableProperty != null)
                {
                    var table = (MatTable)tableProperty?.GetGetMethod().Invoke(listPage, null);
                    var action = table.Actions.FirstOrDefault(v => v.Equals("overview"));

                    if (action != null)
                    {
                        var objects = this.Session.Instantiate(table.ObjectIds);
                        foreach (IObject @object in objects)
                        {
                            listPage = (Component)navigateTo.Invoke(this.Sidenav, null);
                            table.Action(@object, action);

                            this.Driver.WaitForAngular();
                            var dialogElement = this.Driver.FindElement(By.CssSelector("mat-sidenav-content ng-component[data-test-scope]"));
                            var testScope = dialogElement.GetAttribute("data-test-scope");
                            var type = Assembly.GetExecutingAssembly().GetTypes().First(v => v.Name.Equals(testScope));
                            var page = (Component)Activator.CreateInstance(type, this.Driver);

                            yield return page;
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
