namespace Allors.Excel
{
    using System;
    using System.Windows.Forms;

    using NLog;

    public static class ExceptionExtensions
    {
        public static void Handle(this Exception @this)
        {
            var logger = LogManager.GetCurrentClassLogger();
            logger.Error(@this);
            MessageBox.Show(@"System error occured. Please restart.");
        }
    }
}
