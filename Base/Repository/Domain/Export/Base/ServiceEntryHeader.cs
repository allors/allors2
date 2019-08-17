// <copyright file="ServiceEntryHeader.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("22e85314-cfdf-4ead-a816-18588294fa79")]
    #endregion
    public partial class ServiceEntryHeader : Period, Object
    {
        #region inherited properties
        public DateTime FromDate { get; set; }

        public DateTime ThroughDate { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("6b29a626-04f6-423f-8ae5-cb49e8f9211d")]
        [AssociationId("9f14e67f-328b-44e6-8c80-707441848265")]
        [RoleId("21500c76-8a3e-4737-aa69-e348e06440e2")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]

        public ServiceEntry[] ServiceEntries { get; set; }
        #region Allors
        [Id("7e160fbc-1339-433c-9dcb-9b3ad58ad400")]
        [AssociationId("a9d0cbd8-bb20-45e1-b109-6620b23fa1b7")]
        [RoleId("ef8b435e-e354-45e2-89bc-3d452cc84f5a")]
        #endregion
        [Required]

        public DateTime SubmittedDate { get; set; }
        #region Allors
        [Id("902235fe-a6c5-47bb-936b-8b6ce54b3d15")]
        [AssociationId("1f93dde3-a9bd-4e10-8ec6-38edaec6ffb5")]
        [RoleId("3b27dd30-5452-480f-ae19-6937c422b541")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public Person SubmittedBy { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {

        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        #endregion

    }
}
