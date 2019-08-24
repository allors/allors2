// <copyright file="PartSpecification.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;

    public partial class PartSpecification
    {
        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            {
                new TransitionalConfiguration(M.PartSpecification, M.PartSpecification.PartSpecificationState),
            };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        public void BaseOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistPartSpecificationState)
            {
                this.PartSpecificationState = new PartSpecificationStates(this.Strategy.Session).Created;
            }
        }

        public void BaseApprove(PartSpecificationApprove method) => this.PartSpecificationState = new PartSpecificationStates(this.Strategy.Session).Approved;
    }
}
