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

            if (!@this.ExistQuoteNumber)
            {
                @this.QuoteNumber = @this.Strategy.Session.GetSingleton().InternalOrganisation.DeriveNextQuoteNumber();
            }
        }

        public static void AppsOnDerive(this Quote @this, ObjectOnDerive method)
        {
            @this.Price = 0;
            foreach (QuoteItem item in @this.QuoteItems)
            {
                @this.Price += item.Quantity * item.UnitPrice;
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
    }
}
