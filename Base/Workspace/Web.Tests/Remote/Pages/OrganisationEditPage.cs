namespace Tests.Remote.Pages
{
    using System.Collections.Generic;
    using Allors.Domain;
    using OpenQA.Selenium;
    using Protractor;

    public class OrganisationEditPage : MainPage
    {
        public OrganisationEditPage(NgWebDriver driver)
            : base(driver)
        {
        }

        #region Controls
        public IEnumerable<NgWebElement> OrganisationsField => this.Driver.FindElements(NgBy.Repeater("organisation in vm.organisations"));

        private NgWebElement NameField => this.Driver.FindElement(NgBy.Model("vm.organisation.Name"));

        private NgWebElement OwnerField => this.Driver.FindElement(NgBy.Model("vm.organisation.Owner"));
        
        private IEnumerable<NgWebElement> EmployeesField => this.Driver.FindElements(NgBy.Repeater("vm.organisation.Employees"));

        private NgWebElement AddButton => this.Driver.FindElement(By.Id("add"));

        private NgWebElement SaveButton => this.Driver.FindElement(By.Id("save"));

        private NgWebElement ResetButton => this.Driver.FindElement(By.Id("reset"));

        #endregion
        
        public OrganisationEditPage Edit(Organisation organisation)
        {
            foreach (var filterOrganisationField in this.OrganisationsField)
            {
                var nameElement = filterOrganisationField.FindElement(NgBy.Binding("organisation.Name"));
                if (nameElement.Text.Equals(organisation.Name))
                {
                    var editLink = filterOrganisationField.FindElement(By.LinkText("Edit"));
                    editLink.Click();
                    break;
                }
            }

            return this;
        }

        public OrganisationEditPage EnterName(string name)
        {
            var nameField = this.NameField;
            nameField.Clear();
            nameField.SendKeys(name);

            return this;
        }

        public OrganisationEditPage AddEmployees(params Person[] persons)
        {
            var employeesField = this.EmployeesField;
 
            return this;
        }

        public OrganisationEditPage Save()
        {
            this.SaveButton.Click();
            return this;
        }
    }
}