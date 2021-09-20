// <copyright file="Login.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Extent type.</summary>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("ad7277a8-eda4-4128-a990-b47fe43d120a")]
    #endregion
    public partial class Login : Deletable
    {
        #region inherited properties
        #endregion
        
        #region Allors
        [Id("18262218-a14f-48c3-87a5-87196d3b5974")]
        [AssociationId("3f067cf5-4fcb-4be4-9afb-15ba37700658")]
        [RoleId("e5393717-f46c-4a4c-a87f-3e4684428860")]
        #endregion
        [Indexed]
        [Size(256)]
        public string Key { get; set; }

        #region Allors
        [Id("7a82e721-d0b7-4567-aaef-bd3987ae6d01")]
        [AssociationId("2f2ef41d-8310-45fd-8ab5-e5d067862e3d")]
        [RoleId("c8e3851a-bc57-4acb-934a-4adfc37b1da7")]
        #endregion
        [Indexed]
        [Size(256)]
        public string Provider { get; set; }

        #region Allors
        [Id("B7957FD9-A43C-4E1E-844E-CA115DD33B23")]
        [AssociationId("1F4A70BF-4BC2-4919-BB0A-6C6B7545CEE9")]
        [RoleId("99E78F55-FE53-4F78-A135-4058E1C4A71F")]
        #endregion
        [Indexed]
        [Size(256)]
        public string DisplayName { get; set; }

        #region inherited methods

        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

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
