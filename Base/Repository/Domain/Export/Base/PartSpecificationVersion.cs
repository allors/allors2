// <copyright file="PartSpecificationVersion.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;
    using Allors.Repository.Attributes;

    #region Allors
    [Id("8C6B5DB8-778D-43E0-B7F9-C61E082F468A")]
    #endregion
    public partial class PartSpecificationVersion : Version
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid DerivationId { get; set; }

        public DateTime DerivationTimeStamp { get; set; }

        public User LastModifiedBy { get; set; }

        #endregion

        #region Allors
        [Id("1232404A-DC5B-40DA-A542-EFBF9222639B")]
        [AssociationId("260CBF68-AA11-44A9-933F-8D1981808D6B")]
        [RoleId("FF68AA62-097D-47B2-98B8-3604E44C9272")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        public PartSpecificationState PartSpecificationState { get; set; }

        #region Allors
        [Id("AC25B8DF-723C-4704-A942-A899C515268B")]
        [AssociationId("29C29608-CBBB-46CB-A2C5-DC5A49F6EF76")]
        [RoleId("75FA0CE8-BE5F-48EB-9498-16B15C7B2B89")]
        #endregion
        public DateTime DocumentationDate { get; set; }

        #region Allors
        [Id("268AE79C-47EE-4735-A970-BDC553508D5C")]
        [AssociationId("4675CC11-1153-4460-AAB7-B00A65D42A2B")]
        [RoleId("7F779C0F-165C-4946-8920-BAAF5A96DD46")]
        #endregion
        [Required]
        [Size(-1)]
        public string Description { get; set; }

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
