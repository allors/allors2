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

    public partial interface Object
    {
        [Id("FDD32313-CF62-4166-9167-EF90BE3A3C75")]
        void OnBuild();

        [Id("2B827E22-155D-4AA8-BA9F-46A64D7C79C8")]
        void OnPostBuild();

        [Id("4E5A4C91-C430-49FB-B15D-D4CB0155C551")]
        void OnInit();

        [Id("B33F8EAE-17DC-4BF9-AFBB-E7FC38F42695")]
        void OnPreDerive();

        [Id("C107F8B3-12DC-4FF9-8CBF-A7DEC046244F")]
        void OnDerive();

        [Id("07AFF35D-F4CB-48FE-A39A-176B1931FAB7")]
        void OnPostDerive();
    }
}