//------------------------------------------------------------------------------------------------- 
// <copyright file="NotificationList.cs" company="Allors bvba">
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
    [Id("b6579993-4ff1-4853-b048-1f8e67419c00")]
    #endregion
    public partial class NotificationList : AccessControlledObject, Deletable 
    {
        #region inherited properties
        #endregion

        #region Allors
        [Id("4516c5c1-73a0-4fdc-ac3c-aefaf417c8ba")]
        [AssociationId("7fb512b5-3440-444a-9562-ad3655e551e5")]
        [RoleId("9b7d6984-98cb-4367-a6fc-9b07c9101fda")]
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        #endregion
        public Notification[] Notifications { get; set; }

        #region Allors
        [Id("89487904-053e-470f-bcf9-0e01165b0143")]
        [AssociationId("2d41d7ef-d107-404f-ac9d-fb81105d3ff7")]
        [RoleId("fc089f2e-a625-40f9-bbc0-c9fc05e6e599")]
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        #endregion
        [Derived]
        [Workspace]
        public Notification[] UnconfirmedNotifications { get; set; }

        #region Allors
        [Id("438fdc30-25ac-4d33-9a55-0ef817c05479")]
        [AssociationId("34a36081-e093-4d8b-ae87-4a3df329f7a1")]
        [RoleId("b752a7c3-433c-4b54-bbc1-0f812d5afb16")]
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        #endregion
        [Derived]
        public Notification[] ConfirmedNotifications { get; set; }

        #region inherited methods

        public void OnBuild()
        {
        }

        public void OnPostBuild()
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

        public void Delete()
        {
        }

        #endregion

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }
    }
}