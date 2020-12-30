// <copyright file="RequestExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Linq;
    using Allors.Meta;

    public static partial class RequestExtensions
    {
        public static bool IsDeletable(this Request @this) =>
            // EmailAddress is used whith anonymous request form website
            !@this.ExistEmailAddress 
            && (@this.RequestState.Equals(new RequestStates(@this.Strategy.Session).Submitted)
                || @this.RequestState.Equals(new RequestStates(@this.Strategy.Session).Cancelled)
                || @this.RequestState.Equals(new RequestStates(@this.Strategy.Session).Rejected))
            && !@this.ExistQuoteWhereRequest
            && @this.RequestItems.All(v => v.IsDeletable);

        public static void BaseOnBuild(this Request @this, ObjectOnBuild method)
        {
            if (!@this.ExistRequestState && !@this.ExistOriginator)
            {
                @this.RequestState = new RequestStates(@this.Session()).Anonymous;
            }

            if (!@this.ExistRequestState && @this.ExistOriginator)
            {
                @this.RequestState = new RequestStates(@this.Session()).Submitted;
            }
        }

        public static void BaseOnPreDerive(this Request @this, ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (iteration.IsMarked(@this) || changeSet.IsCreated(@this) || changeSet.HasChangedRoles(@this))
            {
                foreach (RequestItem requestItem in @this.RequestItems)
                {
                    iteration.AddDependency(requestItem, @this);
                    iteration.Mark(requestItem);
                }
            }
        }

        public static void BaseOnDerive(this Request @this, ObjectOnDerive method)
        {
            var session = @this.Strategy.Session;
            if (!@this.ExistRecipient)
            {
                var internalOrganisations = new Organisations(session).InternalOrganisations();

                if (internalOrganisations.Count() == 1)
                {
                    @this.Recipient = internalOrganisations.Single();
                }
            }

            if (@this.ExistRecipient && !@this.ExistRequestNumber)
            {
                var year = @this.RequestDate.Year;
                @this.RequestNumber = @this.Recipient.NextRequestNumber(year);

                var fiscalYearInternalOrganisationSequenceNumbers = @this.Recipient.FiscalYearsInternalOrganisationSequenceNumbers.FirstOrDefault(v => v.FiscalYear == year);
                var prefix = @this.Recipient.RequestSequence.IsEnforcedSequence ? @this.Recipient.RequestNumberPrefix : fiscalYearInternalOrganisationSequenceNumbers.RequestNumberPrefix;
                ((RequestDerivedRoles)@this).SortableRequestNumber = @this.Session().GetSingleton().SortableNumber(prefix, @this.RequestNumber, year.ToString());
            }

            @this.DeriveInitialObjectState();


            @this.AddSecurityToken(new SecurityTokens(session).DefaultSecurityToken);
        }

        public static void DeriveInitialObjectState(this Request @this)
        {
            var session = @this.Strategy.Session;

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

        public static void BaseDelete(this Request @this, DeletableDelete method)
        {
            if (@this.IsDeletable())
            {
                foreach (RequestItem item in @this.RequestItems)
                {
                    item.Delete();
                }
            }
        }

        public static void BaseCancel(this Request @this, RequestCancel method) => @this.RequestState = new RequestStates(@this.Strategy.Session).Cancelled;

        public static void BaseReject(this Request @this, RequestReject method) => @this.RequestState = new RequestStates(@this.Strategy.Session).Rejected;

        public static void BaseSubmit(this Request @this, RequestSubmit method) => @this.RequestState = new RequestStates(@this.Strategy.Session).Submitted;

        public static void BaseHold(this Request @this, RequestHold method) => @this.RequestState = new RequestStates(@this.Strategy.Session).PendingCustomer;
    }
}
