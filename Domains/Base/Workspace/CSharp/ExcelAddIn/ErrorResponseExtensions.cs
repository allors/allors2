namespace Allors.Excel
{
    using System.Text;
    using System.Windows.Forms;

    using Allors.Workspace;
    using Allors.Workspace.Data;

    using NLog;

    public static class MessageBoxExtensions
    {
        public static void Show(this ErrorResponse error)
        {
            if (error.accessErrors?.Length > 0)
            {
                MessageBox.Show(@"You do not have the required rights.", @"Access Error");
            }
            else if (error.versionErrors?.Length > 0 || error.missingErrors?.Length > 0)
            {
                MessageBox.Show(@"Modifications were detected since last access.", @"Concurrency Error");
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
            else
            {
                MessageBox.Show($@"{error.errorMessage}", @"General Error");
            }
        }

        public static void Log(this ErrorResponse errorResponse, Session session)
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
