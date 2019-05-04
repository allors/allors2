namespace Autotest.Testers
{
    using System;
    using System.Linq;
    using Autotest.Html;

    public partial class TesterFactory
    {
        private static Tester BaseCreate(Element element)
        {
            // Allors
            if (element.Attributes.Any(v => string.Equals(v.Name, "[roleType]", StringComparison.OrdinalIgnoreCase)))
            {
                return new RoleFieldTester(element);
            }

            // Angular
            switch (element.Name)
            {
                case "input":
                    return new InputTester(element);

                case "button":
                    return new ButtonTester(element);

                case "a":
                    return new AnchorTester(element);

                case "a-mat-table":
                    return new AllorsMaterialTableTester(element);
            }

            if (element.Component != null)
            {
                return new ComponentElementTester(element);
            }

            return null;
        }
    }
}