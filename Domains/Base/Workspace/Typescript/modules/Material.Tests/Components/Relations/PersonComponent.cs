using Angular;

namespace src.allors.material.custom.relations.people.person
{
    using Allors.Meta;

    using OpenQA.Selenium;

    using Angular.Html;
    using Angular.Material;

    public partial class PersonComponent 
    {
        public MaterialInput<PersonComponent> FirstName => this.MaterialInput(M.Person.FirstName);

        public MaterialInput<PersonComponent> MiddleName => this.MaterialInput(M.Person.MiddleName);

        public MaterialInput<PersonComponent> LastName => this.MaterialInput(M.Person.LastName);

        public Button<PersonComponent> Save => this.Button(By.XPath("//button/span[contains(text(), 'SAVE')]"));
    }
}
