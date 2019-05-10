namespace src.allors.material.custom.relations.people.person
{
    public partial class PersonOverviewComponent 
    {
        public PersonComponent EditAndNavigate()
        {
            this.Edit.Click();
            return new PersonComponent(this.Driver);
        }
    }
}
