// <copyright file="ListPagesTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.ApplicationTests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Components;
    using Xunit;

    [Collection("Test collection")]
    public class ListPagesTest : Test
    {
        private readonly MethodInfo[] navigateTos;

        public ListPagesTest(TestFixture fixture)
            : base(fixture)
        {
            var navigateTos = this.Sidenav.GetType()
                .GetMethods()
                .Where(v => v.Name.StartsWith("NavigateTo"))
                .ToArray();

            // Uncomment next line to only test a certain page
            // navigateTos = navigateTos.Where(v => v.Name.Equals("NavigateToSpareParts")).ToArray();
            this.navigateTos = navigateTos;
        }

        [Fact]
        [Trait("Category", "Generic")]
        public async void Navigate()
        {
            this.Login();

            foreach (var navigateTo in this.navigateTos)
            {
                var page = (Component)navigateTo.Invoke(this.Sidenav, null);
            }
        }

        [Fact]
        [Trait("Category", "Generic")]
        public async void Create()
        {
            this.Login();

            var navigateTos = this.navigateTos;
            foreach (var navigateTo in navigateTos)
            {
                var page = (Component)navigateTo.Invoke(this.Sidenav, null);
                if (page.GetType().GetProperties().Any(v => v.Name.Equals("Factory")))
                {
                    var factory = (MatFactoryFab)((dynamic)page).Factory;

                    if (this.Driver.SelectorIsVisible(factory.Selector))
                    {
                        var classes = factory.Classes;

                        foreach (var @class in classes)
                        {
                            factory.Create(@class);
                            var dialog = this.Driver.GetDialog();
                            Cancel(dialog);
                        }
                    }
                }
            }
        }

        [Fact]
        [Trait("Category", "Generic")]
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

                    if (this.Driver.SelectorIsVisible(table.Selector))
                    {
                        var action = table.Actions.FirstOrDefault(v => v.Equals("edit"));

                        if (action != null)
                        {
                            var objects = this.Session.Instantiate(table.ObjectIds);
                            foreach (var @object in objects)
                            {
                                table.Action(@object, action);
                                var dialog = this.Driver.GetDialog();
                                Cancel(dialog);
                            }
                        }
                    }
                }
            }
        }

        [Fact]
        [Trait("Category", "Generic")]
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

                    if (this.Driver.SelectorIsVisible(table.Selector))
                    {
                        var action = table.Actions.FirstOrDefault(v => v.Equals("overview"));
                        if (action != null)
                        {
                            var objects = this.Session.Instantiate(table.ObjectIds);
                            foreach (var @object in objects)
                            {
                                table.Action(@object, action);
                                this.Driver.Navigate().Back();
                            }
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
