// <copyright file="Deduction.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{

    using Allors.Repository.Attributes;

    #region Allors
    [Id("c04ccfcf-ae3f-4e7f-9e19-503ba547b678")]
    #endregion
    public partial class Deduction : Object
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("0deb347e-22c7-4b48-b461-aa579e156398")]
        [AssociationId("aced036b-04ba-41da-a3fd-fb3d0782b8c6")]
        [RoleId("b09c7a91-bcca-4f80-b68c-309fdf1e80b0")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public DeductionType DeductionType { get; set; }

        #region Allors
        [Id("abaece2a-d56d-4af9-8421-1d587cd9dda2")]
        [AssociationId("b8d4b48b-292a-4348-8dba-15f89d573dd5")]
        [RoleId("1077f672-905b-4198-ada5-e52fb34c986e")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal Amount { get; set; }

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
