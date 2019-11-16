// <copyright file="AccessClass.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("4A1ABF3F-3A03-4B31-99B9-C16A9F27268B")]
    #endregion
    public partial class MediaTyped : Object
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("A01C4AD6-A07E-48D0-B3FB-A35ADEDC9050")]
        [AssociationId("053E9EDC-ABFC-474E-8D6B-827DEC42DBFB")]
        [RoleId("CC1FB0B7-1AF7-4C71-AAD7-4076DEFCC3AE")]
        #endregion
        [Size(-1)]
        [MediaType("text/markdown")]
        public string Block { get; set; }

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
        #endregion
    }
}
