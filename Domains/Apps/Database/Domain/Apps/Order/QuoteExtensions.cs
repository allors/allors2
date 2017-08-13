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
using System.Runtime.CompilerServices;

namespace Allors.Domain
{
    using System;

    public static partial class QuoteExtensions
    {

        public static void AppsOnBuild(this Quote @this, ObjectOnBuild method)
        {
            if (!@this.ExistCurrentObjectState)
            {
                @this.CurrentObjectState = new QuoteObjectStates(@this.Strategy.Session).Created;
            }

            if (!@this.ExistIssueDate)
            {
                @this.IssueDate = DateTime.UtcNow;
            }

            if (!@this.ExistIssuer)
            {
                @this.Issuer = Singleton.Instance(@this.Strategy.Session).DefaultInternalOrganisation;
            }

            if (!@this.ExistQuoteNumber)
            {
                @this.QuoteNumber = Singleton.Instance(@this.Strategy.Session).DefaultInternalOrganisation.DeriveNextQuoteNumber();
            }
        }

        public static void AppsOnDerive(this Quote @this, ObjectOnDerive method)
        {
            if (!@this.ExistQuoteNumber)
            {
                @this.QuoteNumber = Singleton.Instance(@this.Strategy.Session).DefaultInternalOrganisation.DeriveNextQuoteNumber();
            }

            @this.DeriveCurrentObjectState();
        }

        public static void DeriveCurrentObjectState(this Quote @this)
        {
            if (@this.ExistCurrentObjectState && !@this.CurrentObjectState.Equals(@this.LastObjectState))
            {
                var currentStatus = new QuoteStatusBuilder(@this.Strategy.Session)
                    .WithQuoteObjectState(@this.CurrentObjectState)
                    .WithStartDateTime(DateTime.UtcNow)
                    .Build();
                @this.AddQuoteStatus(currentStatus);
                @this.CurrentQuoteStatus = currentStatus;
            }
        }

        public static void AppsApprove(this Quote @this, QuoteApprove method)
        {
            @this.CurrentObjectState = new QuoteObjectStates(@this.Strategy.Session).Approved;
        }

        public static void AppsReject(this Quote @this, QuoteReject method)
        {
            @this.CurrentObjectState = new QuoteObjectStates(@this.Strategy.Session).Rejected;
        }
    }
}
