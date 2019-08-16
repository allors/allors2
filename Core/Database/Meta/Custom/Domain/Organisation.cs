namespace Allors.Meta
{
    using Allors.Data;

    public partial class MetaOrganisation
    {
        public Tree AngularEmployees { get; private set; }

        public Tree AngularShareholders { get; private set; }

        internal override void CustomExtend()
        {
            this.Name.IsRequired = true;

            var organisation = this;
            var person = MetaPerson.Instance;

            this.AngularEmployees = new Tree(organisation.Class)
                .Add(organisation.Employees);

            this.AngularShareholders = new Tree(organisation.Class)
                .Add(organisation.Shareholders,
                    new Tree(person.Class)
                        .Add(person.Photo));
        }
    }
}
