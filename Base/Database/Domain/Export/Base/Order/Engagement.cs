// <copyright file="Engagement.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class Engagement
    {
        public void BaseOnDerive(ObjectOnDerive method)
        {
            if (!this.ExistBillToContactMechanism && this.ExistBillToParty)
            {
                this.BillToContactMechanism = this.BillToParty.BillingAddress;
            }

            if (!this.ExistPlacingContactMechanism && this.ExistPlacingParty)
            {
                this.PlacingContactMechanism = this.PlacingParty.OrderAddress;
            }
        }
    }
}
