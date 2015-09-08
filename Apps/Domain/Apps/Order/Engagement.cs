// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Engagement.cs" company="Allors bvba">
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
    public partial class Engagement
    {
        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistTakenViaInternalOrganisation)
            {
                this.TakenViaInternalOrganisation = Domain.Singleton.Instance(this.Strategy.Session).DefaultInternalOrganisation;
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            if (!this.ExistBillToContactMechanism && this.ExistBillToParty)
            {
                this.BillToContactMechanism = this.BillToParty.BillingAddress;
            }

            if (!this.ExistTakenViaContactMechanism && this.ExistTakenViaInternalOrganisation)
            {
                this.TakenViaContactMechanism = this.TakenViaInternalOrganisation.OrderAddress;
            }

            if (!this.ExistPlacingContactMechanism && this.ExistPlacingParty)
            {
                this.PlacingContactMechanism = this.PlacingParty.OrderAddress;
            }
        }
    }
}