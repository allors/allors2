namespace Allors.Excel
{
    using System;
    using System.Windows.Forms;

    using Nito.AsyncEx;

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

        public void Save()
        {
            try
            {
                AsyncContext.Run(
                    async () =>
                    {
                        var sheet = this.Sheets.ActiveSheet;
                        if (sheet != null)
                        {
                            if (await sheet.Save())
                            {
                                await sheet.Refresh();
                            };
                        }
                    });
            }
            catch (Exception e)
            {
                this.OnError(e);
            }
        }

        public void Refresh()
        {
            try
            {
                AsyncContext.Run(
                    async () =>
                    {
                        var sheet = this.Sheets.ActiveSheet;
                        if (sheet != null)
                        {
                            await sheet.Refresh();
                        }
                    });
            }
            catch (Exception e)
            {
                this.OnError(e);
            }
        }
    }
}
