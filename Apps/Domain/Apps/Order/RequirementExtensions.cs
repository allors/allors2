// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequirementExtensions.cs" company="Allors bvba">
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

    public static partial class RequirementExtensions
    {
        public static void AppsOnBuild(this Requirement @this, ObjectOnBuild method)
        {
            if (!@this.ExistCurrentObjectState)
            {
                @this.CurrentObjectState = new RequirementObjectStates(@this.Strategy.Session).Active;
            }
        }

        public static void AppsOnDerive(this Requirement @this, ObjectOnDerive method)
        {
            if (@this.ExistCurrentObjectState && !@this.CurrentObjectState.Equals(@this.LastObjectState))
            {
                var currentStatus = new RequirementStatusBuilder(@this.Strategy.Session)
                    .WithRequirementObjectState(@this.CurrentObjectState)
                    .WithStartDateTime(DateTime.UtcNow)
                    .Build();
                @this.AddRequirementStatus(currentStatus);
                @this.CurrentRequirementStatus = currentStatus;
            }

            if (@this.ExistCurrentObjectState)
            {
                @this.CurrentObjectState.Process(@this);
            }
        }

        public static void AppsCancel(this Requirement requirement)
        {
            requirement.CurrentObjectState = new RequirementObjectStates(requirement.Strategy.Session).Cancelled;
        }

        public static void AppsClose(this Requirement requirement)
        {
            requirement.CurrentObjectState = new RequirementObjectStates(requirement.Strategy.Session).Closed;
        }

        public static void AppsHold(this Requirement requirement)
        {
            requirement.CurrentObjectState = new RequirementObjectStates(requirement.Strategy.Session).OnHold;
        }

        public static void AppsDelete(this CommunicationEvent @this, DeletableDelete method)
        {
            @this.CurrentObjectState = new CommunicationEventObjectStates(@this.Strategy.Session).Completed;
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
