// <copyright file="QuoteExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
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

            @this.AddSecurityToken(new SecurityTokens(session).DefaultSecurityToken);
        }

        public static void BaseApprove(this Quote @this, QuoteApprove method)
        {
            @this.QuoteState = new QuoteStates(@this.Strategy.Session).Approved;
            SetItemState(@this);
        }

        public static void BaseSend(this Quote @this, QuoteSend method)
        {
            @this.QuoteState = new QuoteStates(@this.Strategy.Session).Sent;
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

        private static void SetItemState(this Quote @this)
        {
            var quoteItemStates = new QuoteItemStates(@this.Strategy.Session);

            foreach (QuoteItem quoteItem in @this.QuoteItems)
            {
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

                if (@this.QuoteState.IsApproved)
                {
                    if (!Equals(quoteItem.QuoteItemState, quoteItemStates.Cancelled)
                        && !Equals(quoteItem.QuoteItemState, quoteItemStates.Rejected))
                    {
                        quoteItem.QuoteItemState = new QuoteItemStates(@this.Strategy.Session).Approved;
                    }
                }

                if (@this.QuoteState.IsSent)
                {
                    if (Equals(quoteItem.QuoteItemState, quoteItemStates.Approved))
                    {
                        quoteItem.QuoteItemState = new QuoteItemStates(@this.Strategy.Session).Sent;
                    }
                }
            }
        }
    }
}
