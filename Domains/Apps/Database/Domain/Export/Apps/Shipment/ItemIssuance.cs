// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PickList.cs" company="Allors bvba">
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

using System.Linq;

namespace Allors.Domain
{
    using System;
    using System.Collections.Generic;

    using Allors.Meta;

    public partial class ItemIssuance
    {
        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            derivation.AddDependency(this.ShipmentItem, this);
            derivation.Mark(this.ShipmentItem, M.ItemIssuance.ShipmentItem);
        }

        public void AppsOnPostBuild(ObjectOnPostBuild method)
        {
            if (!this.ExistIssuanceDateTime)
            {
                this.IssuanceDateTime = this.Strategy.Session.Now();
            }
        }
    }
}