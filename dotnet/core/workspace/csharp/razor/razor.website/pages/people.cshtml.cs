namespace Razor.Pages
{
    using Allors.Workspace;
    using Allors.Workspace.Data;
    using Allors.Workspace.Domain;
    using Allors.Workspace.Meta;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public class PeopleModel : PageModel
    {
        public PeopleModel(IDatabase database, Workspace workspace)
        {
            this.Database = database;
            this.Workspace = workspace;
        }

        public IDatabase Database { get; }

        public Workspace Workspace { get; }

        public Person[] People { get; set; }


        public async void OnGet()
        {
            var context = new Context(this.Database, this.Workspace);

            var pull = new Pull
            {
                Extent = new Extent(M.Person.ObjectType),
            };

            var result = await context.Load(pull);

            this.People = result.GetCollection<Person>("People");
        }
    }
}
