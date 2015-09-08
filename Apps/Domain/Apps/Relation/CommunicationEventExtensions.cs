// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommunicationEvent.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.CompilerServices;

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
            if (!@this.ExistOwner)
            {
                @this.Owner = (Person)new Users(@this.Strategy.Session).GetCurrentAuthenticatedUser();
            }

            if (!@this.ExistCurrentObjectState)
            {
                if (!@this.ExistActualStart || (@this.ExistActualStart && @this.ActualStart > DateTime.UtcNow))
                {
                    @this.CurrentObjectState = new CommunicationEventObjectStates(@this.Strategy.Session).Scheduled;
                    @this.CurrentCommunicationEventStatus = new CommunicationEventStatusBuilder(@this.Strategy.Session)
                        .WithCommunicationEventObjectState(@this.CurrentObjectState)
                        .WithStartDateTime(@this.ActualStart)
                        .Build();
                }

                if (@this.ExistActualStart && @this.ActualStart <= DateTime.UtcNow &&
                    (@this.ExistActualEnd && @this.ActualEnd > DateTime.UtcNow || !@this.ExistActualEnd))
                {
                    @this.CurrentObjectState = new CommunicationEventObjectStates(@this.Strategy.Session).InProgress;
                    @this.CurrentCommunicationEventStatus = new CommunicationEventStatusBuilder(@this.Strategy.Session)
                        .WithCommunicationEventObjectState(@this.CurrentObjectState)
                        .WithStartDateTime(@this.ActualStart)
                        .Build();
                }

                if (@this.ExistActualEnd && @this.ActualEnd <= DateTime.UtcNow)
                {
                    @this.CurrentObjectState = new CommunicationEventObjectStates(@this.Strategy.Session).Completed;
                    @this.CurrentCommunicationEventStatus = new CommunicationEventStatusBuilder(@this.Strategy.Session)
                        .WithCommunicationEventObjectState(@this.CurrentObjectState)
                        .WithStartDateTime(@this.ActualEnd)
                        .Build();
                }
            }

            if (@this.ExistCurrentObjectState && !@this.CurrentObjectState.Equals(@this.PreviousObjectState))
            {
                var currentStatus = new CommunicationEventStatusBuilder(@this.Strategy.Session).WithCommunicationEventObjectState(@this.CurrentObjectState).Build();
                @this.AddCommunicationEventStatus(currentStatus);
                @this.CurrentCommunicationEventStatus = currentStatus;
            }

            if (@this.ExistCurrentObjectState)
            {
                @this.CurrentObjectState.Process(@this);
            }

            if (!@this.ExistInitialScheduledStart && @this.ExistScheduledStart)
            {
                @this.InitialScheduledStart = @this.ScheduledStart;
            }

            if (!@this.ExistInitialScheduledEnd && @this.ExistScheduledEnd)
            {
                @this.InitialScheduledEnd = @this.ScheduledEnd;
            }
        }


        public static void AppsDelete(this CommunicationEvent @this, DeletableDelete method)
        {
            foreach (CommunicationEventStatus communicationEventStatus in @this.CommunicationEventStatuses)
            {
                communicationEventStatus.Delete();
            }

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
    }
}
