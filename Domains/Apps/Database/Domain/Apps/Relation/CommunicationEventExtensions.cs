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
                @this.Owner = (Person)new Users(@this.Strategy.Session).CurrentUser;
            }

            if (@this.ExistScheduledStart && @this.ExistScheduledEnd && @this.ScheduledEnd < @this.ScheduledStart)
            {
                derivation.Validation.AddError(@this, M.CommunicationEvent.ScheduledEnd, ErrorMessages.EndDateBeforeStartDate);
            }

            if (@this.ExistActualStart && @this.ExistActualEnd && @this.ActualEnd < @this.ActualStart)
            {
                derivation.Validation.AddError(@this, M.CommunicationEvent.ActualEnd, ErrorMessages.EndDateBeforeStartDate);
            }

            if (!@this.ExistCurrentObjectState)
            {
                if (!@this.ExistActualStart || (@this.ExistActualStart && @this.ActualStart > DateTime.UtcNow))
                {
                    @this.CurrentObjectState = new CommunicationEventObjectStates(@this.Strategy.Session).Scheduled;
                }

                if (@this.ExistActualStart && @this.ActualStart <= DateTime.UtcNow &&
                    (@this.ExistActualEnd && @this.ActualEnd > DateTime.UtcNow || !@this.ExistActualEnd))
                {
                    @this.CurrentObjectState = new CommunicationEventObjectStates(@this.Strategy.Session).InProgress;
                }

                if (@this.ExistActualEnd && @this.ActualEnd <= DateTime.UtcNow)
                {
                    @this.CurrentObjectState = new CommunicationEventObjectStates(@this.Strategy.Session).Completed;
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

            @this.DeriveOwnerSecurity();
        }

        public static void AppsOnPostDerive(this CommunicationEvent @this, ObjectOnPostDerive method)
        {
            @this.AddSecurityToken(Singleton.Instance(@this.Strategy.Session).DefaultSecurityToken);
            @this.AddSecurityToken(@this.Owner?.OwnerSecurityToken);
        }

        public static void AppsDelete(this CommunicationEvent @this, DeletableDelete method)
        {
            @this.RemoveWorkEfforts();
        }

        public static void AppsClose(this CommunicationEvent @this, CommunicationEventClose method)
        {
            @this.CurrentObjectState = new CommunicationEventObjectStates(@this.Strategy.Session).Completed;
        }

        public static void AppsReopen(this CommunicationEvent @this, CommunicationEventReopen method)
        {
            @this.CurrentObjectState = new CommunicationEventObjectStates(@this.Strategy.Session).Scheduled;
        }

        public static void AppsCancel(this CommunicationEvent @this, CommunicationEventCancel method)
        {
            @this.CurrentObjectState = new CommunicationEventObjectStates(@this.Strategy.Session).Cancelled;
        }

        private static void DeriveOwnerSecurity(this CommunicationEvent @this)
        {
            if (!@this.ExistOwnerAccessControl)
            {
                var ownerRole = new Roles(@this.Strategy.Session).Owner;
                @this.OwnerAccessControl = new AccessControlBuilder(@this.Strategy.Session)
                        .WithRole(ownerRole)
                        .WithSubject(@this.Owner)
                        .Build();
            }

            if (!@this.ExistOwnerSecurityToken)
            {
                @this.OwnerSecurityToken = new SecurityTokenBuilder(@this.Strategy.Session)
                    .WithAccessControl(@this.OwnerAccessControl)
                    .Build();
            }
        }
    }
}
