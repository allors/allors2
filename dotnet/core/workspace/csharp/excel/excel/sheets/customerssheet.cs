namespace Application.Sheets
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using Allors.Excel;
    using Allors.Workspace;
    using Allors.Workspace.Data;
    using Allors.Workspace.Domain;
    using Allors.Workspace.Meta;
    using Excel;
    using Result = Allors.Workspace.Data.Result;
    using Task = System.Threading.Tasks.Task;

    public class CustomersSheet : ISheet, ISaveable
    {
        private const int NewRowCount = 10;

        public CustomersSheet(Program program)
        {
            this.Client = program.Client;
            this.Sheet = program.ActiveWorkbook.AddWorksheet();
            this.Binder = new Binder(this.Sheet);
            this.Binder.ToDomained += this.BinderOnToDomained;

            this.Controls = new Controls(this.Sheet);

            this.MessageService = program.ServiceLocator.GetMessageService();
            this.ErrorService = program.ServiceLocator.GetErrorService();
        }

        public IWorksheet Sheet { get; }

        private Client Client { get; }

        private Binder Binder { get; }

        private Controls Controls { get; }

        private IMessageService MessageService { get; }

        private IErrorService ErrorService { get; }

        private Context Context { get; set; }

        private int StartRowNewItems { get; set; }

        public async Task Init()
        {
            this.Context = new Context(this.Client.Database, this.Client.Workspace);

            var pull = new Pull
            {
                Extent = new Extent(M.Organisation.ObjectType)
                {
                    Predicate = new Equals(M.Organisation.IsCustomer) { Value = true }
                },
                Results = new[]
                {
                    new Result
                    {
                        Fetch = new Fetch
                        {
                            Include = new[]
                            {
                                new Node(M.Organisation.Manager)
                            }
                        }
                    }
                }
            };

            var result = await this.Context.Load(pull);

            var customers = result.GetCollection<Organisation>();

            var index = 0;
            var name = new Column { Header = "Name", Index = index++, NumberFormat = "@" };

            var columns = new[]
            {
                name
            };

            foreach (var column in columns)
            {
                this.Sheet[0, column.Index].Value = column.Header;
                this.Sheet[0, column.Index].Style = new Style(Color.LightBlue, Color.Black);
            }

            var row = 1;
            foreach (var customer in customers)
            {
                foreach (var column in columns)
                {
                    this.Sheet[row, column.Index].NumberFormat = column.NumberFormat;
                }

                this.Controls.TextBox(row, name.Index, customer, M.Organisation.Name);

                row++;
            }

            this.StartRowNewItems = row;

            for (var i = row; i < row + NewRowCount; i++)
            {
                this.Controls.TextBox(i, name.Index, null, M.Organisation.Name,
                    factory: cell =>
                    {
                        var customer = (Organisation)this.Context.Session.Create(M.Organisation.Class);
                        customer.IsCustomer = true;
                        return customer;
                    });
            }

            this.Controls.Bind();
            await this.Sheet.Flush().ConfigureAwait(false);

            this.Sheet.AutoFit();
        }

        private async Task Update()
        {
            // TODO: Pulls for lookups
            // var pulls = new List<Pull>();
            // var result = await this.Context.Load(pulls.ToArray());

            this.Context.Session.Refresh();

            // TODO: Update results for lookups

            this.Controls.Bind();

            await this.Sheet.Flush().ConfigureAwait(false);
        }

        public async Task Save()
        {
            var response = await this.Context.Save();
            if (response.HasErrors)
            {
                if (response.accessErrors?.Any() == true || response.missingErrors?.Any() == true || response.versionErrors?.Any() == true)
                {
                    this.ErrorService.Handle(response, this.Context.Session);
                    this.MessageService.Show("Error was irrecoverable, sheet has been reset", "Info");
                    await this.Init();
                }
                else
                {
                    this.ErrorService.Handle(response, this.Context.Session);
                }
            }
            else
            {
                this.MessageService.Show("Successfully saved", "Info");
                this.Sheet.DeleteRows(this.StartRowNewItems, NewRowCount);
                await this.Init();
            }
        }

        public async Task Refresh()
        {
            if (!this.Context.Session.HasChanges)
            {
                await this.Init();
            }
            else
            {
                switch (this.MessageService.ShowDialog("Do you want to keep your changes", "Info"))
                {
                    case true:
                        await this.Update();
                        break;
                    case false:
                        await this.Init();
                        break;
                }
            }
        }

        private async void BinderOnToDomained(object sender, EventArgs e) => await this.Sheet.Flush().ConfigureAwait(false);
    }
}
