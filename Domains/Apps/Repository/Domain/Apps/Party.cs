namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("3bba6e5a-dc2d-4838-b6c4-881f6c8c3013")]
    #endregion
    [Plural("Parties")]
    public partial interface Party : Localised, Auditable, UniquelyIdentifiable, Commentable
    {
        #region Allors
        [Id("BB29E79A-5B37-4CE7-B366-32F67FFD1FA3")]
        [AssociationId("B7DD5FEE-A50F-48FF-B477-E8A635BE0676")]
        [RoleId("5B1C2D30-8FDC-454E-B7E9-E21AD11A2662")]
        #endregion
        string PartyName { get; set; }

        #region Allors
        [Id("008618c4-6252-4643-a0a8-e736f9288946")]
        [AssociationId("5282ba00-8bbc-4086-994d-7e68ce9224b6")]
        [RoleId("ca45e298-96eb-4166-9450-c65344fb9979")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Workspace]
        PostalAddress GeneralCorrespondence { get; set; }

        #region Allors
        [Id("01771db8-e79c-4ce4-9d81-db3675e8708a")]
        [AssociationId("c6dbe58e-fa09-408b-9324-21fcec3b1900")]
        [RoleId("aebbe259-2619-45bb-9751-68f61a230159")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal YTDRevenue { get; set; }

        #region Allors
        [Id("04bc4912-cd23-4b2e-973c-76bbf2f2de8d")]
        [AssociationId("c369193b-d01b-4f82-83f3-27ecaa3d8d58")]
        [RoleId("ef73d811-7d6a-4168-819f-1588b01979e8")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal LastYearsRevenue { get; set; }

        #region Allors
        [Id("130d6e94-51e2-45f9-82d7-380ae7c8aa44")]
        [AssociationId("68c1c826-9915-4f7b-8a44-dc62e215b260")]
        [RoleId("e47aa296-12fa-45f1-8deb-0f151aaaba60")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Workspace]
        TelecommunicationsNumber BillingInquiriesFax { get; set; }

        #region Allors
        [Id("19c8a5a0-9567-4fc2-bfad-94a549cfa191")]
        [AssociationId("b8622d0f-ba18-4a76-b1d9-25115378c01c")]
        [RoleId("6656341b-4b2a-41a3-abad-9aece1294b79")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        Qualification[] Qualifications { get; set; }

        #region Allors
        [Id("1ad85fce-f2f8-45aa-bf1e-8f5ade34153c")]
        [AssociationId("20dd50d2-06c8-48e8-883d-5f894c973834")]
        [RoleId("e3834580-66fc-4b4d-b0fa-58e19f660316")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Workspace]
        ContactMechanism HomeAddress { get; set; }

        #region Allors
        [Id("1bf7b758-2b58-4f82-a6a1-a8d5991d3d9d")]
        [AssociationId("240a4c51-86f3-47c7-a28d-7c8fd7b5d68e")]
        [RoleId("08655bdf-9abb-404d-a4d4-739896199bc3")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Indexed]
        [Workspace]
        OrganisationContactRelationship[] InactiveOrganisationContactRelationships { get; set; }

        #region Allors
        [Id("1d4e59a6-253f-470e-b9a7-c2c73b67cf2f")]
        [AssociationId("996ea544-3d27-410d-aa23-25457532e3b1")]
        [RoleId("90f0a491-c7c7-4ff5-9910-77d430f6292a")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Workspace]
        ContactMechanism SalesOffice { get; set; }

        #region Allors
        [Id("245eaa78-39d9-404f-a4da-ad3718cfc0ca")]
        [AssociationId("69b7d750-b476-4857-8c34-c335d32e39bc")]
        [RoleId("9b797592-a08b-4c39-aa2d-38d8ceb015bb")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Indexed]
        [Workspace]
        Person[] InactiveContacts { get; set; }

        #region Allors
        [Id("25068a0e-15f7-41bd-b16d-a7dd51ca9aa3")]
        [AssociationId("f37e7ab6-e6c2-4a51-bbb9-bfffab638084")]
        [RoleId("7fd1e04c-fa8e-40fb-bb8d-e4b26d0c3895")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Derived]
        [Indexed]
        [Workspace]
        PartyContactMechanism[] InactivePartyContactMechanisms { get; set; }

        #region Allors
        [Id("29da9212-a70f-4ee6-98d7-508687faa2b4")]
        [AssociationId("6798142d-fefe-40a1-86c2-7788e1961fcb")]
        [RoleId("895e8823-ae01-41d9-b0d1-055fadf45c71")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Workspace]
        TelecommunicationsNumber OrderInquiriesFax { get; set; }

        #region Allors
        [Id("420a7279-ba09-4660-bf5d-7242be07bfb1")]
        [AssociationId("3bb65209-69d2-40e5-890b-c8a9e06da1ac")]
        [RoleId("8f1be044-6b43-4861-b995-fdc080656670")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Indexed]
        [Workspace]
        Person[] CurrentSalesReps { get; set; }

        #region Allors
        [Id("42ab0c4b-52b2-494e-b6a9-cacf55fb002e")]
        [AssociationId("32d52b42-f5cc-4fd0-959c-045ff0c02520")]
        [RoleId("977a3626-85af-47a8-bfe8-ed2e8daa1d9e")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        PartyContactMechanism[] PartyContactMechanisms { get; set; }

        #region Allors
        [Id("436f0ef1-a3ea-439c-9ffd-211c177f5ed1")]
        [AssociationId("1b9df170-befb-46e9-ba07-5a1b4b77e150")]
        [RoleId("a1f5ff98-c126-47f8-b5f6-72180319a847")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Workspace]
        TelecommunicationsNumber ShippingInquiriesFax { get; set; }

        #region Allors
        [Id("4444b0d1-4ade-4fed-88bf-ce9ef275a978")]
        [AssociationId("94602440-bdea-4b49-9fe3-15b0d483c632")]
        [RoleId("9d65c05b-562b-4b31-b717-4247b8086f5b")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Workspace]
        TelecommunicationsNumber ShippingInquiriesPhone { get; set; }

        #region Allors
        [Id("4a46f6aa-d4f9-4e5e-ac17-d77ab0e99c3f")]
        [AssociationId("ba75f426-3a2a-4341-ac95-3562c608d83b")]
        [RoleId("9dd1757a-f31e-4fe1-9195-0a8403f0108a")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        BillingAccount[] BillingAccounts { get; set; }


        #region Allors
        [Id("4d742fa8-f10b-423e-9341-f8a526838eba")]
        [AssociationId("bd9d5e4f-8c3a-4787-8c5a-1e3f9f49db97")]
        [RoleId("f9bcbb5a-6c10-4fa9-8601-82c6fb941f3b")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Workspace]
        TelecommunicationsNumber OrderInquiriesPhone { get; set; }

        #region Allors
        [Id("4e725bd6-2280-48a2-be89-836b4bd7d002")]
        [AssociationId("9d7f6130-f2ba-4da0-9b74-91b6205e42be")]
        [RoleId("eb6079ed-489b-4673-8508-7a9a6e33573f")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        PartySkill[] PartySkills { get; set; }

        #region Allors
        [Id("4e787cf8-9b92-4ab2-8f88-c08bdb90a376")]
        [AssociationId("66778fc1-8d7c-4976-afe1-e07fd4567c46")]
        [RoleId("766900b8-646c-4b59-b022-5143cf5e5ce9")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Indexed]
        [Workspace]
        PartyClassification[] PartyClassifications { get; set; }

        #region Allors
        [Id("52863081-34b7-48e2-a7ff-c6bd67172350")]
        [AssociationId("7ab0f4b0-f4ae-45d4-8c9e-a576f36e4f1a")]
        [RoleId("09d4533e-d118-4395-a7f1-358aad00f6e4")]
        #endregion
        [Workspace]
        bool ExcludeFromDunning { get; set; }

        #region Allors
        [Id("52dd7bf8-bb7e-47bd-85b3-f35fba964e5c")]
        [AssociationId("3eac7011-d5ed-46ce-a678-b1e3a6c02962")]
        [RoleId("fb2c26d4-c23c-4817-94ee-5f2acebb4e41")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        BankAccount[] BankAccounts { get; set; }

        #region Allors
        [Id("59500ed1-2de5-45ff-bec7-275c1941d153")]
        [AssociationId("bd699a2c-e1dc-48dd-9d0a-c1aec3b18f44")]
        [RoleId("9501b51f-92e1-4ab8-862b-c6b6fd469b68")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Indexed]
        [Workspace]
        Person[] CurrentContacts { get; set; }

        #region Allors
        [Id("70ada4aa-c51c-4f1d-a3d2-ea6de31cb988")]
        [AssociationId("9f1ea588-8dd9-4f48-a905-0271e694f1fe")]
        [RoleId("f2455f15-83f5-4599-9b2e-c1b8d9b92995")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Workspace]
        ContactMechanism BillingAddress { get; set; }


        #region Allors
        [Id("78cc2859-b815-453f-9bdc-17fe64a853c4")]
        [AssociationId("746d59e4-1d66-4d63-a680-29c7b858421a")]
        [RoleId("cd885abe-ebaa-47ab-8fac-87e928b478c1")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Workspace]
        ElectronicAddress GeneralEmail { get; set; }

        #region Allors
        [Id("79a5c25a-91e9-4a80-8649-c8abe86e47dd")]
        [AssociationId("39d03d8f-8fbc-4131-8e97-7f5fcf73871c")]
        [RoleId("711fc18b-b5f8-4235-8a51-22f91e4c194e")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        ShipmentMethod DefaultShipmentMethod { get; set; }

        #region Allors
        [Id("7dc1e326-76ef-4bac-aae1-d6a26da9d40a")]
        [AssociationId("5b8c7f22-121d-473f-83e0-41f20740b912")]
        [RoleId("468e863c-79f9-48a1-a28e-ad6159940b01")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        Resume[] Resumes { get; set; }


        #region Allors
        [Id("89971e75-61e5-4a0c-b7fc-6f4c15866175")]
        [AssociationId("ef2f1c0e-ecc2-4949-aec9-88460c0d5b0b")]
        [RoleId("d80cc262-207a-462e-b8ed-ee58f04cf98b")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Workspace]
        ContactMechanism HeadQuarter { get; set; }

        #region Allors
        [Id("90590830-da80-4afd-ac37-e9fafb59493a")]
        [AssociationId("79b4d3ba-70cc-4914-82ee-d06e11ac7b2c")]
        [RoleId("71133938-89e1-45f6-8e5e-6ef699d44db1")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Derived]
        [Indexed]
        [Workspace]
        ElectronicAddress PersonalEmailAddress { get; set; }

        #region Allors
        [Id("92c99262-30ed-4265-975b-07140c46af6e")]
        [AssociationId("71b74bf9-8b50-4f81-9f52-0a06cc223ba9")]
        [RoleId("bc0d1d88-3811-4fdf-b1c7-4ad2d82230cf")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Workspace]
        TelecommunicationsNumber CellPhoneNumber { get; set; }

        #region Allors
        [Id("95f6db56-0dcf-4d5e-8e81-43e0d72faa85")]
        [AssociationId("d47edd54-4d98-428d-9cb9-d57e0e7816f1")]
        [RoleId("14ed3b75-2787-4abf-be44-408ca2945384")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Workspace]
        TelecommunicationsNumber BillingInquiriesPhone { get; set; }

        #region Allors
        [Id("a7720655-a6c1-4f54-a093-b77da985ac5f")]
        [AssociationId("4f9183c0-bac1-4738-97e3-15c2906759e8")]
        [RoleId("d1e7a633-f097-4030-b3c3-9167c022fe05")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Workspace]
        ContactMechanism OrderAddress { get; set; }

        #region Allors
        [Id("ac5a48dc-4115-489a-aa8c-f43268b6bfe3")]
        [AssociationId("97686b93-4c5f-4544-af6a-acacca008060")]
        [RoleId("bf8f9ba5-7a88-4ad4-b154-09b5efae9912")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Workspace]
        ElectronicAddress InternetAddress { get; set; }

        #region Allors
        [Id("acf731ab-c856-4553-a2fc-9f88e3ccc258")]
        [AssociationId("c75a6014-98bd-4e2f-b526-1e2cfda9534c")]
        [RoleId("87d94438-3756-42cd-9356-9d169ce42817")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        Media[] Contents { get; set; }

        #region Allors
        [Id("aecedf16-9e42-4e49-b7ec-e92187262405")]
        [AssociationId("41d4ebe2-dcf3-4517-9ce5-2c1dcc45400d")]
        [RoleId("af648b7c-4407-46b8-8070-76d86a48c605")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        CreditCard[] CreditCards { get; set; }

        #region Allors
        [Id("c20f82fa-3ba2-4e84-beef-52ba30c92695")]
        [AssociationId("0c9edf90-b6fd-476b-86e8-ca1b845ee62b")]
        [RoleId("5da6410e-1311-4664-a0b3-ee2fca4b9ad1")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Workspace]
        PostalAddress ShippingAddress { get; set; }

        #region Allors
        [Id("c79070fc-2c7d-440b-80ce-f86796c59a14")]
        [AssociationId("8bb86356-0b10-4e77-bbbb-d4d33230c3a9")]
        [RoleId("8c72ca39-b408-4623-8a90-54c3b3630e6b")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Indexed]
        [Workspace]
        OrganisationContactRelationship[] CurrentOrganisationContactRelationships { get; set; }

        #region Allors
        [Id("d05ee314-57be-4852-a3b5-62710df4d4b7")]
        [AssociationId("87821f12-6fed-4376-b239-6d2296457b88")]
        [RoleId("a3a1df78-5469-41ae-bdc5-24c340abc378")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal OpenOrderAmount { get; set; }

        #region Allors
        [Id("d562d1f0-1f8f-40c5-a346-ae32e498f332")]
        [AssociationId("8dab565f-7386-4037-843f-bfc3603b27ab")]
        [RoleId("10c1c77e-4b1b-4fd5-b77f-95e8897a4b38")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Workspace]
        TelecommunicationsNumber GeneralFaxNumber { get; set; }

        #region Allors
        [Id("d97ab83b-85dc-4877-8b49-1e552489bcb0")]
        [AssociationId("4af97ea0-bb6b-4fdb-9e0d-798805ccad53")]
        [RoleId("9c644a11-4239-49df-b603-489c547e2085")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        PaymentMethod DefaultPaymentMethod { get; set; }

        #region Allors
        [Id("e16b9c8f-cb53-4d58-aa13-ac92d5de1465")]
        [AssociationId("5476eeb1-246c-43e0-9939-2263dfead9a5")]
        [RoleId("3c3c6c4a-3b7b-4b3f-a843-fa5c334f33fb")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Derived]
        [Indexed]
        [Workspace]
        PartyContactMechanism[] CurrentPartyContactMechanisms { get; set; }

        #region Allors
        [Id("e2017090-fa3f-420e-a5c5-6a2f5aaacd2f")]
        [AssociationId("84c30383-6d26-4abe-92a3-d750e41d2561")]
        [RoleId("51170ba2-d717-41dc-9d6b-18967c37e751")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Workspace]
        TelecommunicationsNumber GeneralPhoneNumber { get; set; }

        #region Allors
        [Id("f0de022f-b94e-4d29-8cdf-99d39ad9add6")]
        [AssociationId("81236f57-51e8-4863-b796-419685199990")]
        [RoleId("a736d5be-33ec-4449-a23d-b4a83a0f4bc3")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        Currency PreferredCurrency { get; set; }

        #region Allors
        [Id("fafa35a1-7762-47f7-a9c2-28d3d0623e7c")]
        [AssociationId("ef3ddd5a-7f11-4191-8098-18fa958f7f93")]
        [RoleId("68f80581-9c1f-4f02-88dc-e6119ab6d135")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        VatRegime VatRegime { get; set; }

        #region Allors
        [Id("B66127AF-F490-4BF0-B8F5-6EBA878B1314")]
        [AssociationId("BBCAA840-593F-4155-8643-2E2FBA3E0035")]
        [RoleId("DF84CED1-8A60-43BF-A1A0-4C971F81C0B6")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        decimal SimpleMovingAverage { get; set; }

        #region Allors
        [Id("42e3b2c4-376d-4e8b-bb49-2af031881ed0")]
        [AssociationId("bcdd31e8-8101-4b6b-8f13-a4397c43adfa")]
        [RoleId("a9ddfe04-e5fd-4b22-9a9a-702dc0533731")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal AmountOverDue { get; set; }

        #region Allors
        [Id("76b46019-c145-403d-9f99-cd8e1001c968")]
        [AssociationId("6702ba13-81eb-4d23-b341-8fb84cf7e60f")]
        [RoleId("079e6188-73d0-4161-8327-607554a42613")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        DunningType DunningType { get; set; }

        #region Allors
        [Id("894f4ff2-9c41-4201-ad36-ac10dafd65dd")]
        [AssociationId("c8a336f0-4fae-4ce6-a900-283066052ffd")]
        [RoleId("11fa6c6e-c528-452c-adca-75f474d2f95b")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal AmountDue { get; set; }

        #region Allors
        [Id("af50ade8-5964-4963-819d-c87689c6434e")]
        [AssociationId("a06dda1c-d91d-4e27-b293-05cb53de65ec")]
        [RoleId("7f6da6ca-b069-47f6-983c-6e33d65ffd0e")]
        #endregion
        [Workspace]
        DateTime LastReminderDate { get; set; }

        #region Allors
        [Id("dd59ed76-b6da-49a3-8c3e-1edf4d1d0900")]
        [AssociationId("e2afe553-7bbd-4f81-97e8-7279defb49ca")]
        [RoleId("b5e30743-6adc-4bf0-b547-72b17b79879c")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal CreditLimit { get; set; }

        #region Allors
        [Id("e3a06a1c-998a-4871-8f0e-2f166eac6c7b")]
        [AssociationId("08dfdeb5-1a62-42d6-b8f3-16025960b09f")]
        [RoleId("9400c681-2a68-4842-89fd-3c9ccb3f2a96")]
        #endregion
        [Required]
        [Workspace]
        int SubAccountNumber { get; set; }

        #region Allors
        [Id("ee871786-8840-404d-9b41-932a9f59be13")]
        [AssociationId("5b98959d-5589-4958-a86f-4c9b465c1632")]
        [RoleId("056ca61a-1ab4-4e53-8d5f-328ada5f3b11")]
        #endregion
        [Workspace]
        DateTime BlockedForDunning { get; set; }

        #region Allors
        [Id("DEC610BF-5051-44EA-B4F9-1D912DCD19D5")]
        [AssociationId("4F0A52D4-886A-4348-B0BC-50461BBABDD9")]
        [RoleId("F74DD0DF-CC88-4FA5-90CC-968CC82CA65B")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        Agreement[] Agreements { get; set; }

        #region Allors
        [Id("BA00C20D-E4A0-4678-A3D4-C24344213DEC")]
        [AssociationId("B6C0F82C-CE97-4F72-9C3D-7CF220C4F9D5")]
        [RoleId("CA30D198-DBD5-47CD-AB1F-72149A20097D")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        CommunicationEvent[] CommunicationEvents { get; set; }
    }
}