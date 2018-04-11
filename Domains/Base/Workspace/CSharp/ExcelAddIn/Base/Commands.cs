namespace Allors.Excel
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using NLog;

    public partial class Commands
    {
        public Commands(Sheets sheets)
        {
            this.Sheets = sheets;
        }

        public Sheets Sheets { get; }

        public bool CanSave => this.Sheets.ActiveSheet != null;

        public bool CanRefresh => this.Sheets.ActiveSheet != null;

        public bool CanNew => this.Sheets.ActiveSheet == null;

        protected Logger Logger { get; } = LogManager.GetCurrentClassLogger();

        public async Task Save()
        {
            try
            {
                var sheet = this.Sheets.ActiveSheet;
                if (sheet != null)
                {
                    if (!(await sheet.Save()).HasErrors)
                    {
                        await sheet.Refresh();
                    };
                }
            }
            catch (Exception e)
            {
                e.Handle();
            }
        }

        public async Task Refresh()
        {
            try
            {
                var sheet = this.Sheets.ActiveSheet;
                if (sheet != null)
                {
                    await sheet.Refresh();
                }
            }
            catch (Exception e)
            {
                e.Handle();
            }
        }
    }
}
