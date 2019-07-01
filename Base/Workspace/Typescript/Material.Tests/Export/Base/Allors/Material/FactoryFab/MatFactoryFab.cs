using System.Linq;
using Allors.Meta;

namespace Components
{
    using OpenQA.Selenium;

    public class MatFactoryFab : SelectorComponent
    {
        public MatFactoryFab(IWebDriver driver, Composite composite, By selector)
            : base(driver)
        {
            this.Composite = composite;
            Selector = selector;
        }

        public Composite Composite { get; set; }

        public override By Selector { get; }

        public void Create(Class @class = null)
        {
            this.Anchor(By.CssSelector("[mat-fab]")).Click();

            if (@class != null && this.Composite.Classes.Count() > 1 )
            { 
                this.Button(By.CssSelector($"button[data-allors-class='{@class.Name}']")).Click();
            }
        }
    }
}
