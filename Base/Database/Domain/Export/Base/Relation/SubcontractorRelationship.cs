
// <copyright file="SubcontractorRelationship.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class SubContractorRelationship
    {
        public void BaseOnDerive(ObjectOnDerive method)
        {
            this.Parties = new[] { this.Contractor, this.SubContractor };

            if (!this.ExistContractor || !this.ExistSubContractor)
            {
                this.Delete();
            }
        }
    }
}
