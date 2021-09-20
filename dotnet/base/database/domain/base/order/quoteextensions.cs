// <copyright file="QuoteExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using System.Linq;
    using Allors.Meta;
    using Resources;

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
                var year = @this.IssueDate.Year;
                @this.QuoteNumber = @this.Issuer.NextQuoteNumber(year);

                var fiscalYearInternalOrganisationSequenceNumbers = @this.Issuer.FiscalYearsInternalOrganisationSequenceNumbers.FirstOrDefault(v => v.FiscalYear == year);
                var prefix = @this.Issuer.QuoteSequence.IsEnforcedSequence ? @this.Issuer.QuoteNumberPrefix : fiscalYearInternalOrganisationSequenceNumbers.QuoteNumberPrefix;
                ((QuoteDerivedRoles)@this).SortableQuoteNumber = @this.Session().GetSingleton().SortableNumber(prefix, @this.QuoteNumber, year.ToString());
            }

            if (@this.QuoteState.IsCreated)
            {
                var quoteDerivedRoles = (QuoteDerivedRoles)@this;

                quoteDerivedRoles.DerivedLocale = @this.Locale ?? @this.Receiver?.Locale ?? @this.Issuer?.Locale;
                quoteDerivedRoles.DerivedCurrency = @this.AssignedCurrency ?? @this.Receiver?.PreferredCurrency ?? @this.Issuer?.PreferredCurrency;
                quoteDerivedRoles.DerivedVatRegime = @this.AssignedVatRegime;
                quoteDerivedRoles.DerivedIrpfRegime = @this.AssignedIrpfRegime;
            }

            if (@this.ExistIssueDate
                && @this.ExistIssuer
                && @this.DerivedCurrency != @this.Issuer.PreferredCurrency)
            {
                var exchangeRate = @this.DerivedCurrency.ExchangeRatesWhereFromCurrency.Where(v => v.ValidFrom.Date <= @this.IssueDate.Date && v.ToCurrency.Equals(@this.Issuer.PreferredCurrency)).OrderByDescending(v => v.ValidFrom).FirstOrDefault();

                if (exchangeRate == null)
                {
                    exchangeRate = @this.Issuer.PreferredCurrency.ExchangeRatesWhereFromCurrency.Where(v => v.ValidFrom.Date <= @this.IssueDate.Date && v.ToCurrency.Equals(@this.DerivedCurrency)).OrderByDescending(v => v.ValidFrom).FirstOrDefault();
                }

                if (exchangeRate == null)
                {
                    derivation.Validation.AddError(@this, M.Quote.AssignedCurrency, ErrorMessages.CurrencyNotAllowed);
                }
            }

            foreach (QuoteItem quoteItem in @this.QuoteItems)
            {
                var quoteItemDerivedRoles = (QuoteItemDerivedRoles)quoteItem;

                quoteItemDerivedRoles.DerivedVatRegime = quoteItem.AssignedVatRegime ?? @this.DerivedVatRegime;
                quoteItemDerivedRoles.VatRate = quoteItem.DerivedVatRegime?.VatRates.First(v => v.FromDate <= @this.IssueDate && (!v.ExistThroughDate || v.ThroughDate >= @this.IssueDate));

                quoteItemDerivedRoles.DerivedIrpfRegime = quoteItem.AssignedIrpfRegime ?? @this.DerivedIrpfRegime;
                quoteItemDerivedRoles.IrpfRate = quoteItem.DerivedIrpfRegime?.IrpfRates.First(v => v.FromDate <= @this.IssueDate && (!v.ExistThroughDate || v.ThroughDate >= @this.IssueDate));
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
                foreach (OrderAdjustment orderAdjustment in @this.OrderAdjustments)
                {
                    orderAdjustment.Delete();
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
        }
    }
}
