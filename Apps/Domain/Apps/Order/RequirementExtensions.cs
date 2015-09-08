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

using System;

namespace Allors.Domain
{
    public static class RequirementExtensions
    {
        public static void AppsOnDerive(this Requirement @this, ObjectOnDerive method)
        {
            if (@this.ExistCurrentObjectState && !@this.CurrentObjectState.Equals(@this.PreviousObjectState))
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
    }
}
