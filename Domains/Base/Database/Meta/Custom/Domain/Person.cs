namespace Allors.Meta
{
    using Allors.Data;

    public partial class MetaPerson
    {
        public Tree AngularHome;

        internal override void CustomExtend()
        {
            this.Delete.Workspace = true;

            this.FirstName.RelationType.Workspace = true;
            this.LastName.RelationType.Workspace = true;
            this.MiddleName.RelationType.Workspace = true;
            
            var person = this;
            this.AngularHome = new Tree(person.Class)
                    .Add(person.Photo);
        }
    }
}