//------------------------------------------------------------------------------------------------- 
// <copyright file="TransitionalVersion.cs" company="Allors bvba">
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
    [Id("A13C9057-8786-40CA-8421-476E55787D73")]
    #endregion
    public partial interface TransitionalVersion : AccessControlledObject 
    {
        #region Allors
        [Id("96685F17-ABE3-459C-BF9F-8C5F05788C04")]
        [AssociationId("40D11625-EF9F-4358-9FC0-5C29248E41DA")]
        [RoleId("3893BB57-1EA6-4DEE-8248-483269CA30DA")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Synced]
        ObjectState[] PreviousObjectStates { get; set; }

        #region Allors
        [Id("39C43EB4-AA16-4CF8-93A0-60066CB746E8")]
        [AssociationId("AEB8A1DC-D214-429E-9A78-6FD60B419BE0")]
        [RoleId("DAC764A7-417C-4E24-985C-63171F7DC347")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Synced]
        ObjectState[] LastObjectStates { get; set; }

        #region Allors
        [Id("F2472C1F-8D2A-4400-B372-34C2B03207B6")]
        [AssociationId("08C19B44-2015-4BCA-B0E2-AB8CA626485F")]
        [RoleId("6C37AE50-8727-4391-A0E8-3596D5E2070F")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Synced]
        ObjectState[] ObjectStates { get; set; }
    }
}