
// <copyright file="Print.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Commands
{
    using System;

    using Allors;
    using Allors.Domain;
    using Allors.Meta;
    using Allors.Services;

    using McMaster.Extensions.CommandLineUtils;

    using Microsoft.Extensions.Logging;

    [Command(Description = "Print Documents")]
    public class Print
    {
        private readonly IDatabaseService databaseService;

        private readonly ILogger<Print> logger;

        public Print(IDatabaseService databaseService, ILogger<Print> logger)
        {
            this.databaseService = databaseService;
            this.logger = logger;
        }

        public int OnExecute(CommandLineApplication app)
        {
            var exitCode = ExitCode.Success;

            using (var session = this.databaseService.Database.CreateSession())
            {
                this.logger.LogInformation("Begin");

                var administrator = new Users(session).GetUser("administrator");
                session.SetUser(administrator);

                var printDocuments = new PrintDocuments(session).Extent();
                printDocuments.Filter.AddNot().AddExists(M.PrintDocument.Media);

                foreach (PrintDocument printDocument in printDocuments)
                {
                    var printable = printDocument.PrintableWherePrintDocument;
                    if (printable == null)
                    {
                        this.logger.LogWarning($"PrintDocument with id {printDocument.Id} has no Printable object");
                        continue;
                    }

                    try
                    {
                        printable.Print();

                        if (printable.ExistPrintDocument)
                        {
                            this.logger.LogInformation($"Printed {printable.PrintDocument.Media.FileName}");
                        }

                        session.Derive();
                        session.Commit();
                    }
                    catch (Exception e)
                    {
                        session.Rollback();
                        exitCode = ExitCode.Error;

                        this.logger.LogError(e, $"Could not print {printable.ToString()}");
                    }
                }

                this.logger.LogInformation("End");
            }

            return exitCode;
        }
    }
}
