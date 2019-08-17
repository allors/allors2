// <copyright file="MaterialsUsage.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("f77787aa-66af-4d6a-bbe1-ce3d93020185")]
    #endregion
    public partial class MaterialsUsage : ServiceEntry
    {
        #region inherited properties
        public EngagementItem EngagementItem { get; set; }

        public bool IsBillable { get; set; }

        public string Description { get; set; }

        public WorkEffort WorkEffort { get; set; }

        public string Comment { get; set; }

        public LocalisedText[] LocalisedComments { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ThroughDate { get; set; }
        #endregion

        #region Allors
        [Id("a244ab38-6469-4aa4-ae7e-c245f17f2368")]
        [AssociationId("719acc0e-aaa9-465a-a08a-a283635cf48c")]
        [RoleId("441feb11-9913-4c2d-a27f-01f0c4ed27ae")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal Amount { get; set; }

        #region inherited methods
        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        public void Delete() { }

        #endregion
    }
}
