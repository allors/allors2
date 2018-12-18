// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InternalOrganisationExtensions.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Allors.Meta;

    public static partial class InternalOrganisationExtensions
    {
        public static InventoryStrategy GetInventoryStrategy(this InternalOrganisation @this)
            => @this.InventoryStrategy ?? new InventoryStrategies(@this.Strategy.Session).Standard;

        public static void AppsOnBuild(this InternalOrganisation @this, ObjectOnBuild method)
        {
            if (!@this.ExistProductQuoteTemplate)
            {
                @this.ProductQuoteTemplate = @this.CreateOpenDocumentTemplate(@this.GetResourceBytes("Templates.ProductQuote.odt"));
            }

            if (!@this.ExistSalesOrderTemplate)
            {
                @this.SalesOrderTemplate = @this.CreateOpenDocumentTemplate(@this.GetResourceBytes("Templates.SalesOrder.odt"));
            }

            if (!@this.ExistSalesInvoiceTemplate)
            {
                @this.SalesInvoiceTemplate = @this.CreateOpenDocumentTemplate(@this.GetResourceBytes("Templates.SalesInvoice.odt"));
            }

            if (!@this.ExistWorkTaskTemplate)
            {
                @this.WorkTaskTemplate = @this.CreateOpenDocumentTemplate(@this.GetResourceBytes("Templates.WorkTask.odt"));
            }
        }

        public static void AppsStartNewFiscalYear(this InternalOrganisation @this, InternalOrganisationStartNewFiscalYear method)
        {
            var organisation = (Organisation)@this;
            if (organisation.IsInternalOrganisation)
            {
                if (@this.ExistActualAccountingPeriod && @this.ActualAccountingPeriod.Active)
                {
                    return;
                }

                var year = @this.Strategy.Session.Now().Year;
                if (@this.ExistActualAccountingPeriod)
                {
                    year = @this.ActualAccountingPeriod.FromDate.Date.Year + 1;
                }

                var fromDate = DateTimeFactory
                    .CreateDate(year, @this.FiscalYearStartMonth.Value, @this.FiscalYearStartDay.Value).Date;

                var yearPeriod = new AccountingPeriodBuilder(@this.Strategy.Session)
                    .WithPeriodNumber(1)
                    .WithTimeFrequency(new TimeFrequencies(@this.Strategy.Session).Year)
                    .WithFromDate(fromDate)
                    .WithThroughDate(fromDate.AddYears(1).AddSeconds(-1).Date)
                    .Build();

                var semesterPeriod = new AccountingPeriodBuilder(@this.Strategy.Session)
                    .WithPeriodNumber(1)
                    .WithTimeFrequency(new TimeFrequencies(@this.Strategy.Session).Semester)
                    .WithFromDate(fromDate)
                    .WithThroughDate(fromDate.AddMonths(6).AddSeconds(-1).Date)
                    .WithParent(yearPeriod)
                    .Build();

                var trimesterPeriod = new AccountingPeriodBuilder(@this.Strategy.Session)
                    .WithPeriodNumber(1)
                    .WithTimeFrequency(new TimeFrequencies(@this.Strategy.Session).Trimester)
                    .WithFromDate(fromDate)
                    .WithThroughDate(fromDate.AddMonths(3).AddSeconds(-1).Date)
                    .WithParent(semesterPeriod)
                    .Build();

                var monthPeriod = new AccountingPeriodBuilder(@this.Strategy.Session)
                    .WithPeriodNumber(1)
                    .WithTimeFrequency(new TimeFrequencies(@this.Strategy.Session).Month)
                    .WithFromDate(fromDate)
                    .WithThroughDate(fromDate.AddMonths(1).AddSeconds(-1).Date)
                    .WithParent(trimesterPeriod)
                    .Build();

                @this.ActualAccountingPeriod = monthPeriod;
            }
        }

        public static void AppsOnPreDerive(this InternalOrganisation @this, ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (derivation.HasChangedRole(@this, M.InternalOrganisation.DoAccounting))
            {
                foreach (PaymentMethod collectionMethod in @this.ActiveCollectionMethods)
                {
                    derivation.AddDependency(collectionMethod, @this);
                }

                foreach (PaymentMethod paymentMethod in @this.PaymentMethods)
                {
                    derivation.AddDependency(paymentMethod, @this);
                }
            }
        }

        public static void AppsOnDerive(this InternalOrganisation @this, ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            var organisation = (Organisation)@this;
            if (organisation.IsInternalOrganisation)
            {
                if (!@this.ExistDefaultCollectionMethod && @this.Strategy.Session.Extent<PaymentMethod>().Count == 1)
                {
                    @this.DefaultCollectionMethod = @this.Strategy.Session.Extent<PaymentMethod>().First;
                }

                if (!@this.ExistPurchaseInvoiceCounter)
                {
                    @this.PurchaseInvoiceCounter = new CounterBuilder(@this.Strategy.Session)
                        .WithUniqueId(Guid.NewGuid()).WithValue(0).Build();
                }

                if (!@this.ExistRequestCounter)
                {
                    @this.RequestCounter = new CounterBuilder(@this.Strategy.Session).WithUniqueId(Guid.NewGuid())
                        .WithValue(0).Build();
                }

                if (!@this.ExistQuoteCounter)
                {
                    @this.QuoteCounter = new CounterBuilder(@this.Strategy.Session).WithUniqueId(Guid.NewGuid())
                        .WithValue(0).Build();
                }

                if (!@this.ExistPurchaseOrderCounter)
                {
                    @this.PurchaseOrderCounter = new CounterBuilder(@this.Strategy.Session).WithUniqueId(Guid.NewGuid())
                        .WithValue(0).Build();
                }

                if (!@this.ExistIncomingShipmentCounter)
                {
                    @this.IncomingShipmentCounter = new CounterBuilder(@this.Strategy.Session)
                        .WithUniqueId(Guid.NewGuid()).WithValue(0).Build();
                }

                if (!@this.ExistSubAccountCounter)
                {
                    @this.SubAccountCounter = new CounterBuilder(@this.Strategy.Session).WithUniqueId(Guid.NewGuid())
                        .WithValue(0).Build();
                }

                if (!@this.ExistInvoiceSequence)
                {
                    @this.InvoiceSequence = new InvoiceSequences(@this.Strategy.Session).RestartOnFiscalYear;
                }

                if (@this.UsePartNumberCounter && !@this.ExistPartNumberCounter)
                {
                    @this.PartNumberCounter = new CounterBuilder(@this.Strategy.Session).WithUniqueId(Guid.NewGuid())
                        .WithValue(0).Build();
                }

                if (!@this.ExistFiscalYearStartMonth)
                {
                    @this.FiscalYearStartMonth = 1;
                }

                if (!@this.ExistFiscalYearStartDay)
                {
                    @this.FiscalYearStartDay = 1;
                }
            }
        }

        public static int NextSubAccountNumber(this InternalOrganisation @this)
        {
            var next = @this.SubAccountCounter.NextElfProefValue();
            return next;
        }

        public static string NextPurchaseInvoiceNumber(this InternalOrganisation @this)
        {
            var purchaseInvoiceNumber = @this.PurchaseInvoiceCounter.NextValue();
            return string.Concat(@this.ExistPurchaseInvoiceNumberPrefix ? @this.PurchaseInvoiceNumberPrefix: string.Empty, purchaseInvoiceNumber);
        }

        public static string NextQuoteNumber(this InternalOrganisation @this)
        {
            var quoteNumber = @this.QuoteCounter.NextValue();
            return string.Concat(@this.ExistQuoteNumberPrefix ? @this.QuoteNumberPrefix : string.Empty, quoteNumber);
        }

        public static string NextRequestNumber(this InternalOrganisation @this)
        {
            var requestNumber = @this.RequestCounter.NextValue();
            return string.Concat(@this.ExistRequestNumberPrefix ? @this.RequestNumberPrefix : string.Empty, requestNumber);
        }

        public static string NextShipmentNumber(this InternalOrganisation @this)
        {
            var shipmentNumber = @this.IncomingShipmentCounter.NextValue();
            return string.Concat(@this.ExistIncomingShipmentNumberPrefix ? @this.IncomingShipmentNumberPrefix : string.Empty, shipmentNumber);
        }

        public static string NextPurchaseOrderNumber(this InternalOrganisation @this)
        {
            var purchaseOrderNumber = @this.PurchaseOrderCounter.NextValue();
            return string.Concat(@this.ExistPurchaseOrderNumberPrefix? @this.PurchaseOrderNumberPrefix : string.Empty, purchaseOrderNumber);
        }

        public static string NextPartNumber(this InternalOrganisation @this)
        {
            var partNumber = @this.PartNumberCounter.NextValue();
            return string.Concat(@this.ExistPartNumberPrefix ? @this.PartNumberPrefix : string.Empty, partNumber);
        }

        public static string NextWorkEffortNumber(this InternalOrganisation @this)
            => string.Concat(@this.WorkEffortPrefix, @this.WorkEffortCounter.NextValue());

        public static int NextValidElevenTestNumer(int previous)
        {
            var candidate = previous.ToString();
            var valid = false;

            while (!valid)
            {
                candidate = previous.ToString();
                var sum = 0;
                for (var i = candidate.Length; i > 0; i--)
                {
                    sum += int.Parse(candidate.Substring(candidate.Length - i, 1)) * i;
                }

                valid = sum % 11 == 0;

                if (!valid)
                {
                    previous++;
                }
            }

            return int.Parse(candidate);
        }

        private static Template CreateOpenDocumentTemplate(this InternalOrganisation @this, byte[] content)
        {
            var media = new MediaBuilder(@this.Strategy.Session).WithInData(content).Build();
            var templateType = new TemplateTypes(@this.Strategy.Session).OpenDocumentType;
            var template = new TemplateBuilder(@this.Strategy.Session).WithMedia(media).WithTemplateType(templateType).Build();
            return template;
        }

        private static byte[] GetResourceBytes(this InternalOrganisation @this, string name)
        {
            var assembly = @this.GetType().GetTypeInfo().Assembly;
            var manifestResourceName = assembly.GetManifestResourceNames().First(v => v.Contains(name));
            var resource = assembly.GetManifestResourceStream(manifestResourceName);
            if (resource != null)
            {
                using (var ms = new MemoryStream())
                {
                    resource.CopyTo(ms);
                    return ms.ToArray();
                }
            }

            return null;
        }
    }
}