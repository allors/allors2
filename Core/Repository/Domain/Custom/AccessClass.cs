// <copyright file="AccessClass.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("8E66F2E1-27FA-4A1C-B410-F082CA1621C7")]
    #endregion
    public partial class AccessClass : AccessInterface
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("81C11FD0-E121-4AE7-B624-441968B62089")]
        [AssociationId("DD922D9F-C308-4C87-B92C-9487E56576BA")]
        [RoleId("B3C8CF9E-3749-4C03-A50F-A07CCC2ED589")]
        #endregion
        [Required]
        public bool Block { get; set; }

        #region Allors
        [Id("A67189D3-CD06-425B-98BB-59E0E73AC211")]
        [AssociationId("E5C1A0C9-98D6-4376-AEED-F1B1ACF3BB31")]
        [RoleId("936B9106-0264-4ADE-B423-4FFB42C9FDF3")]
        #endregion
        public string Property { get; set; }

        #region inherited methods

        public void OnBuild()
        {
        }

        public void OnPostBuild()
        {
        }

        public void OnInit()
        {

        }

        public void OnPreDerive()
        {
        }

        public void OnDerive()
        {
        }

        public void OnPostDerive()
        {
        }

        public void DelegateAccess()
        {
        }
        #endregion
    }
}
