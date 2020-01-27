// <copyright file="Person.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;
    using Allors.Repository.Attributes;

    public partial class Person : Party, Deletable, Versioned
    {
        #region inherited properties
        public Locale Locale { get; set; }

        public string PartyName { get; set; }

        public ContactMechanism GeneralCorrespondence { get; set; }

        public TelecommunicationsNumber BillingInquiriesFax { get; set; }

        public Qualification[] Qualifications { get; set; }

        public ContactMechanism HomeAddress { get; set; }

        public ContactMechanism SalesOffice { get; set; }

        public PartyContactMechanism[] InactivePartyContactMechanisms { get; set; }

        public TelecommunicationsNumber OrderInquiriesFax { get; set; }

        public Person[] CurrentSalesReps { get; set; }

        public PartyContactMechanism[] PartyContactMechanisms { get; set; }

        public PartyRelationship[] InactivePartyRelationships { get; set; }

        public Person[] CurrentContacts { get; set; }

        public Person[] InactiveContacts { get; set; }

        public TelecommunicationsNumber ShippingInquiriesFax { get; set; }

        public TelecommunicationsNumber ShippingInquiriesPhone { get; set; }

        public BillingAccount[] BillingAccounts { get; set; }

        public TelecommunicationsNumber OrderInquiriesPhone { get; set; }

        public PartySkill[] PartySkills { get; set; }

        public PartyClassification[] PartyClassifications { get; set; }

        public BankAccount[] BankAccounts { get; set; }

        public ContactMechanism BillingAddress { get; set; }

        public EmailAddress GeneralEmail { get; set; }

        public ShipmentMethod DefaultShipmentMethod { get; set; }

        public Resume[] Resumes { get; set; }

        public ContactMechanism HeadQuarter { get; set; }

        public EmailAddress PersonalEmailAddress { get; set; }

        public TelecommunicationsNumber CellPhoneNumber { get; set; }

        public TelecommunicationsNumber BillingInquiriesPhone { get; set; }

        public ContactMechanism OrderAddress { get; set; }

        public ElectronicAddress InternetAddress { get; set; }

        public Media[] Contents { get; set; }

        public CreditCard[] CreditCards { get; set; }

        public PostalAddress ShippingAddress { get; set; }

        public TelecommunicationsNumber GeneralFaxNumber { get; set; }

        public PartyContactMechanism[] CurrentPartyContactMechanisms { get; set; }

        public PartyRelationship[] CurrentPartyRelationships { get; set; }

        public TelecommunicationsNumber GeneralPhoneNumber { get; set; }

        public Currency PreferredCurrency { get; set; }

        public VatRegime VatRegime { get; set; }

        public Agreement[] Agreements { get; set; }

        public PaymentMethod DefaultPaymentMethod { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public string Comment { get; set; }

        public LocalisedText[] LocalisedComments { get; set; }

        public PartyRate[] PartyRates { get; set; }

        public bool CollectiveWorkEffortInvoice { get; set; }

        public UserProfile UserProfile { get; set; }

        #endregion

        #region Versioning
        #region Allors
        [Id("F97D0E2B-C361-42BD-AB01-C99E8EDBAB02")]
        [AssociationId("5647343B-CE74-4330-8F1B-C44452570598")]
        [RoleId("73059957-CF4A-4837-AE96-6B1EE67279B8")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public PersonVersion CurrentVersion { get; set; }

        #region Allors
        [Id("B43008FF-7AEB-4599-BB9D-E112E9C80AD9")]
        [AssociationId("7D02ABA4-8832-4126-BA36-F872120D75E5")]
        [RoleId("956170ED-F0E1-44B2-AC7C-B95A8A1FD6BD")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public PersonVersion[] AllVersions { get; set; }
        #endregion

        #region Allors
        [Id("348dd7c2-c534-422c-90aa-d48b1e504df9")]
        [AssociationId("7516fdc6-10c1-4f61-8a9f-f1d84b1a9899")]
        [RoleId("f0306c09-0b6f-4e73-b789-47c3b3c2b0d6")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Salutation Salutation { get; set; }

        #region Allors
        [Id("4a01889c-ed4f-41f5-8a25-f0e3bbeb095b")]
        [AssociationId("1282318d-0ac0-406b-868c-36176b4b0610")]
        [RoleId("b62f0e23-6928-40b5-abc0-feac01a40e98")]
        #endregion
        [Derived]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal YTDCommission { get; set; }

        #region Allors
        [Id("4de34e8b-6c0e-48e7-9b5a-5390325a13ff")]
        [AssociationId("aa8b50f0-176f-4036-819a-205f68ab6d64")]
        [RoleId("525e2b11-5917-4b21-abdf-15ed74048e38")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Indexed]
        [Workspace]
        public PersonClassification[] PersonClassifications { get; set; }

        #region Allors
        [Id("539b51e6-dd15-481c-86d3-ceb84588c078")]
        [AssociationId("280bf735-be99-4c2e-b867-efbf187d8a67")]
        [RoleId("766470ee-34f8-4a49-8622-28e5f79bea72")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        [Indexed]
        public Citizenship Citizenship { get; set; }

        #region Allors
        [Id("5f5d8dd2-33e6-4924-bae7-b6710a789ac9")]
        [AssociationId("007ba2c5-9fdd-425e-8842-27554cdbaf27")]
        [RoleId("99b8085b-0ccf-44a3-a4d4-e1d091af8969")]
        #endregion
        [Derived]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal LastYearsCommission { get; set; }

        #region Allors
        [Id("634130cf-b466-4ed3-9036-a4a20566c344")]
        [AssociationId("41dd15b0-3525-428d-8af3-7ef4b90b974c")]
        [RoleId("1dd0070d-af81-486e-8897-45727dae950a")]
        #endregion
        [Size(256)]
        [Workspace]
        public string GivenName { get; set; }

        #region Allors
        [Id("6d425613-b821-46f2-896a-a04dc4b377a3")]
        [AssociationId("18b225ef-df54-4fd1-9423-36d334d1d876")]
        [RoleId("128f6659-8313-4c53-8ff5-eb1fcffd1b36")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        public PersonalTitle[] Titles { get; set; }

        #region Allors
        [Id("6f7b0a7f-0b8e-4fbe-b248-b7b90fb18613")]
        [AssociationId("33d02f85-4e00-40ef-821d-19278666b178")]
        [RoleId("1e4214e8-f228-4f6e-8885-29a59fcd19f3")]
        #endregion
        [Size(256)]
        [Workspace]
        public string MothersMaidenName { get; set; }

        #region Allors
        [Id("7bcba7fd-6419-4324-8a11-c56bd46581a1")]
        [AssociationId("78ccda0f-4b17-40f1-93ad-b86e1181cb80")]
        [RoleId("1babd38a-8a52-4a92-bb99-7a289d41bb1e")]
        #endregion
        [Workspace]
        public DateTime BirthDate { get; set; }

        #region Allors
        [Id("a2ace3b0-e38e-49c8-8c4b-0e97672830c4")]
        [AssociationId("e5e9e017-d642-4c03-97c0-f106aff2eff5")]
        [RoleId("55ed88ed-5634-4267-961f-c75469302637")]
        #endregion
        [Indexed]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal Height { get; set; }

        #region Allors
        [Id("ab9b5c70-3d58-4e2b-a140-f8f1a904da51")]
        [AssociationId("45889e13-eba5-4648-8f89-ee161e9335c9")]
        [RoleId("f634cc39-2f16-4dea-958d-bcc56fbe61aa")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        public PersonTraining[] PersonTrainings { get; set; }

        #region Allors
        [Id("b6f28dbd-f20f-44ed-a2e7-476f1a8a5518")]
        [AssociationId("3ddb90b4-84df-4214-818d-7fa05a464815")]
        [RoleId("a609ed29-bde6-4b43-bfe7-4abda8630b90")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public GenderType Gender { get; set; }

        #region Allors
        [Id("d48c94ea-5106-44a2-8eda-959e03480960")]
        [AssociationId("32e77969-92bc-4387-9f93-350eaba42fea")]
        [RoleId("b7789142-9c4a-452c-927b-7de2c7e09e83")]
        #endregion
        [Indexed]
        [Workspace]
        public int Weight { get; set; }

        #region Allors
        [Id("ee6e4476-b1fa-431f-add3-30afe199cdd1")]
        [AssociationId("ffecd512-f3cd-44d1-868e-824fd81e6431")]
        [RoleId("8a302cbf-f784-42d9-a127-55b256895959")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Workspace]
        [Indexed]
        public Hobby[] Hobbies { get; set; }

        #region Allors
        [Id("eeb16852-431b-4b84-983d-559e64af6dfb")]
        [AssociationId("85960e64-77a1-4744-9be5-c1704030247c")]
        [RoleId("6fe6349d-b63f-4387-9a3f-bf83576e0d97")]
        #endregion
        [Workspace]
        public int TotalYearsWorkExperience { get; set; }

        #region Allors
        [Id("f0708d80-a9cf-47be-9bed-76201fe9f17d")]
        [AssociationId("bfce261c-2f85-4be2-97ee-de15d3158b1d")]
        [RoleId("e55b5f5e-931f-40a5-b092-45cdc57fd0ec")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        public Passport[] Passports { get; set; }

        #region Allors
        [Id("f15d6344-e4f4-4b79-a1af-c6a7417af844")]
        [AssociationId("13d296f7-0118-48dc-9f60-cbbdee324ad7")]
        [RoleId("284e3a85-eba6-47f4-b97e-848bf2a163e5")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Workspace]
        public MaritalStatus MaritalStatus { get; set; }

        #region Allors
        [Id("f92c5c86-c32a-41e0-99ff-2d94a8d6ccfa")]
        [AssociationId("0ff499d5-300f-483c-b722-757787c1f4b3")]
        [RoleId("2162765c-5fd8-42aa-85f7-20f0effbc308")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Workspace]
        public Media Picture { get; set; }

        #region Allors
        [Id("fefb8dc2-cfe5-4078-b3a9-8c4622047c34")]
        [AssociationId("7ecef213-f2db-4f79-8bf3-fc0979f81420")]
        [RoleId("e19baff7-c31c-4462-8a48-ac30e862b4ea")]
        #endregion
        [Size(256)]
        [Workspace]
        public string SocialSecurityNumber { get; set; }

        #region Allors
        [Id("ffda06c0-7dff-42fa-abd5-1ed6fa8c43da")]
        [AssociationId("8dabd93a-badc-40f3-96af-f97c1b61d262")]
        [RoleId("92041fa4-b675-4fd4-b6c4-d9143393878e")]
        #endregion
        [Workspace]
        public DateTime DeceasedDate { get; set; }

        #region Allors
        [Id("996809E3-994B-4189-B3A8-77CC1BB99B0A")]
        [AssociationId("8382F44C-D777-4431-828A-0EA8970751C1")]
        [RoleId("8EBB2477-95FB-43CA-9A79-0F83D2449449")]
        #endregion
        [Size(256)]
        [Workspace]
        public string Function { get; set; }

        #region Allors
        [Id("38469571-E9E6-473D-BE7B-FA9DB3B67AA0")]
        [AssociationId("8DDDF680-2208-4827-81AA-C5290E2C9DE0")]
        [RoleId("6B19DAD5-974C-471D-AB73-D8CDE375EFE2")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Workspace]
        public OrganisationContactRelationship[] CurrentOrganisationContactRelationships { get; set; }

        #region Allors
        [Id("C6AC83CB-08BF-46FE-8BC0-D777DFD18BB0")]
        [AssociationId("18E943B5-83BE-4FFD-AD81-6EB8BAAA9162")]
        [RoleId("EA13ED09-1E82-4A57-A95C-31E38F9EE20F")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Workspace]
        public OrganisationContactRelationship[] InactiveOrganisationContactRelationships { get; set; }

        #region Allors
        [Id("042A0F63-EB65-41CD-A32A-A14E99EFE4FB")]
        [AssociationId("5F9AEB37-F54A-4617-A03F-2DD6F4667ACF")]
        [RoleId("B5E1433D-8401-48FC-8334-645F30C9B2E0")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Workspace]
        public ContactMechanism[] CurrentOrganisationContactMechanisms { get; set; }

        #region inherited methods
        public void Delete() { }
        #endregion
    }
}
