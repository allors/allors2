// <copyright file="QuoteExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using System.Linq;

    public static partial class QuoteExtensions
    {
        public static void BaseOnBuild(this Quote @this, ObjectOnBuild method)
        {
            if (!@this.ExistQuoteState)
            {
                @this.QuoteState = new QuoteStates(@this.Strategy.Session).Created;
            }
        }

        public static void BaseOnDerive(this Quote @this, ObjectOnDerive method)
        {
            var session = @this.Strategy.Session;
            var derivation = method.Derivation;

            if (!@this.ExistIssuer)
            {
                var internalOrganisations = new Organisations(session).InternalOrganisations();

                if (internalOrganisations.Count() == 1)
                {
                    @this.Issuer = internalOrganisations.First();
                }
            }

            if (!@this.ExistQuoteNumber && @this.ExistIssuer)
            {
                @this.QuoteNumber = @this.Issuer.NextQuoteNumber(session.Now().Year);
            }

            @this.Currency ??= @this.Receiver?.PreferredCurrency ?? @this.Issuer?.PreferredCurrency;

            foreach (QuoteItem quoteItem in @this.QuoteItems)
            {
                var quoteItemDerivedRoles = (QuoteItemDerivedRoles)quoteItem;

                quoteItemDerivedRoles.VatRegime = quoteItem.AssignedVatRegime ?? @this.VatRegime;
                quoteItemDerivedRoles.VatRate = quoteItem.VatRegime?.VatRate ?? quoteItem.Product?.VatRate;
            }

            @this.AddSecurityToken(new SecurityTokens(session).DefaultSecurityToken);

            @this.Sync(derivation);
        }

        public static void BaseDelete(this Quote @this, DeletableDelete method)
        {
            var productQuote = @this as ProductQuote;
            var propasal = @this as Proposal;
            var statementOfWork = @this as StatementOfWork;

            if ((productQuote != null && productQuote.IsDeletable)
                || (propasal != null && propasal.IsDeletable)
                || (statementOfWork != null && statementOfWork.IsDeletable))
            {
                if (@this.ExistShippingAndHandlingCharge)
                {
                    @this.ShippingAndHandlingCharge.Delete();
                }

                if (@this.ExistFee)
                {
                    @this.Fee.Delete();
                }

                if (@this.ExistDiscountAdjustment)
                {
                    @this.DiscountAdjustment.Delete();
                }

                if (@this.ExistSurchargeAdjustment)
                {
                    @this.SurchargeAdjustment.Delete();
                }

                foreach (QuoteItem item in @this.QuoteItems)
                {
                    item.Delete();
                }
            }
        }

        public static void BaseApprove(this Quote @this, QuoteApprove method)
        {
            @this.QuoteState = new QuoteStates(@this.Strategy.Session).InProcess;
            SetItemState(@this);
        }

        public static void BaseSend(this Quote @this, QuoteSend method)
        {
            @this.QuoteState = new QuoteStates(@this.Strategy.Session).AwaitingAcceptance;
            SetItemState(@this);
        }

        public static void BaseAccept(this Quote @this, QuoteAccept method)
        {
            @this.QuoteState = new QuoteStates(@this.Strategy.Session).Accepted;
            SetItemState(@this);
        }

        public static void BaseRevise(this Quote @this, QuoteRevise method)
        {
            @this.QuoteState = new QuoteStates(@this.Strategy.Session).Created;
            SetItemState(@this);
        }

        public static void BaseReopen(this Quote @this, QuoteReopen method)
        {
            @this.QuoteState = new QuoteStates(@this.Strategy.Session).Created;
            SetItemState(@this);
        }

        public static void BaseReject(this Quote @this, QuoteReject method)
        {
            @this.QuoteState = new QuoteStates(@this.Strategy.Session).Rejected;
            SetItemState(@this);
        }

        public static void BaseCancel(this Quote @this, QuoteCancel method)
        {
            @this.QuoteState = new QuoteStates(@this.Strategy.Session).Cancelled;
            SetItemState(@this);
        }

        public static void SetItemState(this Quote @this)
        {
            var quoteItemStates = new QuoteItemStates(@this.Strategy.Session);

            foreach (QuoteItem quoteItem in @this.QuoteItems)
            {
                if (@this.QuoteState.IsCreated)
                {
                    quoteItem.QuoteItemState = new QuoteItemStates(@this.Strategy.Session).Draft;
                }

                if (@this.QuoteState.IsCancelled)
                {
                    if (!Equals(quoteItem.QuoteItemState, quoteItemStates.Rejected))
                    {
                        quoteItem.QuoteItemState = new QuoteItemStates(@this.Strategy.Session).Cancelled;
                    }
                }

                if (@this.QuoteState.IsRejected)
                {
                    if (!Equals(quoteItem.QuoteItemState, quoteItemStates.Cancelled))
                    {
                        quoteItem.QuoteItemState = new QuoteItemStates(@this.Strategy.Session).Rejected;
                    }
                }

                if (@this.QuoteState.IsAwaitingApproval)
                {
                    if (Equals(quoteItem.QuoteItemState, quoteItemStates.Draft))
                    {
                        quoteItem.QuoteItemState = new QuoteItemStates(@this.Strategy.Session).AwaitingApproval;
                    }
                }

                if (@this.QuoteState.IsInProcess)
                {
                    if (!Equals(quoteItem.QuoteItemState, quoteItemStates.Cancelled)
                        && !Equals(quoteItem.QuoteItemState, quoteItemStates.Rejected))
                    {
                        quoteItem.QuoteItemState = new QuoteItemStates(@this.Strategy.Session).InProcess;
                    }
                }

                if (@this.QuoteState.IsAwaitingAcceptance)
                {
                    if (Equals(quoteItem.QuoteItemState, quoteItemStates.InProcess))
                    {
                        quoteItem.QuoteItemState = new QuoteItemStates(@this.Strategy.Session).AwaitingAcceptance;
                    }
                }

                if (@this.QuoteState.IsAccepted)
                {
                    if (Equals(quoteItem.QuoteItemState, quoteItemStates.AwaitingAcceptance))
                    {
                        quoteItem.QuoteItemState = new QuoteItemStates(@this.Strategy.Session).Accepted;
                    }
                }

                if (@this.QuoteState.IsOrdered)
                {
                    if (Equals(quoteItem.QuoteItemState, quoteItemStates.Accepted))
                    {
                        quoteItem.QuoteItemState = new QuoteItemStates(@this.Strategy.Session).Ordered;
                    }
                }
            }
        }

        private static void Sync(this Quote @this, IDerivation derivation)
        {
            var QuoteDerivedRoles = (QuoteDerivedRoles)@this;
            // Calculate Totals
            QuoteDerivedRoles.TotalBasePrice = 0;
            QuoteDerivedRoles.TotalDiscount = 0;
            QuoteDerivedRoles.TotalSurcharge = 0;
            QuoteDerivedRoles.TotalExVat = 0;
            QuoteDerivedRoles.TotalFee = 0;
            QuoteDerivedRoles.TotalShippingAndHandling = 0;
            QuoteDerivedRoles.TotalVat = 0;
            QuoteDerivedRoles.TotalIncVat = 0;
            QuoteDerivedRoles.TotalListPrice = 0;

            if (@this.ExistFee)
            {
                var fee = @this.Fee.Percentage.HasValue ?
                                    Math.Round(@this.TotalExVat * @this.Fee.Percentage.Value / 100, 2) :
                                    @this.Fee.Amount ?? 0;

                QuoteDerivedRoles.TotalFee += fee;
                QuoteDerivedRoles.TotalExVat += fee;

                if (@this.Fee.ExistVatRate)
                {
                    var vat1 = Math.Round(fee * @this.Fee.VatRate.Rate / 100, 2);
                    QuoteDerivedRoles.TotalVat += vat1;
                    QuoteDerivedRoles.TotalIncVat += fee + vat1;
                }
            }

            if (@this.ExistShippingAndHandlingCharge)
            {
                var shipping = @this.ShippingAndHandlingCharge.Percentage.HasValue ?
                                        Math.Round(@this.TotalExVat * @this.ShippingAndHandlingCharge.Percentage.Value / 100, 2) :
                                        @this.ShippingAndHandlingCharge.Amount ?? 0;

                QuoteDerivedRoles.TotalShippingAndHandling += shipping;
                QuoteDerivedRoles.TotalExVat += shipping;

                if (@this.ShippingAndHandlingCharge.ExistVatRate)
                {
                    var vat2 = Math.Round(shipping * @this.ShippingAndHandlingCharge.VatRate.Rate / 100, 2);
                    QuoteDerivedRoles.TotalVat += vat2;
                    QuoteDerivedRoles.TotalIncVat += shipping + vat2;
                }
            }

            //// Only take into account items for which there is data at the item level.
            //// Skip negative sales.
            decimal totalUnitBasePrice = 0;
            decimal totalListPrice = 0;

            foreach (QuoteItem item1 in @this.ValidQuoteItems)
            {
                if (item1.TotalExVat > 0)
                {
                    totalUnitBasePrice += item1.UnitBasePrice;
                    totalListPrice += item1.UnitPrice;
                }
            }
        }
    }
}
