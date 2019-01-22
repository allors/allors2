// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkTaskModel.cs" company="Allors bvba">
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

namespace Allors.Domain.Print.WorkTaskModel
{
    using System;
    using System.Linq;

    public class WorkTaskModel
    {
        public WorkTaskModel(WorkTask workTask)
        {
            this.Number = workTask.WorkEffortNumber;
            
            this.Date = (workTask.ThroughDate() ?? DateTime.UtcNow).ToString("yyyy-MM-dd");
            this.Purpose = string.Join(", ", workTask.WorkEffortPurposes.Select(v => v.Name));
            this.Facility = workTask.Facility?.Name;
            this.ContactName = workTask.ContactPerson?.PartyName;
            this.ContactTelephone = workTask.ContactPerson?.CellPhoneNumber?.Description ?? workTask.ContactPerson?.GeneralPhoneNumber?.Description;

            if (workTask.ExistOrderItemFulfillment)
            {
                if (workTask.OrderItemFulfillment is SalesOrderItem salesOrderItem)
                {
                    var salesOrder = salesOrderItem.SalesOrderWhereSalesOrderItem;
                    this.PurchaseOrder = salesOrder?.OrderNumber;

                    if (salesOrderItem.ExistSalesTerms)
                    {
                        this.PaymentTerms = string.Join(", ", salesOrderItem.SalesTerms.Select(v => v.Description));
                    }
                    else if (salesOrder?.ExistSalesTerms == true)
                    {
                        this.PaymentTerms = string.Join(", ", salesOrder.SalesTerms.Select(v => v.Description));
                    }
                    else if (workTask.Customer?.PaymentNetDays() != null)
                    {
                        this.PaymentTerms = workTask.Customer.PaymentNetDays().ToString();
                    }
                }
            }
        }

        public string Number { get; }
        public string Purpose { get; }
        public string Date { get; }
        public string PurchaseOrder { get; }
        public string ContactName { get; }
        public string ContactTelephone { get; }
        public string PaymentTerms { get; }
        public string Facility { get; }
        public string SalesRep { get; }
    }
}
