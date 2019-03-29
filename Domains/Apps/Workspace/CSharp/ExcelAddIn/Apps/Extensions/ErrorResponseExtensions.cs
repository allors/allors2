namespace Allors.Excel
{
    using System.Text;
    using System.Windows.Forms;

    using Allors.Workspace;
    using Allors.Server;

    using NLog;

    public static class MessageBoxExtensions
    {
        public static void Show(this ErrorResponse error)
        {
            if (error.AccessErrors?.Length > 0)
            {
                MessageBox.Show(@"You do not have the required rights.", @"Access Error");
            }
            else if (error.VersionErrors?.Length > 0 || error.MissingErrors?.Length > 0)
            {
                MessageBox.Show(@"Modifications were detected since last access.", @"Concurrency Error");
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
            else
            {
                MessageBox.Show($@"{error.ErrorMessage}", @"General Error");
            }
        }

        public static void Log(this ErrorResponse errorResponse, Session session)
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

        private static string Message(Session session, string error)
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
