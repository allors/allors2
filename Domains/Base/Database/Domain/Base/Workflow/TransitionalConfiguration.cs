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
        public TransitionalConfiguration(RoleType roleType)
        {
            try
            {
                var objectType = roleType.AssociationType.ObjectType;
                var previousObjectState = objectType.RoleTypes.First(v => v.Name.Equals("Previous" + roleType.Name));
                var lastObjectState = objectType.RoleTypes.First(v => v.Name.Equals("Last" + roleType.Name));

                this.ObjectState = roleType.RelationType;
                this.PreviousObjectState = previousObjectState.RelationType;
                this.LastObjectState = lastObjectState.RelationType;
            }
            catch
            {
                throw new Exception("Could not create TransitionalConfiguration for " + roleType.FullName);
            }
        }

        public RelationType ObjectState { get; set; }

        public RelationType PreviousObjectState { get; set; }

        public RelationType LastObjectState { get; set; }
    }
}