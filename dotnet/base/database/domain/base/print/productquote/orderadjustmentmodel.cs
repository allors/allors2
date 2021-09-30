// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QuoteItemModel.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain.Print.ProductQuoteModel
{
    using System.Globalization;

    public class OrderAdjustmentModel
    {
        public OrderAdjustmentModel(OrderAdjustment orderAdjustment)
        {
            var quote = orderAdjustment.QuoteWhereOrderAdjustment;

            this.AdjustmentTypeName = orderAdjustment.GetType().Name;
            this.Description = orderAdjustment.Description;

            if (orderAdjustment.GetType().Name.Equals(typeof(DiscountAdjustment).Name))
            {
                this.Amount = quote.TotalDiscount.ToString("N2", new CultureInfo("nl-BE"));
            }

            if (orderAdjustment.GetType().Name.Equals(typeof(SurchargeAdjustment).Name))
            {
                this.Amount = quote.TotalSurcharge.ToString("N2", new CultureInfo("nl-BE"));
            }

            if (orderAdjustment.GetType().Name.Equals(typeof(Fee).Name))
            {
                this.Amount = quote.TotalFee.ToString("N2", new CultureInfo("nl-BE"));
            }

            if (orderAdjustment.GetType().Name.Equals(typeof(ShippingAndHandlingCharge).Name))
            {
                this.Amount = quote.TotalShippingAndHandling.ToString("N2", new CultureInfo("nl-BE"));
            }

            if (orderAdjustment.GetType().Name.Equals(typeof(MiscellaneousCharge).Name))
            {
                var miscCharge = quote.TotalExtraCharge - quote.TotalFee - quote.TotalShippingAndHandling;
                this.Amount = miscCharge.ToString("N2", new CultureInfo("nl-BE"));
            }
        }

        public string AdjustmentTypeName { get; set; }

        public string Amount { get; set; }

        public string Description { get; }
    }
}
