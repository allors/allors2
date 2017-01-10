namespace Tests.Remote
{
    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    public class Population
    {
        private ISession session;

        public Person Administrator { get; }

        public Population(ISession session)
        {
            this.session = session;

            // Persons
            this.Administrator = new People(this.session).FindBy(MetaPerson.Instance.UserName, @"administrator");

            this.session.Derive(true);
            this.session.Commit();
        }
    }
}