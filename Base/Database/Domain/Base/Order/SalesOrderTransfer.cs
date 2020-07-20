// <copyright file="SalesOrderTransfer.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Meta;
    using Allors.Services;
    using Microsoft.Extensions.DependencyInjection;
    using Resources;

    public partial class SalesOrderTransfer
    {
        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (iteration.IsMarked(this) || changeSet.IsCreated(this) || changeSet.HasChangedRoles(this))
            {
                if (changeSet.HasChangedRole(this, this.Meta.From))
                {
                    iteration.AddDependency(this.From, this);
                    iteration.Mark(this.From);
                }

                if (changeSet.HasChangedRole(this, this.Meta.To))
                {
                    iteration.AddDependency(this.To, this);
                    iteration.Mark(this.To);
                }
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;
            var session = this.Session();
        }
    }
}
