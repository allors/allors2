//------------------------------------------------------------------------------------------------- 
// <copyright file="DelegatedAccessControlledObject.cs" company="Allors bvba">
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

    #region Allors
    [Id("842FA7B5-2668-43E9-BFEF-21B6F5B20E8B")]
    #endregion
    public partial interface DelegatedAccessControlledObject : Object
    {
        [Id("C56B5BC5-35BD-4762-B237-54EA3BFC7E7A")]
        void DelegateAccess();
    }
}
