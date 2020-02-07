// <copyright file="Employment.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Linq;

    public partial class Employment
    {
        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (iteration.IsMarked(this) || changeSet.IsCreated(this) || changeSet.HasChangedRoles(this))
            {
                iteration.Mark(this.Employer);
                iteration.Mark(this.Employee);
            }
        }

        public void BaseOnInit(ObjectOnInit method)
        {
            // TODO: Don't extent for InternalOrganisations
            var internalOrganisations = new Organisations(this.Strategy.Session).Extent().Where(v => Equals(v.IsInternalOrganisation, true)).ToArray();

            if (!this.ExistEmployer && internalOrganisations.Length == 1)
            {
                this.Employer = internalOrganisations.First();
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            this.Parties = new Party[] { this.Employee, this.Employer };
        }
    }
}
