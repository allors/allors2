// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransitionalExtensions.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
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

namespace Allors.Domain
{
    using System.Linq;

    public static partial class TransitionalExtensions
    {
        public static void CoreOnPostDerive(this Transitional @this, ObjectOnPostDerive method)
        {
            // Update PreviousObjectState and LastObjectState
            foreach (var transitionalConfiguration in @this.TransitionalConfigurations)
            {
                var objectState = @this.Strategy.GetCompositeRole(transitionalConfiguration.ObjectState);
                var lastObjectState = @this.Strategy.GetCompositeRole(transitionalConfiguration.LastObjectState);

                if (objectState != null && !objectState.Equals(lastObjectState))
                {
                    @this.Strategy.SetCompositeRole(transitionalConfiguration.PreviousObjectState, lastObjectState);
                }

                @this.Strategy.SetCompositeRole(transitionalConfiguration.LastObjectState, objectState);
            }

            // Rollup ObjectStates, PreviousObjectState and LastObjectStates
            @this.RemoveObjectStates();
            @this.RemoveLastObjectStates();
            @this.RemovePreviousObjectStates();
            foreach (var transitionalConfiguration in @this.TransitionalConfigurations)
            {
                var objectState = (ObjectState)@this.Strategy.GetCompositeRole(transitionalConfiguration.ObjectState);
                var lastObjectState = (ObjectState)@this.Strategy.GetCompositeRole(transitionalConfiguration.LastObjectState);
                var previousObjectState = (ObjectState)@this.Strategy.GetCompositeRole(transitionalConfiguration.PreviousObjectState);
                @this.AddObjectState(objectState);
                @this.AddLastObjectState(lastObjectState);
                @this.AddPreviousObjectState(previousObjectState);
            }

            // Update security
            @this.DeniedPermissions = @this.ObjectStates.SelectMany(v => v.DeniedPermissions).ToArray();
        }
    }
}