// <copyright file="TransitionalConfiguration.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using System.Linq;

    using Allors.Meta;

    public partial class TransitionalConfiguration
    {
        public TransitionalConfiguration(Class objectType, RoleType roleType)
        {
            var previousObjectState = objectType.RoleTypes.FirstOrDefault(v => v.Name.Equals("Previous" + roleType.Name));
            var lastObjectState = objectType.RoleTypes.FirstOrDefault(v => v.Name.Equals("Last" + roleType.Name));

            this.ObjectState = roleType.RelationType;
            this.PreviousObjectState = previousObjectState?.RelationType ?? throw new Exception("Previous ObjectState is not defined for " + roleType.Name + " in type " + roleType.AssociationType.ObjectType.Name);
            this.LastObjectState = lastObjectState?.RelationType ?? throw new Exception("Last ObjectState is not defined for " + roleType.Name + " in type " + roleType.AssociationType.ObjectType.Name);
        }

        public RelationType ObjectState { get; set; }

        public RelationType PreviousObjectState { get; set; }

        public RelationType LastObjectState { get; set; }
    }
}
