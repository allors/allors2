//-------------------------------------------------------------------------------------------------
// <copyright file="LocalisedText.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Extent type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("020f5d4d-4a59-4d7b-865a-d72fc70e4d97")]
    #endregion
    public partial class LocalisedText : Localised, Deletable, Object
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Locale Locale { get; set; }

        #endregion

        #region Allors
        [Id("50dc85f0-3d22-4bc1-95d9-153674b89f7a")]
        [AssociationId("accd061b-20b9-4a24-bb2c-c2f7276f43ab")]
        [RoleId("8d3f68e1-fa6e-414f-aa4d-25fcc2c975d6")]
        #endregion
        [Size(-1)]
        [Workspace]
        public string Text { get; set; }

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
