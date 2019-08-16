//------------------------------------------------------------------------------------------------- 
// <copyright file="Object.cs" company="Allors bvba">
// Copyright 2002-2016 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>Defines the Extent type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Repository
{
    using Attributes;

    [Id("12504f04-02c6-4778-98fe-04eba12ef8b2")]
    public partial interface Object
    {
        #region Allors
        [Id("5c70ca14-4601-4c65-9b0d-cb189f90be27")]
        [AssociationId("267053f0-43b4-4cc7-a0e2-103992b2d0c5")]
        [RoleId("867765fa-49dc-462f-b430-3c0e264c5283")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        Permission[] DeniedPermissions { get; set; }

        #region Allors
        [Id("b816fccd-08e0-46e0-a49c-7213c3604416")]
        [AssociationId("1739db0d-fe6b-42e1-a6a5-286536ff4f56")]
        [RoleId("9f722315-385a-42ab-b84e-83063b0e5b0d")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        SecurityToken[] SecurityTokens { get; set; }

        #region Allors
        [Id("FDD32313-CF62-4166-9167-EF90BE3A3C75")]
        #endregion
        void OnBuild();

        #region Allors
        [Id("2B827E22-155D-4AA8-BA9F-46A64D7C79C8")]
        #endregion
        void OnPostBuild();

        #region Allors
        [Id("4E5A4C91-C430-49FB-B15D-D4CB0155C551")]
        #endregion
        void OnInit();

        #region Allors
        [Id("B33F8EAE-17DC-4BF9-AFBB-E7FC38F42695")]
        #endregion
        void OnPreDerive();

        #region Allors
        [Id("C107F8B3-12DC-4FF9-8CBF-A7DEC046244F")]
        #endregion
        void OnDerive();

        #region Allors
        [Id("07AFF35D-F4CB-48FE-A39A-176B1931FAB7")]
        #endregion
        void OnPostDerive();
    }
}
