namespace Application
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Allors.Excel;
    using Application.Sheets;
    using Dipu.Excel;
    using Microsoft.Extensions.DependencyInjection;

    public class Program : IProgram
    {
        public Program(ServiceProvider serviceProvider, Client client)
        {
            this.ServiceProvider = serviceProvider;
            this.Client = client;
        }

        public ServiceProvider ServiceProvider { get; }

        public Client Client { get; }

        public IAddIn AddIn { get; private set; }

        public IList<IWorkbook> Workbooks { get; } = new List<IWorkbook>();

        public IList<IWorksheet> Worksheets { get; } = new List<IWorksheet>();

        public IWorkbook ActiveWorkbook => this.AddIn.Workbooks.FirstOrDefault(v => v.Active);

        public async Task OnHandle(string handle)
        {
            switch (handle)
            {
                case Actions.People:
                    await new PeopleSheet(this).Load();
                    break;
            }
        }

        #region Application
        public async Task OnStart(IAddIn addIn)
        {
            this.AddIn = addIn;
        }

        public async Task OnStop()
        {
        }
        #endregion

        #region Workbook
        public async Task OnNew(IWorkbook workbook)
        {
            this.Workbooks.Add(workbook);
        }

        public void OnClose(IWorkbook workbook, ref bool cancel)
        {
            this.Workbooks.Remove(workbook);
        }
        #endregion

        #region Worksheet
        public async Task OnNew(IWorksheet worksheet)
        {
            this.Worksheets.Add(worksheet);
        }

        public async Task OnBeforeDelete(IWorksheet worksheet)
        {
            this.Worksheets.Remove(worksheet);
        }

        #endregion
    }
}
