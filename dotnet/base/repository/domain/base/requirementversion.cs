// <copyright file="RequirementVersion.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("B0A09032-FEC1-4047-8264-8DBD68C281A0")]
    #endregion
    public partial class RequirementVersion : Version
    {
        #region inherited properties

        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid DerivationId { get; set; }

        public DateTime DerivationTimeStamp { get; set; }

        public User LastModifiedBy { get; set; }

        #endregion

        #region Allors
        [Id("BB7D5EA4-6D52-4E64-B18C-CD74FA010102")]
        [AssociationId("000AD1EC-523D-4619-A5C3-9A922A1A7294")]
        [RoleId("E59B163A-8336-4BF0-BE60-9B3BC7859418")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public RequirementState RequirementState { get; set; }

        #region Allors
        [Id("5719FF6E-EEC1-4465-A9B0-6B0779CFE2B4")]
        [AssociationId("E78BC638-1E36-494D-B3A8-02598C8D3170")]
        [RoleId("B867C467-0CD3-4AB7-A63E-E216E5F64FB2")]
        #endregion
        [Workspace]
        public DateTime RequiredByDate { get; set; }

        #region Allors
        [Id("4F5BF6F8-C4B8-4E3B-A1F2-6E212322AB8B")]
        [AssociationId("B9D7CFC7-D118-4E6C-BB52-023C5EFC4FFC")]
        [RoleId("28304D0A-288E-4ECD-986D-30D7A27A35DF")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public RequirementType RequirementType { get; set; }

        #region Allors
        [Id("37EFDA28-11F6-45C1-8243-AE38F8DAD9C9")]
        [AssociationId("D6C90564-5E19-447D-A225-8871D640648C")]
        [RoleId("8C6B5E25-E40A-4451-8975-338623B0971F")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Party Authorizer { get; set; }

        #region Allors
        [Id("86DF3995-B405-4A26-934C-40C47DFA48D9")]
        [AssociationId("C5325961-65C9-455D-8F28-BF962E591FD4")]
        [RoleId("91C176F9-38E6-4F3C-95CD-84B1951D6B39")]
        #endregion
        [Size(-1)]
        [Workspace]
        public string Reason { get; set; }

        #region Allors
        [Id("58872073-760D-4E9C-9926-A6A784329010")]
        [AssociationId("B9F4ED20-ED1D-49E4-A6E1-676795F5BF62")]
        [RoleId("A4C472A9-D517-4997-817D-D4F2C70CD01C")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public Requirement[] Children { get; set; }

        #region Allors
        [Id("82377E0E-9AA2-4571-BB4C-A859AFB1DB22")]
        [AssociationId("A8F4747F-AE32-447F-AC74-2E79A2EB54C4")]
        [RoleId("68704777-F144-40A1-B5EA-BC99A8C60414")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Party NeededFor { get; set; }

        #region Allors
        [Id("367D85A1-D463-440C-A0F0-1DA4633EE6F1")]
        [AssociationId("345C15CB-A38D-4080-A156-A9065266FEFD")]
        [RoleId("2623EE5E-8E57-488E-BFC1-1BA97406F5B2")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Party Originator { get; set; }

        #region Allors
        [Id("6775C4EA-06F2-48DC-854A-7913510D117A")]
        [AssociationId("F69AF840-E9A5-4DC3-9396-6227859A796D")]
        [RoleId("0933FB21-3260-4BF1-8ED6-2EF2DDC52F67")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Facility Facility { get; set; }

        #region Allors
        [Id("2FA42AB7-9B48-44E2-B929-3D7040555425")]
        [AssociationId("A6A569BF-2D95-4D0D-AC1C-B953FE11D421")]
        [RoleId("85ADBBC5-E5A0-4112-AB7B-560A13C77FD2")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Party ServicedBy { get; set; }

        #region Allors
        [Id("9E02E045-F22F-4D4B-9382-42593FDF7FD6")]
        [AssociationId("04AE5EF8-7DEB-4144-A680-25900E0A6F18")]
        [RoleId("2E0981C3-913B-451F-AF53-BF24DAEAA87C")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal EstimatedBudget { get; set; }

        #region Allors
        [Id("0A5A967A-E35D-45E3-BE3D-588A9633E330")]
        [AssociationId("253FD290-B06A-4C6F-89C8-EB4DD6CECB8D")]
        [RoleId("A9700ACD-06E6-4867-9CF1-90499A6C6812")]
        #endregion
        [Required]
        [Size(-1)]
        [Workspace]
        public string Description { get; set; }

        #region Allors
        [Id("571C92CC-F79E-49D9-BCE4-DDADDABD7F6B")]
        [AssociationId("923B3FEC-0BBE-4CCF-911F-3E07AB76BFA8")]
        [RoleId("359DF146-5F3B-47D6-83DA-79C1F3788B25")]
        #endregion
        [Workspace]
        public int Quantity { get; set; }

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
