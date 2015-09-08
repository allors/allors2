// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShipmentPackage.cs" company="Allors bvba">
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

namespace Allors.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public partial class ShipmentPackage
    {
        public decimal TotalQuantity
        {
            get
            {
                var total = 0M;
                foreach (PackagingContent packagingContent in this.PackagingContents)
                {
                    total += packagingContent.Quantity;
                }

                return total;
            }
        }

        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistCreationDate)
            {
                this.CreationDate = DateTime.UtcNow;
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            this.AppsOnDeriveSequenceNumber(derivation);

            if (!this.ExistDocuments)
            {
                var name = string.Format("Package {0}", this.ExistSequenceNumber ? this.SequenceNumber.ToString(CultureInfo.InvariantCulture) : string.Empty);
                this.AddDocument(new PackagingSlipBuilder(this.Strategy.Session).WithName(name).Build());
            }
        }

        public void AppsOnDeriveSequenceNumber(IDerivation derivation)
        {
            var highestNumber = 0;
            if (this.ExistShipmentWhereShipmentPackage)
            {
                foreach (ShipmentPackage shipmentPackage in this.ShipmentWhereShipmentPackage.ShipmentPackages)
                {
                    if (shipmentPackage.ExistSequenceNumber && shipmentPackage.SequenceNumber > highestNumber)
                    {
                        highestNumber = shipmentPackage.SequenceNumber;
                    }
                }

                if (!this.ExistSequenceNumber || this.SequenceNumber == 0)
                {
                    this.SequenceNumber = highestNumber + 1;
                }
            }
        }
    }
}