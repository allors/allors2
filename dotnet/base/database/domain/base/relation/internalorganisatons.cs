// <copyright file="InternalOrganisatons.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;

    public partial class InternalOrganisations
    {
        protected override void BasePrepare(Setup setup)
        {
            setup.AddDependency(this.ObjectType, M.Locale.ObjectType);
            setup.AddDependency(this.ObjectType, M.TemplateType.ObjectType);
            setup.AddDependency(this.ObjectType, M.ShipmentMethod.ObjectType);
            setup.AddDependency(this.ObjectType, M.Carrier.ObjectType);
            setup.AddDependency(this.ObjectType, M.BillingProcess.ObjectType);
        }
    }
}
