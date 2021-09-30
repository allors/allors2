// <copyright file="TransitionalExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
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
            @this.DerivedRoles.RemoveObjectStates();
            @this.DerivedRoles.RemoveLastObjectStates();
            @this.DerivedRoles.RemovePreviousObjectStates();
            foreach (var transitionalConfiguration in @this.TransitionalConfigurations)
            {
                var objectState = (ObjectState)@this.Strategy.GetCompositeRole(transitionalConfiguration.ObjectState);
                var lastObjectState = (ObjectState)@this.Strategy.GetCompositeRole(transitionalConfiguration.LastObjectState);
                var previousObjectState = (ObjectState)@this.Strategy.GetCompositeRole(transitionalConfiguration.PreviousObjectState);
                @this.DerivedRoles.AddObjectState(objectState);
                @this.DerivedRoles.AddLastObjectState(lastObjectState);
                @this.DerivedRoles.AddPreviousObjectState(previousObjectState);
            }

            // Update security
            ((TransitionalDerivedRoles)@this).Restrictions = @this.ObjectStates.Select(v => v.ObjectRestriction).ToArray();
        }

        public static bool HasChangedStates(this Transitional @this) =>
            @this.LastObjectStates.Count != @this.ObjectStates.Count || @this.LastObjectStates.Except(@this.ObjectStates).Count() != 0;
    }
}
