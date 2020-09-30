// <copyright file="TimeEntryBilling.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("DDDA4365-DD74-4664-8B7D-92C894AECA21")]
    #endregion
    public partial class TimeEntryBilling : Object, Deletable
    {
        #region inherited properties
        #endregion

        #region Allors
        [Id("A6C5F1EB-53B1-4A62-8F94-1ECCFD87048A")]
        [AssociationId("3983E6E9-68E3-49A5-9A59-650C56919FA5")]
        [RoleId("A7067992-DA9C-4D11-97D7-7406FC5B07CE")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        public TimeEntry TimeEntry { get; set; }

        #region Allors
        [Id("9D155FCD-CAF9-4150-A632-4FF5F3BAA2DF")]
        [AssociationId("ADC56D19-5BF2-4AEC-9A8B-A5D8F1BB1D4A")]
        [RoleId("C465ABA8-B4FB-4A00-BAC5-F5C8CC0569EE")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        public InvoiceItem InvoiceItem { get; set; }

        #region inherited methods

        public Permission[] DeniedPermissions { get; set; }

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
