// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommunicationEvent.cs" company="Allors bvba">
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
using Allors.Meta;
using Resources;

namespace Allors.Domain
{
    using System;
    using System.Linq;

    public static partial class CommunicationEventExtensions
    {
        public static DateTime? AppsGetStart(this CommunicationEvent communicationEvent)
        {
            if (communicationEvent.ExistActualStart)
            {
                return communicationEvent.ActualStart;
            }

            if (communicationEvent.ExistScheduledStart)
            {
                return communicationEvent.ScheduledStart;
            }

            return null;
        }

        public static void AppsOnDerive(this CommunicationEvent @this, ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (!@this.ExistOwner)
            {
                var owner = @this.Strategy.Session.GetUser() as Person;
                if (owner == null)
                {
                    owner = @this.Strategy.Session.GetSingleton().Guest as Person;
                }

                @this.Owner = owner;
            }

            if (@this.ExistScheduledStart && @this.ExistScheduledEnd && @this.ScheduledEnd < @this.ScheduledStart)
            {
                derivation.Validation.AddError(@this, M.CommunicationEvent.ScheduledEnd, ErrorMessages.EndDateBeforeStartDate);
            }

            if (@this.ExistActualStart && @this.ExistActualEnd && @this.ActualEnd < @this.ActualStart)
            {
                derivation.Validation.AddError(@this, M.CommunicationEvent.ActualEnd, ErrorMessages.EndDateBeforeStartDate);
            }

            if (!@this.ExistCommunicationEventState)
            {
                if (!@this.ExistActualStart || (@this.ExistActualStart && @this.ActualStart > @this.Strategy.Session.Now()))
                {
                    @this.CommunicationEventState = new CommunicationEventStates(@this.Strategy.Session).Scheduled;
                }

                if (@this.ExistActualStart && @this.ActualStart <= @this.Strategy.Session.Now() &&
                    (@this.ExistActualEnd && @this.ActualEnd > @this.Strategy.Session.Now() || !@this.ExistActualEnd))
                {
                    @this.CommunicationEventState = new CommunicationEventStates(@this.Strategy.Session).InProgress;
                }

                if (@this.ExistActualEnd && @this.ActualEnd <= @this.Strategy.Session.Now())
                {
                    @this.CommunicationEventState = new CommunicationEventStates(@this.Strategy.Session).Completed;
                }
            }

            if (!@this.ExistInitialScheduledStart && @this.ExistScheduledStart)
            {
                @this.InitialScheduledStart = @this.ScheduledStart;
            }

            if (!@this.ExistInitialScheduledEnd && @this.ExistScheduledEnd)
            {
                @this.InitialScheduledEnd = @this.ScheduledEnd;
            }

            @this.DeriveInvolvedParties();
            @this.DeriveOwnerSecurity();
        }

        public static void AppsOnPostDerive(this CommunicationEvent @this, ObjectOnPostDerive method)
        {
            @this.AddSecurityToken(@this.Strategy.Session.GetSingleton().DefaultSecurityToken);
            @this.AddSecurityToken(@this.Owner?.OwnerSecurityToken);
        }

        public static void AppsDelete(this CommunicationEvent @this, DeletableDelete method)
        {
            @this.RemoveWorkEfforts();
        }

        public static void AppsClose(this CommunicationEvent @this, CommunicationEventClose method)
        {
            @this.CommunicationEventState = new CommunicationEventStates(@this.Strategy.Session).Completed;
        }

        public static void AppsReopen(this CommunicationEvent @this, CommunicationEventReopen method)
        {
            @this.CommunicationEventState = new CommunicationEventStates(@this.Strategy.Session).Scheduled;
        }

        public static void AppsCancel(this CommunicationEvent @this, CommunicationEventCancel method)
        {
            @this.CommunicationEventState = new CommunicationEventStates(@this.Strategy.Session).Cancelled;
        }

        private static void DeriveOwnerSecurity(this CommunicationEvent @this)
        {
            if (!@this.ExistOwnerAccessControl)
            {
                var ownerRole = new Roles(@this.Strategy.Session).Owner;
                @this.OwnerAccessControl = new AccessControlBuilder(@this.Strategy.Session).WithRole(ownerRole)
                    .WithSubject(@this.Owner).Build();
            }

            if (!@this.ExistOwnerSecurityToken)
            {
                @this.OwnerSecurityToken = new SecurityTokenBuilder(@this.Strategy.Session)
                    .WithAccessControl(@this.OwnerAccessControl).Build();
            }
        }

        private static void DeriveInvolvedParties(this CommunicationEvent @this)
        {
            var partiesToRemove = @this.PartiesWhereCommunicationEvent.ToList();

            if (@this.GetType().Name.Equals(typeof(EmailCommunication).Name))
            {
                var mail = (EmailCommunication)@this;

                if (mail.ExistFromEmail)
                {
                    mail.FromParty = mail.FromEmail.PartyWherePersonalEmailAddress;
                }

                if (mail.ExistToEmail)
                {
                    mail.ToParty = mail.ToEmail.PartyWherePersonalEmailAddress;
                }
            }

            @this.RemoveInvolvedParties();

            if (@this.ExistFromParty)
            {
                @this.AddInvolvedParty(@this.FromParty);
                @this.FromParty.AddCommunicationEvent(@this);
                if (partiesToRemove.Contains(@this.FromParty))
                {
                    partiesToRemove.Remove(@this.FromParty);
                }
            }

            if (@this.ExistToParty)
            {
                @this.AddInvolvedParty(@this.ToParty);
                @this.ToParty.AddCommunicationEvent(@this);
                if (partiesToRemove.Contains(@this.ToParty))
                {
                    partiesToRemove.Remove(@this.ToParty);
                }
            }

            if (@this.ExistOwner)
            {
                @this.AddInvolvedParty(@this.Owner);
                @this.Owner.AddCommunicationEvent(@this);
                if (partiesToRemove.Contains(@this.Owner))
                {
                    partiesToRemove.Remove(@this.Owner);
                }
            }

            foreach (Party party in @this.InvolvedParties)
            {
                if (party is Person person)
                {
                    foreach (OrganisationContactRelationship organisationContactRelationship in person.OrganisationContactRelationshipsWhereContact)
                    {
                        if (organisationContactRelationship.FromDate <= @this.Strategy.Session.Now() &&
                            (!organisationContactRelationship.ExistThroughDate || organisationContactRelationship.ThroughDate >= @this.Strategy.Session.Now()))
                        {
                            var organisation = organisationContactRelationship.Organisation;
                            @this.AddInvolvedParty(organisation);
                            organisation.AddCommunicationEvent(@this);
                            if (partiesToRemove.Contains(organisation))
                            {
                                partiesToRemove.Remove(organisation);
                            }
                        }
                    }
                }
            }

            foreach (Party party in partiesToRemove)
            {
                party.RemoveCommunicationEvent(@this);
            }
        }
    }
}
