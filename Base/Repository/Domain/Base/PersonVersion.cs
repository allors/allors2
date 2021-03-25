// <copyright file="PersonVersion.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;
    using Allors.Repository.Attributes;

    #region Allors
    [Id("E42FCDBF-5DEF-4743-BDE8-4028AC6A00A5")]
    #endregion

    public partial class PersonVersion : PartyVersion
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid DerivationId { get; set; }

        public DateTime DerivationTimeStamp { get; set; }

        public string Comment { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public string PartyName { get; set; }

        public ContactMechanism GeneralCorrespondence { get; set; }

        public TelecommunicationsNumber BillingInquiriesFax { get; set; }

        public Qualification[] Qualifications { get; set; }

        public ContactMechanism HomeAddress { get; set; }

        public ContactMechanism SalesOffice { get; set; }

        public TelecommunicationsNumber OrderInquiriesFax { get; set; }

        public PartyContactMechanism[] PartyContactMechanisms { get; set; }

        public TelecommunicationsNumber ShippingInquiriesFax { get; set; }

        public TelecommunicationsNumber ShippingInquiriesPhone { get; set; }

        public BillingAccount[] BillingAccounts { get; set; }

        public TelecommunicationsNumber OrderInquiriesPhone { get; set; }

        public PartySkill[] PartySkills { get; set; }

        public PartyClassification[] PartyClassifications { get; set; }

        public BankAccount[] BankAccounts { get; set; }

        public ContactMechanism BillingAddress { get; set; }

        public ElectronicAddress GeneralEmail { get; set; }

        public ShipmentMethod DefaultShipmentMethod { get; set; }

        public Resume[] Resumes { get; set; }

        public ContactMechanism HeadQuarter { get; set; }

        public ElectronicAddress PersonalEmailAddress { get; set; }

        public TelecommunicationsNumber CellPhoneNumber { get; set; }

        public TelecommunicationsNumber BillingInquiriesPhone { get; set; }

        public ContactMechanism OrderAddress { get; set; }

        public ElectronicAddress InternetAddress { get; set; }

        public Media[] Contents { get; set; }

        public CreditCard[] CreditCards { get; set; }

        public PostalAddress ShippingAddress { get; set; }

        public TelecommunicationsNumber GeneralFaxNumber { get; set; }

        public TelecommunicationsNumber GeneralPhoneNumber { get; set; }

        public Currency PreferredCurrency { get; set; }

        public PartyRate[] PartyRates { get; set; }
        #endregion

        #region Allors
        [Id("57345BFB-05EC-429F-88AF-BEE30408B121")]
        [AssociationId("1225C556-BB23-433C-9072-0AAA792BBDCE")]
        [RoleId("EE484B8C-2FB4-4ECE-9368-DDE8BF3BE2D9")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Salutation Salutation { get; set; }

        #region Allors
        [Id("0C856758-192D-41E8-83CA-9471D41AA832")]
        [AssociationId("DDAD2C2A-DEA1-4527-9AAE-8415CD70827D")]
        [RoleId("4C310BE9-41A1-40C8-9553-D74D96E46BEA")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Workspace]
        public PersonClassification[] PersonClassifications { get; set; }

        #region Allors
        [Id("A0025652-7D6B-4AD1-B2A5-93C15D13D427")]
        [AssociationId("C3ED19F4-5F41-4709-9A9A-08D43C2719C0")]
        [RoleId("1E010A19-9102-462A-8F40-F6CB08321E89")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public Citizenship Citizenship { get; set; }

        #region Allors
        [Id("1EB80365-57C9-4C63-B7A3-A1A956C098B1")]
        [AssociationId("0649BBD9-84DA-4403-ADE8-04F276644699")]
        [RoleId("438A2CA4-4C7B-4627-AF0B-7B89B1F4E607")]
        #endregion
        [Size(256)]
        [Workspace]
        public string GivenName { get; set; }

        #region Allors
        [Id("69E4038A-10F6-4623-A68D-550206F7A2FE")]
        [AssociationId("594F6B54-D8C6-4CFA-961B-C25565B7A4EB")]
        [RoleId("A5999830-EF35-4D3E-8D9C-DC3F75C76EB4")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Workspace]
        public PersonalTitle[] Titles { get; set; }

        #region Allors
        [Id("4E7EEAE8-AA25-49ED-858D-ABB9F63FEF9C")]
        [AssociationId("641654FA-F7EA-4864-9B71-51A91DBEF8F6")]
        [RoleId("EC9A3BD3-6BE2-445C-9DCA-2F39BBCBDEF1")]
        #endregion
        [Size(256)]
        [Workspace]
        public string MothersMaidenName { get; set; }

        #region Allors
        [Id("DCF909FD-CB0D-4808-8637-2280164F2808")]
        [AssociationId("9D8B5DC0-8D74-4F2A-AC55-B72B78DF127A")]
        [RoleId("01C6D058-079F-45FC-B438-860202CC1D42")]
        #endregion
        [Workspace]
        public DateTime BirthDate { get; set; }

        #region Allors
        [Id("7D028617-E3ED-4532-B49B-4B0F04C14EB1")]
        [AssociationId("632A5845-1B4E-41BC-AF45-084C6203451C")]
        [RoleId("5FEDA22B-2538-4116-A94B-6E6FE781BBCD")]
        #endregion
        [Indexed]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal Height { get; set; }

        #region Allors
        [Id("FF747CE7-24CB-4CA2-A407-DAFBA3CD2AD8")]
        [AssociationId("46B7F0E1-C960-4E80-B1D5-13A101598D9F")]
        [RoleId("D50DEE90-203B-4D89-8931-8FE7EFD0CEDA")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Workspace]
        public PersonTraining[] PersonTrainings { get; set; }

        #region Allors
        [Id("C681DA3A-DC8C-4048-B7AE-9B7FF510DA65")]
        [AssociationId("E09F2CBC-8C58-4174-87B8-73EE384ECF3F")]
        [RoleId("D09B0D2A-8946-4F2A-AAC3-E2317F6F69C7")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public GenderType Gender { get; set; }

        #region Allors
        [Id("B18564AD-D2A6-46CB-989F-8777E8AE7191")]
        [AssociationId("99E3A6C9-67CC-4EFA-862B-FD1A70D4970D")]
        [RoleId("71D33EF4-4049-4105-BCA9-2545314E8A40")]
        #endregion
        [Indexed]
        [Workspace]
        public int Weight { get; set; }

        #region Allors
        [Id("C85647ED-5173-47CC-A86F-8C3770A53124")]
        [AssociationId("33897C93-B097-46DD-B983-67332DA918D3")]
        [RoleId("A8DB076D-A8CC-4E70-9A82-2DF7AA0E88CB")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Workspace]
        public Hobby[] Hobbies { get; set; }

        #region Allors
        [Id("B9C6121A-FE18-4FD9-BC47-05716061B7A8")]
        [AssociationId("E897F556-812E-497C-95E0-3292AA45436A")]
        [RoleId("F4A3A132-2F33-428C-9434-0C74C7D5E4E3")]
        #endregion
        [Workspace]
        public int TotalYearsWorkExperience { get; set; }

        #region Allors
        [Id("E141FDC1-29C9-494F-A6C8-BCA76D0FB364")]
        [AssociationId("9AA416D2-E5AD-4F0F-A3D9-45E3E6425CAD")]
        [RoleId("95220E00-6231-479E-ACF9-43BF948929C3")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public Passport[] Passports { get; set; }

        #region Allors
        [Id("FC3AD9FF-B460-41E7-8433-1B7273D62A3A")]
        [AssociationId("11A50843-E6AD-4003-AAB2-F09DBF6DE386")]
        [RoleId("EF64158F-9E38-4E24-90A2-77F114C28BCB")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public MaritalStatus MaritalStatus { get; set; }

        #region Allors
        [Id("9A2F5050-FA3A-45FF-9363-8A6B2123D35D")]
        [AssociationId("11A7107C-152B-4195-B548-3DA74BC78322")]
        [RoleId("4DA2003A-241E-4D7D-B4CE-07DE1E88A8D4")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public Media Picture { get; set; }

        #region Allors
        [Id("3B0FDE6C-31C0-4633-8083-B01D5EEB7FF1")]
        [AssociationId("58FFA60F-DEDF-466A-BB69-D70763BEE89A")]
        [RoleId("556FE2D1-4E0C-4220-9139-62A4BA2339EB")]
        #endregion
        [Size(256)]
        [Workspace]
        public string SocialSecurityNumber { get; set; }

        #region Allors
        [Id("34104E71-74E3-4BFD-ADE5-CE4BD79A8A0A")]
        [AssociationId("2FF9ED04-1FE5-4BA4-9AC8-0D3C77D9718C")]
        [RoleId("512BBDA2-426D-43C8-AA7B-8CE65D03A129")]
        #endregion
        [Workspace]
        public DateTime DeceasedDate { get; set; }

        #region Allors
        [Id("7F28B1AB-7978-444C-9CB6-3E11F739234C")]
        [AssociationId("A8CAC069-C0BF-4618-B777-9D152B844C70")]
        [RoleId("C114BC4B-8730-46AC-9245-6879D08D4100")]
        #endregion
        [Size(256)]
        [Workspace]
        public string Function { get; set; }

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
