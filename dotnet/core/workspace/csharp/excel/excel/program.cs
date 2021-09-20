namespace Application
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Excel;
    using Allors.Workspace;
    using Allors.Workspace.Data;
    using Allors.Workspace.Domain;
    using Allors.Workspace.Meta;
    using Sheets;
    using Result = Allors.Workspace.Data.Result;
    using Task = System.Threading.Tasks.Task;

    public class Program : IProgram
    {
        public Program(IServiceLocator serviceLocator, Client client)
        {
            this.ServiceLocator = serviceLocator;
            this.Client = client;
            this.Roles = new Roles();

            this.Workbooks = new List<IWorkbook>();
            this.Worksheets = new List<IWorksheet>();
            this.SheetByWorksheet = new ConcurrentDictionary<IWorksheet, ISheet>();
        }

        public IServiceLocator ServiceLocator { get; }

        public Client Client { get; }

        public IList<IWorkbook> Workbooks { get; }

        public IList<IWorksheet> Worksheets { get; }

        public IDictionary<IWorksheet, ISheet> SheetByWorksheet { get; }

        public Roles Roles { get; private set; }

        public IAddIn AddIn { get; private set; }

        public IWorkbook ActiveWorkbook => this.AddIn.Workbooks.FirstOrDefault(v => v.IsActive);

        public IWorksheet ActiveWorksheet => this.ActiveWorkbook.Worksheets.FirstOrDefault(v => v.IsActive);

        public async Task OnHandle(string handle, params object[] arguments)
        {
            if (this.ActiveWorkbook == null)
            {
                this.ServiceLocator.GetMessageService().Show("Please open a workbook", "Missing workbook");
                return;
            }

            switch (handle)
            {
                case Actions.Save:
                    await this.OnSave();
                    break;
                case Actions.Refresh:
                    await this.OnRefresh();
                    break;
                case Actions.Customers:
                    var customersSheet = new CustomersSheet(this);
                    this.SheetByWorksheet.Add(customersSheet.Sheet, customersSheet);
                    await customersSheet.Init();
                    break;
            }
        }

        public async Task OnLogin()
        {
            var context = new Context(this.Client.Database, this.Client.Workspace);

            var pulls = new[]
            {
                new Pull
                {
                    Extent = new Extent(M.Person.ObjectType)
                    {
                        Predicate = new Equals(M.Person.UserName){Value = this.Client.UserName}
                    },
                    Results = new[]
                    {
                        new Result
                        {
                            Fetch = new Fetch
                            {
                                Include = new PersonNodeBuilder(v =>
                                {
                                    v.UserGroupsWhereMember();
                                })
                            }
                        }
                    }
                }
            };

            var result = await context.Load(pulls);

            var person = result.GetCollection<Person>().FirstOrDefault();
            var groups = person?.UserGroupsWhereMember;

            this.Roles = new Roles
            {
                IsAdministrator = groups?.Any(v => v.UniqueId == Roles.AdministratorsId) == true
            };
        }

        public Task OnLogout()
        {
            this.Roles = new Roles();
            return Task.CompletedTask;
        }

        public bool IsEnabled(string controlId, string controlTag)
        {
            var isLoggedIn = this.Client.IsLoggedIn;

            return controlId switch
            {
                "save" => isLoggedIn,
                "refresh" => isLoggedIn,
                "customers" => isLoggedIn && this.Roles.IsAdministrator,
                _ => throw new Exception($"Unhandled control with id {controlId}")
            };
        }

        private async Task OnSave()
        {
            var activeWorksheet = this.ActiveWorksheet;

            if (activeWorksheet != null)
            {
                if (this.SheetByWorksheet.TryGetValue(activeWorksheet, out var sheet))
                {
                    if (sheet is ISaveable saveable)
                    {
                        await saveable.Save();
                    }
                }
            }
        }

        private async Task OnRefresh()
        {
            var activeWorksheet = this.ActiveWorksheet;

            if (activeWorksheet != null)
            {
                if (this.SheetByWorksheet.TryGetValue(activeWorksheet, out var sheet))
                {
                    if (sheet is ISaveable saveable)
                    {
                        await saveable.Refresh();
                    }
                }
            }
        }

        #region Application
        public Task OnStart(IAddIn addIn)
        {
            this.AddIn = addIn;
            return Task.CompletedTask;
        }

        public Task OnStop() => Task.CompletedTask;

        #endregion

        #region Workbook
        public Task OnNew(IWorkbook workbook)
        {
            this.Workbooks.Add(workbook);
            return Task.CompletedTask;
        }

        public void OnClose(IWorkbook workbook, ref bool cancel) => this.Workbooks.Remove(workbook);

        #endregion

        #region Worksheet
        public Task OnNew(IWorksheet worksheet)
        {
            this.Worksheets.Add(worksheet);
            return Task.CompletedTask;
        }

        public Task OnBeforeDelete(IWorksheet worksheet)
        {
            this.SheetByWorksheet.Remove(worksheet);
            this.Worksheets.Remove(worksheet);
            return Task.CompletedTask;
        }

        #endregion
    }
}
