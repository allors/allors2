// <copyright file="TesterFactory.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Autotest.Testers
{
    using System.Linq;
    using Autotest.Html;

    public partial class TesterFactory
    {
        private static Tester BaseCreate(Element element)
        {
            // Angular
            switch (element.Name)
            {
                case "input":
                    return new InputTester(element);

                case "button":
                    return new ButtonTester(element);

                case "a":
                    return new AnchorTester(element);

                case "a-mat-factory-fab":
                    return new AllorsMaterialFactoryFabTester(element);

                case "a-mat-table":
                    return new AllorsMaterialTableTester(element);
            }

            if (element.Component != null)
            {
                var component = element.Component;
                if (component.Type != null)
                {
                    var type = component.Type;

                    // Allors
                    if (type.Bases?.Contains("RoleField") == true)
                    {
                        return new RoleFieldTester(element);
                    }
                }

                return new ComponentElementTester(element);
            }

            return null;
        }
    }
}
