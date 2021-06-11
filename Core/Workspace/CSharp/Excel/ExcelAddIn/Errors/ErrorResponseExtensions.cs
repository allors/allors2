
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
            if (error.accessErrors?.Length > 0)
            {
                MessageBox.Show(@"You do not have the required rights.", @"Access Error");
            }
            else if (error.derivationErrors?.Length > 0)
            {
                var message = new StringBuilder();
                foreach (var derivationError in error.derivationErrors)
                {
                    message.Append($" - {derivationError.m}\n");
                }

                MessageBox.Show(message.ToString(), @"Derivation Errors");
            }
            else if (error.versionErrors?.Length > 0 || error.missingErrors?.Length > 0)
            {
                MessageBox.Show(@"Modifications were detected since last access.", @"Concurrency Error");
            }
            else
            {
                MessageBox.Show($@"{error.errorMessage}", @"General Error");
            }
        }

        public static void ShowToaster(this Response @this, Form form)
        {
            if (@this.accessErrors?.Length > 0)
            {
                form.Invoke(new MethodInvoker(() => Toaster.NotAuthorizedToast()));
            }
            else if (@this.derivationErrors?.Length > 0)
            {
                form.Invoke(new MethodInvoker(() => Toaster.DerivationErrors(@this.derivationErrors)));
            }
            else if (@this.versionErrors?.Length > 0 || @this.missingErrors?.Length > 0)
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

            if (errorResponse.accessErrors?.Length > 0)
            {
                foreach (var error in errorResponse.accessErrors)
                {
                    logger.Error("Access error: " + Message(session, error));
                }
            }
            else if (errorResponse.versionErrors?.Length > 0)
            {
                foreach (var error in errorResponse.versionErrors)
                {
                    logger.Error("Version error: " + Message(session, error));
                }
            }
            else if (errorResponse.missingErrors?.Length > 0)
            {
                foreach (var error in errorResponse.missingErrors)
                {
                    logger.Error("Missing error: " + Message(session, error));
                }
            }
            else if (errorResponse.derivationErrors?.Length > 0)
            {
                foreach (var error in errorResponse.derivationErrors)
                {
                    logger.Error("Derivation error: " + error.m);
                }
            }
            else
            {
                logger.Error($@"{errorResponse.errorMessage}");
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
