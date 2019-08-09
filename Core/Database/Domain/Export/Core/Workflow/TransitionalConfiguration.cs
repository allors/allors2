// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransitionalConfiguration.cs" company="Allors bvba">
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