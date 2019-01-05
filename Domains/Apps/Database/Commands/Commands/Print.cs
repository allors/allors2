// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Print.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Commands
{
    using System;
    using System.IO;

    using Allors;
    using Allors.Domain;
    using Allors.Services;

    using McMaster.Extensions.CommandLineUtils;

    using Microsoft.Extensions.Logging;

    [Command(Description = "Print")]
    public class Print
    {
        private readonly IDatabaseService databaseService;

        private readonly ILogger<Custom> logger;

        public Print(IDatabaseService databaseService, ILogger<Custom> logger)
        {
            this.databaseService = databaseService;
            this.logger = logger;
        }

        private Commands Parent { get; set; }

        public int OnExecute(CommandLineApplication app)
        {
            using (var session = this.databaseService.Database.CreateSession())
            {
                this.logger.LogInformation("Begin");

                var administrator = new Users(session).GetUser("administrator");
                session.SetUser(administrator);

                var templateFilePath = "domain/templates/ProductQuote.odt";
                var templateFileInfo = new FileInfo(templateFilePath);
                var prefix = string.Empty;
                while (!templateFileInfo.Exists)
                {
                    prefix += "../";
                    templateFileInfo = new FileInfo(prefix + templateFilePath);
                }

                var quote = new ProductQuotes(session).Extent().First;
                var template = quote.Issuer.ProductQuoteTemplate;

                using (var memoryStream = new MemoryStream())
                using (var fileStream = new FileStream(templateFileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    fileStream.CopyTo(memoryStream);
                    template.Media.InData = memoryStream.ToArray();
                }

                session.Derive();

                var printModel = new Allors.Domain.ProductQuotePrint.Model(quote);
                quote.RenderPrintDocument(template, printModel);

                session.Derive();

                var result = quote.PrintDocument;

                var desktopDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                var outputFile = File.Create(Path.Combine(desktopDir, "productQuote.odt"));
                using (var stream = new MemoryStream(result.MediaContent.Data))
                {
                    stream.CopyTo(outputFile);
                }

                this.logger.LogInformation("End");
            }

            return ExitCode.Success;
        }
    }
}