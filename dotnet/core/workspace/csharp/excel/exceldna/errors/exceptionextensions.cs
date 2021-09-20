namespace ExcelDNA
{
    using System;
    using System.Windows.Forms;

    using NLog;

    public static class ExceptionExtensions
    {
        public static void Handle(this Exception @this)
        {
            LogManager.GetCurrentClassLogger().Error(@this);
            MessageBox.Show(@"System error occurred. Please restart.");
        }

        public static void Show(this Exception @this)
        {
            LogManager.GetCurrentClassLogger().Error(@this);
            MessageBox.Show(@this.Message);
        }

        public static void ShowMessage(this Exception @this, string message)
        {
            LogManager.GetCurrentClassLogger().Error(@this);
            MessageBox.Show(message);
        }
    }
}
