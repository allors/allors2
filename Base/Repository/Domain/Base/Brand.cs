// <copyright file="Brand.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("0a7ac589-946b-4d49-b7e0-7e0b9bc90111")]
    #endregion
    public partial class Brand : Deletable, Object
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("2A45A70B-ECF0-441E-AD89-52FC123BC79E")]
        [AssociationId("12157031-C8EA-4047-8FD5-969FF6B07C4C")]
        [RoleId("5F97231D-FEF9-4FC1-8F00-2E6BCFDF75FF")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Media LogoImage { get; set; }

        #region Allors
        [Id("C603F7EA-5201-464A-B657-BE23D42EF6DB")]
        [AssociationId("F733AB79-19D8-4829-97BB-51E3AC79D672")]
        [RoleId("68C0B00C-E857-4B2F-A6BC-4804C8B6AFFD")]
        [Required]
        [Workspace]
        #endregion
        public string Name { get; set; }

        #region Allors
        [Id("0DA86868-CD0A-4370-BD47-34790A22860F")]
        [AssociationId("0D13E3AF-A29D-4E96-9F0E-341DCF2B6AB6")]
        [RoleId("B36BF943-2FA0-4D67-B2D5-50C3E94BA79A")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public Model[] Models { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {
        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        public void Delete() { }

        #endregion
    }
}
