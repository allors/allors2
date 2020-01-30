// <copyright file="ProfessionalServicesRelationship.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class ProfessionalServicesRelationship
    {
        public void BaseOnDerive(ObjectOnDerive method)
        {
            this.Parties = new Party[] { this.Professional, this.ProfessionalServicesProvider };

            if (!this.ExistProfessional | !this.ExistProfessionalServicesProvider)
            {
                // TODO: Move Delete
                this.Delete();
            }
        }
    }
}
