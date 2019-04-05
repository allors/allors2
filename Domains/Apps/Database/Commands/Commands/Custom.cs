// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Custom.cs" company="Allors bvba">
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
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Allors;
    using Allors.Domain;
    using Allors.Services;

    using McMaster.Extensions.CommandLineUtils;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    [Command(Description = "Execute custom code")]
    public class Custom
    {
        private readonly IDatabaseService databaseService;

        private readonly ILogger<Custom> logger;

        public Custom(IDatabaseService databaseService, ILogger<Custom> logger)
        {
            this.databaseService = databaseService;
            this.logger = logger;
        }

        private Commands Parent { get; set; }

        public int OnExecute(CommandLineApplication app)
        {
            //return this.PrintSalesInvoice();
            return this.PrintWorkTask();
        }

        private int PrintSalesInvoice()
        {
            using (var session = this.databaseService.Database.CreateSession())
            {
                this.logger.LogInformation("Begin");

                var administrator = new Users(session).GetUser("administrator");
                session.SetUser(administrator);

                var templateFilePath = "domain/templates/SalesInvoice.odt";
                var templateFileInfo = new FileInfo(templateFilePath);
                var prefix = string.Empty;
                while (!templateFileInfo.Exists)
                {
                    prefix += "../";
                    templateFileInfo = new FileInfo(prefix + templateFilePath);
                }

                var invoice = new SalesInvoices(session).Extent().First;
                var template = invoice.BilledFrom.SalesInvoiceTemplate;

                using (var memoryStream = new MemoryStream())
                using (var fileStream = new FileStream(
                    templateFileInfo.FullName,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.ReadWrite))
                {
                    fileStream.CopyTo(memoryStream);
                    template.Media.InData = memoryStream.ToArray();
                }

                session.Derive();

                var images = new Dictionary<string, byte[]> { { "Logo", session.GetSingleton().LogoImage.MediaContent.Data }, };

                if (invoice.ExistInvoiceNumber)
                {
                    var barcodeService = session.ServiceProvider.GetRequiredService<IBarcodeService>();
                    var barcode = barcodeService.Generate(invoice.InvoiceNumber, BarcodeType.CODE_128, 320, 80);
                    images.Add("Barcode", barcode);
                }

                var printModel = new Allors.Domain.Print.SalesInvoiceModel.Model(invoice);
                invoice.RenderPrintDocument(template, printModel, images);

                session.Derive();

                var result = invoice.PrintDocument;

                var desktopDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                var outputFile = File.Create(Path.Combine(desktopDir, "salesInvoice.odt"));
                using (var stream = new MemoryStream(result.Media.MediaContent.Data))
                {
                    stream.CopyTo(outputFile);
                }

                this.logger.LogInformation("End");
            }

            return ExitCode.Success;
        }

        private int PrintWorkTask()
        {
            using (var session = this.databaseService.Database.CreateSession())
            {
                this.logger.LogInformation("Begin");

                var administrator = new Users(session).GetUser("administrator");
                session.SetUser(administrator);

                var templateFilePath = "domain/templates/WorkTask.odt";
                var templateFileInfo = new FileInfo(templateFilePath);
                var prefix = string.Empty;
                while (!templateFileInfo.Exists)
                {
                    prefix += "../";
                    templateFileInfo = new FileInfo(prefix + templateFilePath);
                }

                var workTasks = new WorkTasks(session).Extent();
                var workTask = workTasks.First(v => v.Name.Equals("maintenance"));
                var template = workTask.TakenBy.WorkTaskTemplate;

                using (var memoryStream = new MemoryStream())
                using (var fileStream = new FileStream(
                    templateFileInfo.FullName,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.ReadWrite))
                {
                    fileStream.CopyTo(memoryStream);
                    template.Media.InData = memoryStream.ToArray();
                }

                session.Derive();

                var images = new Dictionary<string, byte[]> { { "Logo", session.GetSingleton().LogoImage.MediaContent.Data }, };

                if (workTask.ExistWorkEffortNumber)
                {
                    var barcodeService = session.ServiceProvider.GetRequiredService<IBarcodeService>();
                    var barcode = barcodeService.Generate(workTask.WorkEffortNumber, BarcodeType.CODE_128, 320, 80);
                    images.Add("Barcode", barcode);
                }

                var printModel = new Allors.Domain.Print.WorkTaskModel.Model(workTask);
                workTask.RenderPrintDocument(template, printModel, images);

                session.Derive();

                var result = workTask.PrintDocument;

                var desktopDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                var outputFile = File.Create(Path.Combine(desktopDir, "workTask.odt"));
                using (var stream = new MemoryStream(result.Media.MediaContent.Data))
                {
                    stream.CopyTo(outputFile);
                }

                this.logger.LogInformation("End");
            }

            return ExitCode.Success;
        }
    }
}