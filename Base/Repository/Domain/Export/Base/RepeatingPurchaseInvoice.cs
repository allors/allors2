// <copyright file="RepeatingPurchaseInvoice.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("B5E190E1-F05F-420E-915C-0E5553D88109")]
    #endregion
    public partial class RepeatingPurchaseInvoice : Object
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
        #endregion

        #region Allors
        [Id("A2EC3A94-A529-4A5E-8B66-6CEE3F5231DD")]
        [AssociationId("F7D96209-39EE-46BF-876B-711A47896CD5")]
        [RoleId("0FFC6E4E-69F7-4306-AD61-F2F185732A0C")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public Organisation Supplier { get; set; }

        #region Allors
        [Id("8CF664E5-A7E8-4686-83AF-1C58A3CC5132")]
        [AssociationId("CBA60762-8EA4-42A8-B25D-4C331218A5DC")]
        [RoleId("8D8A48D3-CB51-4D15-9850-F29A185DD2A4")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        public InternalOrganisation InternalOrganisation { get; set; }

        #region Allors
        [Id("51361693-38A4-4FF8-8C64-57E78521EBB9")]
        [AssociationId("47C78348-0461-4A21-A4F4-7C39DCE358E1")]
        [RoleId("CB439ABF-5426-4E3B-8AC5-A47392C3A03E")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        public TimeFrequency Frequency { get; set; }

        #region Allors
        [Id("C16AFF4B-7DDC-4E7E-87FD-33F12F8B3219")]
        [AssociationId("82E60873-6946-4767-9972-788347EA6EAE")]
        [RoleId("4826C5F2-6DD1-4FE5-B19B-D5029D907AD5")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public DayOfWeek DayOfWeek { get; set; }

        #region Allors
        [Id("506127E5-0E94-44A4-9274-09E3D6C0103F")]
        [AssociationId("6E9500AE-1B86-4B5F-861A-CA076C9DADBB")]
        [RoleId("36815BBB-C38B-4F52-8E2C-21910849607F")]
        #endregion
        [Required]
        [Workspace]
        public DateTime NextExecutionDate { get; set; }

        #region Allors
        [Id("E95ED423-E581-4B3C-8DCA-9E8882B26FED")]
        [AssociationId("CC3A8BF9-2366-452B-938A-E7F6401D8F81")]
        [RoleId("643A5489-B952-42F2-8C07-6762E82FA628")]
        #endregion
        [Derived]
        [Workspace]
        public DateTime PreviousExecutionDate { get; set; }

        #region Allors
        [Id("B258B677-E0D0-4D22-9148-598960EA60A9")]
        [AssociationId("FDF0CE1D-02EA-4A61-9156-D183F1A02886")]
        [RoleId("8A73E50F-81F9-4946-ACBA-9463B7C0F603")]
        #endregion
        [Workspace]
        public DateTime FinalExecutionDate { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {

        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        #endregion
    }
}
