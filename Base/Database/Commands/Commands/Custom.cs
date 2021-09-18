// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Custom.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
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
    using Allors.Meta;
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
            //return this.MetaStatistics();
            //return this.CheckSecurity();
            //return this.PrintPurchaseInvoice();
            //return this.PrintSalesInvoice();
            return this.PrintProductQuote();
            //return this.PrintWorkTask();
            //return this.MonthlyScheduler();
        }

        private int MetaStatistics()
        {
            var metaPopulation = this.databaseService.Database.MetaPopulation;
            Console.WriteLine("Workspace ObjectTypes: " + metaPopulation.Composites.Cast<Composite>().Count(v => v.Workspace));

            return 0;
        }

        private int CheckSecurity()
        {
            using (var session = this.databaseService.Database.CreateSession())
            {
                var people = new People(session);

                var jane = people.FindBy(M.Person.FirstName, "jane");
                var john = people.FindBy(M.Person.FirstName, "john");

                var acls = new DatabaseAccessControlLists(jane);
                var acl = acls[john];

                var accessControl = acl.AccessControls.Single();

                var effectivePermissions = accessControl.EffectivePermissions;
                var personPermissions = effectivePermissions.Where(v => v.ConcreteClass == M.Person.Class).ToArray();

                var workspacePersonPermissions = personPermissions.Where(w => w.OperandType.Workspace).ToArray();

                //var canRead = acl.CanRead(M.Person.Salutation);
                //var canWrite = acl.CanRead(M.Person.Salutation);
            }

            return 0;
        }

        private int PrintPurchaseInvoice()
        {
            using (var session = this.databaseService.Database.CreateSession())
            {
                this.logger.LogInformation("Begin");

                var scheduler = new AutomatedAgents(session).System;
                session.SetUser(scheduler);

                var templateFilePath = "domain/templates/PurchaseInvoice.odt";
                var templateFileInfo = new FileInfo(templateFilePath);
                var prefix = string.Empty;
                while (!templateFileInfo.Exists)
                {
                    prefix += "../";
                    templateFileInfo = new FileInfo(prefix + templateFilePath);
                }

                var invoice = new PurchaseInvoices(session).Extent().Last();
                var template = invoice.BilledTo.PurchaseInvoiceTemplate;

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

                var printModel = new Allors.Domain.Print.PurchaseInvoiceModel.Model(invoice);
                invoice.RenderPrintDocument(template, printModel, images);

                session.Derive();

                var result = invoice.PrintDocument;

                var desktopDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                var outputFile = File.Create(Path.Combine(desktopDir, "purchaseInvoice.odt"));
                using (var stream = new MemoryStream(result.Media.MediaContent.Data))
                {
                    stream.CopyTo(outputFile);
                }

                this.logger.LogInformation("End");
            }

            return ExitCode.Success;
        }

        private int PrintSalesInvoice()
        {
            using (var session = this.databaseService.Database.CreateSession())
            {
                this.logger.LogInformation("Begin");

                var scheduler = new AutomatedAgents(session).System;
                session.SetUser(scheduler);

                var templateFilePath = "domain/templates/SalesInvoice.odt";
                var templateFileInfo = new FileInfo(templateFilePath);
                var prefix = string.Empty;
                while (!templateFileInfo.Exists)
                {
                    prefix += "../";
                    templateFileInfo = new FileInfo(prefix + templateFilePath);
                }

                var invoice = new SalesInvoices(session).Extent().Last();
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

        private int PrintProductQuote()
        {
            using (var session = this.databaseService.Database.CreateSession())
            {
                this.logger.LogInformation("Begin");

                var scheduler = new AutomatedAgents(session).System;
                session.SetUser(scheduler);

                var templateFilePath = "domain/templates/ProductQuote.odt";
                var templateFileInfo = new FileInfo(templateFilePath);
                var prefix = string.Empty;
                while (!templateFileInfo.Exists)
                {
                    prefix += "../";
                    templateFileInfo = new FileInfo(prefix + templateFilePath);
                }

                var quote = new ProductQuotes(session).Extent().Last();
                var template = quote.Issuer.ProductQuoteTemplate;

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

                if (quote.ExistQuoteNumber)
                {
                    var barcodeService = session.ServiceProvider.GetRequiredService<IBarcodeService>();
                    var barcode = barcodeService.Generate(quote.QuoteNumber, BarcodeType.CODE_128, 320, 80);
                    images.Add("Barcode", barcode);
                }

                var printModel = new Allors.Domain.Print.ProductQuoteModel.Model(quote, images);
                quote.RenderPrintDocument(template, printModel, images);

                session.Derive();

                var result = quote.PrintDocument;

                var desktopDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                var outputFile = File.Create(Path.Combine(desktopDir, "quote.odt"));
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

                var scheduler = new AutomatedAgents(session).System;
                session.SetUser(scheduler);

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

        private int MonthlyScheduler()
        {
            using (var session = this.databaseService.Database.CreateSession())
            {
                this.logger.LogInformation("Begin");

                var scheduler = new AutomatedAgents(session).System;
                session.SetUser(scheduler);

                WorkTasks.BaseMonthly(session);

                var validation = session.Derive(false);

                if (validation.HasErrors)
                {
                    foreach (var error in validation.Errors)
                    {
                        this.logger.LogError("Validation error: {error}", error);
                    }

                    session.Rollback();
                }
                else
                {
                    session.Commit();
                }

                session.Derive();
                session.Commit();

                this.logger.LogInformation("End");
            }

            return ExitCode.Success;
        }
    }
}
