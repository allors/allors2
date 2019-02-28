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

    using Allors.Meta;

    public static partial class RequestExtensions
    {
        public static void AppsOnDerive(this Request @this, ObjectOnDerive method)
        {
            if (!@this.ExistRecipient)
            {
                var internalOrganisations = new Organisations(@this.Strategy.Session).InternalOrganisations();
                
                if (internalOrganisations.Count() == 1)
                {
                    @this.Recipient = internalOrganisations.Single();
                }
            }

            if (@this.ExistRecipient && !@this.ExistRequestNumber)
            {
                @this.RequestNumber = @this.Recipient.NextRequestNumber();
            }

            @this.DeriveInitialObjectState();
        }

        public static void DeriveInitialObjectState(this Request @this)
        {
            var session = @this.Strategy.Session;
            if (!@this.ExistRequestState && !@this.ExistOriginator)
            {
                @this.RequestState = new RequestStates(session).Anonymous;
            }

            if (!@this.ExistRequestState && @this.ExistOriginator)
            {
                @this.RequestState = new RequestStates(session).Submitted;
            }

            if (@this.ExistRequestState && @this.RequestState.Equals(new RequestStates(session).Anonymous) && @this.ExistOriginator)
            {
                @this.RequestState = new RequestStates(session).Submitted;

                if (@this.ExistEmailAddress 
                    && @this.Originator.PartyContactMechanisms.Where(v => v.ContactMechanism.GetType().Name == typeof(EmailAddress).Name).FirstOrDefault(v => ((EmailAddress)v.ContactMechanism).ElectronicAddressString.Equals(@this.EmailAddress)) == null)
                {
                    @this.Originator.AddPartyContactMechanism(
                        new PartyContactMechanismBuilder(session)
                        .WithContactMechanism(new EmailAddressBuilder(session).WithElectronicAddressString(@this.EmailAddress).Build())
                        .WithContactPurpose(new ContactMechanismPurposes(session).GeneralEmail)
                        .Build());
                }

                if (@this.ExistTelephoneNumber 
                    && @this.Originator.PartyContactMechanisms.Where(v => v.ContactMechanism.GetType().Name == typeof(TelecommunicationsNumber).Name).FirstOrDefault(v => ((TelecommunicationsNumber)v.ContactMechanism).ContactNumber.Equals(@this.TelephoneNumber)) == null)
                {
                    @this.Originator.AddPartyContactMechanism(
                        new PartyContactMechanismBuilder(session)
                            .WithContactMechanism(new TelecommunicationsNumberBuilder(session).WithContactNumber(@this.TelephoneNumber).WithCountryCode(@this.TelephoneCountryCode).Build())
                            .WithContactPurpose(new ContactMechanismPurposes(session).GeneralPhoneNumber)
                            .Build());
                }
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
