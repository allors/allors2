
namespace ExcelAddIn
{
    using System.Text;
    using System.Windows.Forms;
    using Allors.Protocol.Remote;
    using Allors.Workspace;
    using ToastNotifications;
    using NLog;

    public static class ErrorResponseExtensions
    {
        public static void HandleErrors(this Response @this, ISession session)
        {
            if (@this.HasErrors)
            {
                @this.Log(session);
                @this.Show();
            }
        }

        public static void Show(this Response error)
        {
            if (error.AccessErrors?.Length > 0)
            {
                MessageBox.Show(@"You do not have the required rights.", @"Access Error");
            }
            else if (error.DerivationErrors?.Length > 0)
            {
                var message = new StringBuilder();
                foreach (var derivationError in error.DerivationErrors)
                {
                    message.Append($" - {derivationError.M}\n");
                }

                MessageBox.Show(message.ToString(), @"Derivation Errors");
            }
            else if (error.VersionErrors?.Length > 0 || error.MissingErrors?.Length > 0)
            {
                MessageBox.Show(@"Modifications were detected since last access.", @"Concurrency Error");
            }
            else
            {
                MessageBox.Show($@"{error.ErrorMessage}", @"General Error");
            }
        }

        public static void ShowToaster(this Response @this, Form form)
        {
            if (@this.AccessErrors?.Length > 0)
            {
                form.Invoke(new MethodInvoker(() => Toaster.NotAuthorizedToast()));
            }
            else if (@this.DerivationErrors?.Length > 0)
            {
                form.Invoke(new MethodInvoker(() => Toaster.DerivationErrors(@this.DerivationErrors)));
            }
            else if (@this.VersionErrors?.Length > 0 || @this.MissingErrors?.Length > 0)
            {
                form.Invoke(new MethodInvoker(() => Toaster.ConcurrencyDetectedToast()));
            }
            else
            {
                form.Invoke(new MethodInvoker(() => Toaster.GeneralError(@this)));
            }
        }

        public static void Log(this Response errorResponse, ISession session)
        {
            var logger = LogManager.GetCurrentClassLogger();

            if (errorResponse.AccessErrors?.Length > 0)
            {
                foreach (var error in errorResponse.AccessErrors)
                {
                    logger.Error("Access error: " + Message(session, error));
                }
            }
            else if (errorResponse.VersionErrors?.Length > 0)
            {
                foreach (var error in errorResponse.VersionErrors)
                {
                    logger.Error("Version error: " + Message(session, error));
                }
            }
            else if (errorResponse.MissingErrors?.Length > 0)
            {
                foreach (var error in errorResponse.MissingErrors)
                {
                    logger.Error("Missing error: " + Message(session, error));
                }
            }
            else if (errorResponse.DerivationErrors?.Length > 0)
            {
                foreach (var error in errorResponse.DerivationErrors)
                {
                    logger.Error("Derivation error: " + error.M);
                }
            }
            else
            {
                logger.Error($@"{errorResponse.ErrorMessage}");
            }
        }

        private static string Message(ISession session, string error)
        {
            try
            {
                long id;
                if (long.TryParse(error, out id))
                {
                    var @object = session.Get(id);
                    return @object.ToString();
                }

                return error;
            }
            catch
            {
                return error;
            }
        }
    }
}
