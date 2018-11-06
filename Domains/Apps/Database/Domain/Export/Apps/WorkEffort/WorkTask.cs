// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkTask.cs" company="Allors bvba">
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
    using System.Linq;
    using System.Collections.Generic;
    
    using Allors.Meta;
    using Allors.Services;
    using Sandwych.Reporting;
    using Microsoft.Extensions.DependencyInjection;

    public partial class WorkTask
    {
        public new string ToString() => this.WorkEffortNumber ?? this.Name;

        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            {
                new TransitionalConfiguration(M.WorkTask, M.WorkTask.WorkEffortState),
            };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        public void AppsOnBuild(ObjectOnBuild method)
        {
            var internalOrganisations = new Organisations(this.strategy.Session).Extent()
                .Where(v => Equals(v.IsInternalOrganisation, true)).ToArray();

            if (!this.ExistTakenBy && internalOrganisations.Count() == 1)
            {
                this.TakenBy = internalOrganisations.First();
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            if (!this.ExistWorkEffortNumber && this.ExistTakenBy)
            {
                this.WorkEffortNumber = this.TakenBy.NextWorkEffortNumber();
            }

            this.RenderPrintDocument(this.TakenBy?.WorkTaskTemplate, new WorkTaskSandwychPrintModel(this).Model);
        }

        //public void AppsDelete(DeletableDelete method)
        //{
        //    foreach (WorkEffortStatus workEffortStatus in this.WorkEffortStatuses)
        //    {
        //        workEffortStatus.Delete();
        //    }

        //    foreach (WorkEffortAssignment workEffortAssignment in this.WorkEffortAssignmentsWhereAssignment)
        //    {
        //        workEffortAssignment.Delete();
        //    }
        //}
    }

    class WorkTaskSandwychPrintModel
    {
        private class WorkTaskPrintModel
        {
            public string Number;
            public ImageBlob Barcode;
            public string Purpose;
            public string Date;
            public string PurchaseOrder;
            public string ContactName;
            public string ContactTelephone;
            public string PaymentTerms;
            public string Establishment;
            public string SalesRep;

            public WorkTaskPrintModel(WorkTask workTask)
            {
                this.Number = workTask.WorkEffortNumber;

                var session = workTask.Strategy.Session;
                var barcodeService = session.ServiceProvider.GetRequiredService<IBarcodeService>();
                var barcode = barcodeService.Generate(this.Number, BarcodeType.CODE_128, 230, 80);
                this.Barcode = new ImageBlob("png", barcode);

                this.Date = (workTask.ThroughDate() ?? DateTime.UtcNow).ToString("yyyy-MM-dd");
                this.Purpose = String.Empty;

                foreach (WorkEffortPurpose purpose in workTask.WorkEffortPurposes)
                {
                    this.Purpose = this.Purpose.Equals(string.Empty) ? purpose.Name : $", {purpose.Name}";
                }

                if (workTask.ExistOrderItemFulfillment)
                {
                    if (workTask.OrderItemFulfillment is SalesOrderItem salesOrderItem)
                    {
                        if (salesOrderItem.ExistSalesTerms)
                        {
                            this.PaymentTerms = salesOrderItem.SalesTerms.First.Description;
                        }

                        var salesOrder = salesOrderItem.SalesOrderWhereSalesOrderItem;
                        this.PurchaseOrder = salesOrder?.OrderNumber;
                        this.SalesRep = salesOrder?.TakenByContactPerson?.PartyName;

                        if ((this.PaymentTerms == null) && salesOrder.ExistSalesTerms)
                        {
                            this.PaymentTerms = salesOrder.SalesTerms.First.Description;
                        }
                    }

                    if (workTask.ExistContactPerson)
                    {
                        var contact = workTask.ContactPerson;
                        this.ContactName = contact.PartyName;
                        this.ContactTelephone = contact.CellPhoneNumber?.Description ?? contact.GeneralPhoneNumber?.Description;
                    }

                    if (workTask.ExistSpecialTerms)
                    {
                        this.PaymentTerms = workTask.SpecialTerms;
                    }

                    this.Establishment = workTask.Facility?.Name;
                }
            }
        }

        private class CustomerPrintModel
        {
            public string Number;
            public string BillingAddress1;
            public string BillingAddress2;
            public string BillingAddress3;
            public string BillingCity;
            public string BillingState;
            public string BillingPostalCode;
            public string ShippingAddress1;
            public string ShippingAddress2;
            public string ShippingAddress3;
            public string ShippingCity;
            public string ShippingState;
            public string ShippingPostalCode;

            public CustomerPrintModel(Party customer)
            {
                this.Number = customer.Id.ToString();

                if (customer.BillingAddress is PostalAddress billingAddress)
                {
                    this.BillingAddress1 = billingAddress.Address1;
                    this.BillingAddress2 = billingAddress.Address2;
                    this.BillingAddress3 = billingAddress.Address3;

                    if (billingAddress.ExistCity)
                    {
                        this.BillingCity = billingAddress.City.Name;
                        this.BillingState = billingAddress.City.State?.Name;
                    }

                    this.BillingPostalCode = billingAddress.PostalCode?.Code;
                }

                if (customer.ShippingAddress is PostalAddress shippingAddress)
                {
                    this.ShippingAddress1 = shippingAddress.Address1;
                    this.ShippingAddress2 = shippingAddress.Address2;
                    this.ShippingAddress3 = shippingAddress.Address3;

                    if (shippingAddress.ExistCity)
                    {
                        this.ShippingCity = shippingAddress.City.Name;
                        this.ShippingState = shippingAddress.City.State?.Name;
                    }

                    this.ShippingPostalCode = shippingAddress.PostalCode?.Code;
                }
            }
        }

        private class TimeEntryPrintModel
        {
            public decimal AmountOfTime;
            public decimal BillingRate;
            public decimal Cost;
            public string TimeFrequency;
            public string WorkerName;
            public string WorkerId;
            public string Description;
            public bool IsBillable;
            public string FromDate;
            public string FromTime;
            public string ThroughDate;
            public string ThroughTime;

            public TimeEntryPrintModel(TimeEntry timeEntry)
            {
                var frequency = timeEntry.TimeFrequency.Abbreviation ?? timeEntry.TimeFrequency.Name;

                this.AmountOfTime = Math.Round(timeEntry.AmountOfTime ?? 0.0m, 2);
                this.BillingRate = Math.Round(timeEntry.BillingRate ?? 0.0m, 2);
                this.Cost = Math.Round(timeEntry.Cost, 2);
                this.TimeFrequency = frequency.ToUpperInvariant();
                this.WorkerName = timeEntry.TimeSheetWhereTimeEntry?.Worker?.PartyName;
                this.WorkerId = timeEntry.TimeSheetWhereTimeEntry?.Worker.Id.ToString();
                this.Description = timeEntry.Description;
                this.IsBillable = timeEntry.IsBillable != false;
                this.FromDate = timeEntry.FromDate.ToString("yyyy-MM-dd");
                this.FromTime = timeEntry.FromDate.ToString("hh:mm:ss");
                this.ThroughDate = ((DateTime)timeEntry.ThroughDate).ToString("yyyy-MM-dd");
                this.ThroughTime = ((DateTime)timeEntry.ThroughDate).ToString("hh:mm:ss");
            }
        }

        private class WorkEffortInventoryAssignmentPrintModel
        {
            public string PartId;
            public string PartName;
            public string UnitOfMeasure;
            public int Quantity;

            public WorkEffortInventoryAssignmentPrintModel(WorkEffortInventoryAssignment assignment)
            {
                var unit = assignment.Part.UnitOfMeasure.Abbreviation ?? assignment.Part.UnitOfMeasure.Name;

                if (unit == null)
                {
                    unit = "EA";
                }
                else
                {
                    unit = unit.ToUpperInvariant();
                }

                this.PartId = assignment.Part.PartIdentification;
                this.PartName = assignment.Part.Name;
                this.UnitOfMeasure = assignment.Part.UnitOfMeasure?.Abbreviation?.ToUpperInvariant() ?? "EA";
                this.Quantity = assignment.Quantity;
            }
        }

        public Dictionary<string, object> Model { get; }

        public WorkTaskSandwychPrintModel(WorkTask workTask)
        {
            this.Model = new Dictionary<string, object> { { "workTask", new WorkTaskPrintModel(workTask) } };

            // Logo
            var singleton = workTask.Strategy.Session.GetSingleton();
            ImageBlob logo = new ImageBlob("png", singleton.LogoImage.MediaContent.Data);

            if (workTask.ExistTakenBy && workTask.TakenBy.ExistLogoImage)
            {
                logo = new ImageBlob("png", workTask.TakenBy.LogoImage.MediaContent.Data);
            }
            this.Model.Add("logo", logo);

            // Customer
            if (workTask.ExistCustomer)
            {
                this.Model.Add("customer", new CustomerPrintModel(workTask.Customer));
            }

            // Time Entries
            var timeEntries = new List<TimeEntryPrintModel>();

            foreach (ServiceEntry serviceEntry in workTask.ServiceEntriesWhereWorkEffort)
            {
                if (serviceEntry is TimeEntry timeEntry)
                {
                    timeEntries.Add(new TimeEntryPrintModel(timeEntry));
                }
            }

            if (timeEntries.Count() > 0)
            {
                this.Model.Add("time", timeEntries.ToArray());
            }

            // Work Effort Inventory Assignments
            var inventoryAssignments = new List<WorkEffortInventoryAssignmentPrintModel>();
                
            foreach (WorkEffortInventoryAssignment assignment in workTask.WorkEffortInventoryAssignmentsWhereAssignment)
            {
                inventoryAssignments.Add(new WorkEffortInventoryAssignmentPrintModel(assignment));
            }

            if (inventoryAssignments.Count() > 0)
            {
                this.Model.Add("inventory", inventoryAssignments);
            }
        }
    }
}
