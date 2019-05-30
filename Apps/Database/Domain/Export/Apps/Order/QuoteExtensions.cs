// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QuoteExtensions.cs" company="Allors bvba">
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

using System.Linq;
using Allors.Meta;

namespace Allors.Domain
{
    public static partial class QuoteExtensions
    {

        public static void AppsOnBuild(this Quote @this, ObjectOnBuild method)
        {
            if (!@this.ExistQuoteState)
            {
                @this.QuoteState = new QuoteStates(@this.Strategy.Session).Created;
            }

            @this.AddSecurityToken(@this.Strategy.Session.GetSingleton().InitialSecurityToken);
        }

        public static void AppsOnDerive(this Quote @this, ObjectOnDerive method)
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

            @this.Price = 0;
            foreach (QuoteItem item in @this.QuoteItems)
            {
                @this.Price += item.LineTotal;
            }

            var singleton = @this.Strategy.Session.GetSingleton();

            @this.SecurityTokens = new[]
            {
                singleton.DefaultSecurityToken
            };

            if (@this.ExistIssuer)
            {
                @this.AddSecurityToken(@this.Issuer.LocalAdministratorSecurityToken);
            }
        }

        public static void AppsApprove(this Quote @this, QuoteApprove method)
        {
            @this.QuoteState = new QuoteStates(@this.Strategy.Session).Approved;
        }

        public static void AppsReject(this Quote @this, QuoteReject method)
        {
            @this.QuoteState = new QuoteStates(@this.Strategy.Session).Rejected;
        }

        public static void AppsCancel(this Quote @this, QuoteCancel method)
        {
            @this.QuoteState = new QuoteStates(@this.Strategy.Session).Cancelled;
        }
    }
}
