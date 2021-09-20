// <copyright file="Organisation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Person type.</summary>

namespace Allors.Domain
{
    public partial class Organisation
    {
        public void CustomToggleCanWrite(OrganisationToggleCanWrite method)
        {
            if (this.ExistRestrictions)
            {
                this.RemoveRestrictions();
            }
            else
            {
                var toggleRestriction = new Restrictions(this.strategy.Session).ToggleRestriction;
                this.AddRestriction(toggleRestriction);
            }

            this.Address = this.MainAddress;
        }

        public void CustomJustDoIt(OrganisationJustDoIt method) => this.JustDidIt = true;

        public override string ToString() => this.Name;
    }
}
