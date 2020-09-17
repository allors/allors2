// <copyright file="WorkEffortGoodStandard.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("81ddff76-9b82-4309-9c9f-f7f9dbd2db21")]
    #endregion
    public partial class WorkEffortGoodStandard : Object
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("086907b1-97c2-47c1-ade4-f7749f615ae1")]
        [AssociationId("f3cf9c9b-2d69-4ef7-8240-44d1ca53bc6f")]
        [RoleId("cd4b1f0a-425f-43d3-bc00-d64a0c4e84df")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public UnifiedProduct UnifiedProduct { get; set; }

        #region Allors
        [Id("28b3b976-3354-4095-b928-7c1474e8c492")]
        [AssociationId("3b07f539-a06c-4cdc-8790-98c05e097aa6")]
        [RoleId("211ae475-665a-4677-a9eb-376ed9c4d886")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        public decimal EstimatedCost { get; set; }

        #region Allors
        [Id("c94d5e97-ec2b-4d32-ae8d-145595f0ad91")]
        [AssociationId("3ddc2478-34ba-45fa-aa21-a11c856fbfe0")]
        [RoleId("33be021f-3194-4e54-b69e-844814ca0bbd")]
        #endregion

        public int EstimatedQuantity { get; set; }

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
