namespace Application.Sheets
{
    using System;
    using System.Drawing;
    using Allors.Excel;
    using Allors.Workspace;
    using Allors.Workspace.Data;
    using Allors.Workspace.Domain;
    using Allors.Workspace.Meta;
    using Dipu.Excel;
    using Microsoft.Extensions.DependencyInjection;
    using Task = System.Threading.Tasks.Task;

    public class PeopleSheet
    {
        public PeopleSheet(Program program)
        {
            this.Context = new Context(program.Client.Database, program.Client.Workspace);
            this.Sheet = program.ActiveWorkbook.AddWorksheet();
            this.Binder = new Binder(this.Sheet);
            this.Binder.ToDomained += this.BinderOnToDomained;

            this.MessageService = program.ServiceProvider.GetRequiredService<IMessageService>();
            this.ErrorService = program.ServiceProvider.GetRequiredService<IErrorService>();
        }

        public Context Context { get; }

        public IWorksheet Sheet { get; }

        public Binder Binder { get; set; }

        public IMessageService MessageService { get; set; }

        public IErrorService ErrorService { get; }

        public async Task Load()
        {
            var now = DateTime.Now;

            var pull = new Pull
            {
                Extent = new Filter(M.Person.ObjectType),

                Results = new[]
                {
                    new Allors.Workspace.Data.Result
                    {
                        Fetch = new Fetch()
                        {
                            Include = new[]
                            {
                                new Node(M.Person.OrganisationWhereManager),
                            },
                        },
                    },
                },
            };

            var result = await this.Context.Load(pull);

            var people = result.GetCollection<Person>();

            var index = 0;
            var firstName = new Column { Header = "First Name", Index = index++, NumberFormat = "@" };
            var lastName = new Column { Header = "Last Name", Index = index++, NumberFormat = "@" };

            var columns = new[]
            {
                firstName,
                lastName,
            };

            foreach (var column in columns)
            {
                this.Sheet[0, column.Index].Value = column.Header;
                this.Sheet[0, column.Index].Style = new Style(Color.LightBlue, Color.Black);
            }

            var row = 1;
            foreach (var customer in people)
            {
                foreach (var column in columns)
                {
                    this.Sheet[row, column.Index].NumberFormat = column.NumberFormat;
                }

                this.Binder.Set(row, firstName.Index, new RoleTypeBinding(customer, M.Person.FirstName));
                this.Binder.Set(row, lastName.Index, new RoleTypeBinding(customer, M.Person.LastName));

                row++;
            }

            this.Binder.ToCells();

            await this.Sheet.Flush();
        }

        private async void BinderOnToDomained(object sender, EventArgs e)
        {
            var response = await this.Context.Save();
            if (response.HasErrors)
            {
                this.ErrorService.Handle(response, this.Context.Session);
            }
            else
            {
                this.MessageService.Show("Successfully saved", "Info");
            }

            await this.Load();
        }
    }
}
