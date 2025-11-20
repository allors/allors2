// <copyright file="AccessClass.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("355AEFD2-F5B2-499A-81D2-DD9C9F62832C")]
    #endregion
    public partial class MediaTyped : Object
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("D23961DF-6688-44D1-87D4-0E5D0C2ED533")]
        [AssociationId("EA16D7E4-1AB4-404A-B7AD-0101E5AB64F5")]
        [RoleId("2FB01840-76F0-4837-B81A-061922563912")]
        #endregion
        [Size(-1)]
        [MediaType("text/markdown")]
        [Workspace]
        public string Markdown { get; set; }

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
