// <copyright file="OrganisationRollup.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class OrganisationRollUp
    {
        public void BaseOnDerive(ObjectOnDerive method)
        {
            this.Parties = new Party[] { this.Child, this.Parent };

            if (!this.ExistParent | !this.ExistChild)
            {
                // TODO: Move Delete
                this.Delete();
            }
        }
    }
}
