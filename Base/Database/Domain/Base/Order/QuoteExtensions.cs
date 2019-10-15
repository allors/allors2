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
            if (!@this.ExistIssuer)
            {
                var internalOrganisations = new Organisations(@this.Strategy.Session).InternalOrganisations();

                if (internalOrganisations.Count() == 1)
                {
                    @this.Issuer = internalOrganisations.First();
                }
            }

            if (!@this.ExistQuoteNumber && @this.ExistIssuer)
            {
                @this.QuoteNumber = @this.Issuer.NextQuoteNumber(@this.Strategy.Session.Now().Year);
            }

            @this.Currency = @this.Currency ?? @this.Receiver?.PreferredCurrency ?? @this.Issuer?.PreferredCurrency;

            var singleton = @this.Strategy.Session.GetSingleton();

            @this.SecurityTokens = new[]
            {
                singleton.DefaultSecurityToken,
            };

            if (@this.ExistIssuer)
            {
                @this.AddSecurityToken(@this.Issuer.ProductQuoteApproverSecurityToken);
                @this.AddSecurityToken(@this.Issuer.LocalAdministratorSecurityToken);
            }
        }

        public static void BaseApprove(this Quote @this, QuoteApprove method) => @this.QuoteState = new QuoteStates(@this.Strategy.Session).Approved;

        public static void BaseReject(this Quote @this, QuoteReject method) => @this.QuoteState = new QuoteStates(@this.Strategy.Session).Rejected;

        public static void BaseCancel(this Quote @this, QuoteCancel method) => @this.QuoteState = new QuoteStates(@this.Strategy.Session).Cancelled;
    }
}
