// <copyright file="RepeatingSalesInvoice.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("2439F72A-A435-4070-9A11-EDCDF679FCC9")]
    #endregion
    public partial class RepeatingSalesInvoice : Object
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }
        #endregion

        #region Allors
        [Id("9B472F44-5546-4E75-861A-6B7E4E3A068C")]
        [AssociationId("9866D66A-2469-4D9F-B567-283A3E317284")]
        [RoleId("5FEB6B76-39C4-493B-B368-86E981812F39")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Required]
        [Workspace]
        public SalesInvoice Source { get; set; }

        #region Allors
        [Id("BFB6C78B-42AF-4704-A670-A28126072F47")]
        [AssociationId("B7F2D6D0-17A9-4596-90A7-BB8D8627917B")]
        [RoleId("4A3A0B99-ECE1-48D5-9B28-84BB1C16096C")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        public TimeFrequency Frequency { get; set; }

        #region Allors
        [Id("C97F7B2C-7D81-4022-BB4A-C0760BC15F1B")]
        [AssociationId("EF521018-B98A-4D1D-BC1E-2B7D08E58AA7")]
        [RoleId("2BC6D461-D209-4356-BB75-F34E8D492A75")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public DayOfWeek DayOfWeek { get; set; }

        #region Allors
        [Id("6BDEBD8A-6993-477B-83A4-F7CC52FBD677")]
        [AssociationId("D5B36209-44E7-4889-8386-79FFB9A72E63")]
        [RoleId("4D0A25E5-CCCC-4499-B77C-627DF7219830")]
        #endregion
        [Required]
        [Workspace]
        public DateTime NextExecutionDate { get; set; }

        #region Allors
        [Id("7BA2A639-17BA-4296-813E-DEB76F056B87")]
        [AssociationId("E0AC38F4-B8FD-42C1-A9C7-16AE9CB73C0C")]
        [RoleId("8562C190-A41B-43F8-898C-C1061BEF5CD5")]
        #endregion
        [Derived]
        [Workspace]
        public DateTime PreviousExecutionDate { get; set; }

        #region Allors
        [Id("6CC66FEB-8126-437B-8372-4B8EC7827FB6")]
        [AssociationId("16C4F78D-B088-4B86-A039-FAA923618975")]
        [RoleId("15629AEF-7979-48BB-8FFD-AFB5FCE1A0F9")]
        #endregion
        [Workspace]
        public DateTime FinalExecutionDate { get; set; }

        #region Allors
        [Id("B32E3E20-5F2D-476A-B11D-19C996436649")]
        [AssociationId("31E20918-2581-47BF-B383-F75BDF641143")]
        [RoleId("B520DF27-6270-491C-B8EE-01A922597532")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Derived]
        [Workspace]
        public SalesInvoice[] SalesInvoices { get; set; }

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
