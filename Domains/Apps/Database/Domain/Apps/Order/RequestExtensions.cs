// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestExtensions.cs" company="Allors bvba">
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
using System.Runtime.CompilerServices;

namespace Allors.Domain
{
    using System;

    public static partial class RequestExtensions
    {
        public static void AppsOnDerive(this Request @this, ObjectOnDerive method)
        {
            var internalOrganisations = new Organisations(@this.Strategy.Session).Extent().Where(v => Equals(v.IsInternalOrganisation, true)).ToArray();

            if (!@this.ExistRecipient && internalOrganisations.Count() == 1)
            {
                @this.Recipient = internalOrganisations.First();
            }

            if (!@this.ExistRequestNumber)
            {
                @this.RequestNumber = @this.Recipient.NextRequestNumber();
            }

            @this.DeriveInitialObjectState();
        }

        public static void DeriveInitialObjectState(this Request @this)
        {
            if (!@this.ExistRequestState && !@this.ExistOriginator)
            {
                @this.RequestState = new RequestStates(@this.Strategy.Session).Anonymous;
            }

            if (!@this.ExistRequestState && @this.ExistOriginator)
            {
                @this.RequestState = new RequestStates(@this.Strategy.Session).Submitted;
            }
        }

        public static void AppsCancel(this Request @this, RequestCancel method)
        {
            @this.RequestState = new RequestStates(@this.Strategy.Session).Cancelled;
        }

        public static void AppsReject(this Request @this, RequestReject method)
        {
            @this.RequestState = new RequestStates(@this.Strategy.Session).Rejected;
        }

        public static void AppsSubmit(this Request @this, RequestSubmit method)
        {
            @this.RequestState = new RequestStates(@this.Strategy.Session).Submitted;
        }

        public static void AppsHold(this Request @this, RequestHold method)
        {
            @this.RequestState = new RequestStates(@this.Strategy.Session).PendingCustomer;
        }
    }
}
