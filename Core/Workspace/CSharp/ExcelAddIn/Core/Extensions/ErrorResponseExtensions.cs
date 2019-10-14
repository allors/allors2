// <copyright file="ErrorResponseExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Excel
{
    using System.Text;
    using System.Windows.Forms;

    using Allors.Protocol.Remote;
    using Allors.Workspace;

    using NLog;

    public static class MessageBoxExtensions
    {
        public static void Show(this Response error)
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

        public static void Log(this Response response, Session session)
        {
            var logger = LogManager.GetCurrentClassLogger();

            if (response.AccessErrors?.Length > 0)
            {
                foreach (var error in response.AccessErrors)
                {
                    logger.Error("Access error: " + Message(session, error));
                }
            }
            else if (response.VersionErrors?.Length > 0)
            {
                foreach (var error in response.VersionErrors)
                {
                    logger.Error("Version error: " + Message(session, error));
                }
            }
            else if (response.MissingErrors?.Length > 0)
            {
                foreach (var error in response.MissingErrors)
                {
                    logger.Error("Missing error: " + Message(session, error));
                }
            }
            else if (response.DerivationErrors?.Length > 0)
            {
                foreach (var error in response.DerivationErrors)
                {
                    logger.Error("Derivation error: " + error.M);
                }
            }
            else
            {
                logger.Error($@"{response.ErrorMessage}");
            }
        }

        private static string Message(Session session, string error)
        {
            try
            {
                if (long.TryParse(error, out var id))
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
