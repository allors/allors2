namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("FA4FC65D-8B26-434D-95C9-06991EAA0B57")]
    #endregion
    public partial interface PartyVersion : Version
    {
        #region Allors
        [Id("A2915A2C-0D81-4BE3-8EEA-193692351F52")]
        [AssociationId("00783FDB-0A42-48C0-A87C-BA09D772F763")]
        [RoleId("0F474204-92EA-4AE9-88A9-82E708A3ED2D")]
        #endregion
        [Size(-1)]
        [Workspace]
        string Comment { get; set; }

        #region Allors
        [Id("089768C8-5084-4917-8B21-3B185B9FADE6")]
        [AssociationId("10043F9A-83FF-44CA-9498-996976D75C6F")]
        [RoleId("561E4573-8A12-4CF0-912F-819A96D716FE")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        User CreatedBy { get; set; }

        #region Allors
        [Id("545C3AE5-56EE-449C-8FDB-D8F321C72C16")]
        [AssociationId("854BD05D-A726-41E1-9E48-D38EC969BBB7")]
        [RoleId("3C22C9AB-AD24-4D2E-8BE9-F41C85AC6586")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        User LastModifiedBy { get; set; }

        #region Allors
        [Id("2BAD0A81-70F0-44DF-8539-280A34206DF6")]
        [AssociationId("7271180A-C29E-4483-84BB-E96E303C33FF")]
        [RoleId("0003AED3-6CB2-4796-A0E0-B73A016CEE72")]
        #endregion
        [Workspace]
        DateTime CreationDate { get; set; }

        #region Allors
        [Id("DABBBD97-E832-4FB1-BA59-4F6988022ACE")]
        [AssociationId("573A1AEB-4768-4ABC-A4A5-E654CA58FB1D")]
        [RoleId("442EF695-F7D8-49F4-9BF4-308B1ED296BF")]
        #endregion
        [Workspace]
        DateTime LastModifiedDate { get; set; }

        #region Allors
        [Id("5079430B-3610-4906-8F58-B34D6DCD1832")]
        [AssociationId("F70F4B10-9E44-4E9D-8AD5-5E5A9869EBD3")]
        [RoleId("B5315617-AC94-4572-8E4C-00329E69A5E3")]
        [Workspace]
        #endregion
        string PartyName { get; set; }

        #region Allors
        [Id("3F2F3296-7DA7-48F7-B172-16904F47FA5F")]
        [AssociationId("529FD0F0-1098-41E7-A935-0D8E4FDCFDE0")]
        [RoleId("2B482D0B-4E5F-4DEA-8B53-87DE14EF2872")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        PostalAddress GeneralCorrespondence { get; set; }

        #region Allors
        [Id("D6319E25-4893-429E-91DD-F7D53A388A9F")]
        [AssociationId("72810DB1-9F63-42E4-84E4-E7FF78923527")]
        [RoleId("086B65CC-AD80-4C91-969F-A5253C91332E")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal YTDRevenue { get; set; }

        #region Allors
        [Id("8DA92A3D-522E-4AED-AC32-96D77DAD2378")]
        [AssociationId("87E94A98-5C02-4632-8549-10524BAD8236")]
        [RoleId("7F8D85C6-AB23-41A9-B343-36AB0B6EC609")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal LastYearsRevenue { get; set; }

        #region Allors
        [Id("27A64036-F3FD-4F7A-BF11-5AC3F032C943")]
        [AssociationId("8F8B670D-1A14-4AD2-8FB4-848DAE017127")]
        [RoleId("13AE7FD6-7F18-4CD3-AAEA-81CB1EDCECF0")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        TelecommunicationsNumber BillingInquiriesFax { get; set; }

        #region Allors
        [Id("70B38198-DA16-4764-88A6-DF491E376001")]
        [AssociationId("68FF432E-FD81-44FF-88AE-4EE232538F42")]
        [RoleId("85D4DA1E-26AD-4CB2-BE18-E3C11D12008C")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Workspace]
        Qualification[] Qualifications { get; set; }

        #region Allors
        [Id("352932DD-2804-43D7-A660-C478A14E35D7")]
        [AssociationId("DCE6A39A-D290-4522-988D-5FEA4CAD3A88")]
        [RoleId("000365DD-FD0B-42FF-9547-D7A37E22EEEE")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        ContactMechanism HomeAddress { get; set; }

        #region Allors
        [Id("9F8D0B9B-94EE-4BB8-AC65-D82E29C3DA60")]
        [AssociationId("453E8252-D6AF-4B55-B9A3-C2ECF6FB34E3")]
        [RoleId("CE5B3AD5-27C4-4553-993A-8A70677A3110")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Workspace]
        OrganisationContactRelationship[] InactiveOrganisationContactRelationships { get; set; }

        #region Allors
        [Id("5CC1C591-1C3D-4E09-A4CA-05E224C3EC70")]
        [AssociationId("6513D6D8-D71A-46D6-ACF2-721A86BEA211")]
        [RoleId("BB1A6C3F-91E9-4DA0-A3F9-C4FEE38FA2E0")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        ContactMechanism SalesOffice { get; set; }

        #region Allors
        [Id("9132EF8F-6F45-4CD3-8426-CFB75B86F3BC")]
        [AssociationId("DC1CE14C-39AB-4BE6-AE66-54C01AC65B2C")]
        [RoleId("DFEDBA0B-07DD-48EB-AC69-830DC1FC9E3B")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Workspace]
        Person[] InactiveContacts { get; set; }

        #region Allors
        [Id("6B66A621-37E4-40A9-8153-7C8DB6D53169")]
        [AssociationId("708BE485-0B66-4F54-AEBD-E27275A23316")]
        [RoleId("F033E191-93B6-4FFB-B4DA-2665F59FD275")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Derived]
        [Workspace]
        PartyContactMechanism[] InactivePartyContactMechanisms { get; set; }

        #region Allors
        [Id("33E1CF67-EA30-4DB6-BF95-AA1549D72C0D")]
        [AssociationId("BC3E7743-84E5-411D-9628-FA65E67B6E13")]
        [RoleId("3D64AE1C-08B9-4834-8B4D-14C9E9ABF127")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        TelecommunicationsNumber OrderInquiriesFax { get; set; }

        #region Allors
        [Id("060F4F6C-58B5-4958-9E30-FB91AFC9543E")]
        [AssociationId("D7C1305C-DC9A-42E0-8040-3D05A99A4820")]
        [RoleId("80F6272D-8ECC-49C4-8B51-9E422FE9DFA6")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Workspace]
        Person[] CurrentSalesReps { get; set; }

        #region Allors
        [Id("6AF7A276-4C4E-4492-A26B-3BFDF38A9BE7")]
        [AssociationId("0D4E5BBE-03A5-41B9-888E-EA49658B70FF")]
        [RoleId("6C29DFDF-439D-4417-9C86-399A90DB93E1")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        PartyContactMechanism[] PartyContactMechanisms { get; set; }

        #region Allors
        [Id("599ED873-0645-466F-9156-F14FC42CE14D")]
        [AssociationId("A87146BC-D5F0-482F-BD0A-AFCD0DE9ABF4")]
        [RoleId("B628266A-7675-4A41-8B36-17D840170ECE")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        TelecommunicationsNumber ShippingInquiriesFax { get; set; }

        #region Allors
        [Id("1412C759-965B-45CF-AE03-2699428C082F")]
        [AssociationId("170EE3B3-2478-4440-A5CF-C9DE0FD513DA")]
        [RoleId("EDADD65F-ABF5-4FD4-A122-EB7B8D7B8568")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        TelecommunicationsNumber ShippingInquiriesPhone { get; set; }

        #region Allors
        [Id("67AD2282-2742-4335-9CC2-A0ADA1464075")]
        [AssociationId("8C10771B-87BD-4A62-977B-B2CE49C906CD")]
        [RoleId("C66B9F1B-6DEF-4C19-BF85-378BE1C3E42B")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        BillingAccount[] BillingAccounts { get; set; }

        #region Allors
        [Id("ACCE4AF5-3A14-4C33-AE5D-055A7AE89D6C")]
        [AssociationId("A95E0330-4A14-46ED-951E-2417624FB0BA")]
        [RoleId("1B80C6D3-C355-461E-A05D-8F48726B4EFD")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        TelecommunicationsNumber OrderInquiriesPhone { get; set; }

        #region Allors
        [Id("5FBED0ED-AF61-42D0-B78D-E9D19F29B70F")]
        [AssociationId("7A990A16-05A7-438E-B460-772ECC0722C2")]
        [RoleId("D41E6F44-D2B4-481C-B11D-B71854167195")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Workspace]
        PartySkill[] PartySkills { get; set; }

        #region Allors
        [Id("7B55B29E-497A-470C-8E91-19782B4E1FA4")]
        [AssociationId("A0553A9E-25CE-47CB-88B5-DC744EFA4C95")]
        [RoleId("DBCE5675-5E31-4F8E-A808-5CF29E0CEC13")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Workspace]
        PartyClassification[] PartyClassifications { get; set; }

        #region Allors
        [Id("BC13BA0F-FE1E-4701-AB98-F5E7009B91B2")]
        [AssociationId("B9117347-0EAF-43DC-96FC-C95194286469")]
        [RoleId("DD68BCB4-1D58-4B2C-A171-1E76CB87F224")]
        #endregion
        [Workspace]
        bool ExcludeFromDunning { get; set; }

        #region Allors
        [Id("36BB4124-03C3-429D-AD01-D31BFCF81CCB")]
        [AssociationId("94779C4C-E577-4B28-9A28-DFEE2F13A30F")]
        [RoleId("338BCBB2-D1DD-4CBA-92B3-24773879C6DD")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        BankAccount[] BankAccounts { get; set; }

        #region Allors
        [Id("E3518B88-974C-483F-AD7F-233EE6064D74")]
        [AssociationId("5DD9EB48-447A-4B0C-8FBE-45A4EC41F19E")]
        [RoleId("AEB7A095-9EEB-4605-8CEB-C873E2EA0A34")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Workspace]
        Person[] CurrentContacts { get; set; }

        #region Allors
        [Id("25733706-8788-42E5-9EBA-41D2F5179438")]
        [AssociationId("682E49E8-B22F-4C15-9CB3-EDD447210D5F")]
        [RoleId("A420EFB5-D03B-4CB4-BD45-324F4B588E5D")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        ContactMechanism BillingAddress { get; set; }

        #region Allors
        [Id("51D21BDF-F8C5-4486-851C-C0506C6E6CAF")]
        [AssociationId("48F8FEB2-D4BA-43FB-B74C-D141AD03518E")]
        [RoleId("B27A2669-C7A4-4BE1-B3DA-45AB7C260ECA")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        ElectronicAddress GeneralEmail { get; set; }

        #region Allors
        [Id("C58443ED-7877-4BCA-B8D1-CD109E809220")]
        [AssociationId("60C1FDCE-5BF3-474F-B926-286F393B8D83")]
        [RoleId("ED34553E-5328-474A-BE34-5E8FA1DCAC31")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        ShipmentMethod DefaultShipmentMethod { get; set; }

        #region Allors
        [Id("35A8E698-2F7A-4DD3-A751-1546D8330929")]
        [AssociationId("BBF188F9-FD2A-47B6-B218-695899867D96")]
        [RoleId("A2DD6369-535C-4439-A58A-FD5E8944D700")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        Resume[] Resumes { get; set; }

        #region Allors
        [Id("563009C2-48D8-42E3-99CE-9FE44932E6E4")]
        [AssociationId("CFBAB153-2860-4D6B-A861-E3D87E505163")]
        [RoleId("0B0B85D8-E3A0-493E-9196-5828880B05FA")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        ContactMechanism HeadQuarter { get; set; }

        #region Allors
        [Id("76B8C934-C3BE-4489-AD19-3F0C75CF61FC")]
        [AssociationId("E7BE97E0-9550-497C-8F97-491D4B3EE7C2")]
        [RoleId("4377D30A-1F39-43AE-A08A-F602CF463742")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Derived]
        [Workspace]
        ElectronicAddress PersonalEmailAddress { get; set; }

        #region Allors
        [Id("AE543D94-C557-4083-8A9D-3A959D6B70F1")]
        [AssociationId("2B61984C-5373-484F-BE76-DA550C12F6DB")]
        [RoleId("144DDD24-93A9-48E2-A1B6-DE70212DC6FB")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        TelecommunicationsNumber CellPhoneNumber { get; set; }

        #region Allors
        [Id("3A19126D-388A-499E-9629-DC9C551FB689")]
        [AssociationId("7A53A271-5307-42EF-B160-649DAB3429B6")]
        [RoleId("ABB061F2-73E8-4F86-9EF8-E30DDD6F0CD2")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        TelecommunicationsNumber BillingInquiriesPhone { get; set; }

        #region Allors
        [Id("0B2392E0-CBAC-4916-85D6-9299A740059F")]
        [AssociationId("C3332EC7-2EB7-4768-807C-AF7994F36893")]
        [RoleId("ECDC5BFD-0A6A-42AA-9875-F75FB8997616")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        ContactMechanism OrderAddress { get; set; }

        #region Allors
        [Id("F536692F-739B-47AB-9D95-C8B8DEEDBBEC")]
        [AssociationId("5EF3E3FB-7A86-4AF5-8E56-F437B87D0588")]
        [RoleId("82578DEE-73B0-4E6E-A703-F49C34C78F85")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        ElectronicAddress InternetAddress { get; set; }

        #region Allors
        [Id("2BC6355F-E363-4573-AAE8-4C5FBD835966")]
        [AssociationId("75A33718-E29E-48F5-9F81-88E092E37E00")]
        [RoleId("863A377D-FED3-4101-858A-9BF37F823064")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        Media[] Contents { get; set; }

        #region Allors
        [Id("3E92FFDE-FF8E-4793-AEA3-44EBCE373523")]
        [AssociationId("9B202283-5E6F-449F-98C9-C678FB8A5B6E")]
        [RoleId("097BD37F-94DE-4FDE-897A-AE46BE430BE7")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        CreditCard[] CreditCards { get; set; }

        #region Allors
        [Id("FA8FD2C5-0761-4617-999A-3A720D2D643E")]
        [AssociationId("6B92D08C-0655-41EE-ADD4-CA5C384436EE")]
        [RoleId("97E942BF-B972-40D7-B8A3-ECC3C27F2F79")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        PostalAddress ShippingAddress { get; set; }

        #region Allors
        [Id("A8BA35AB-CEEF-4580-9DBA-5A895F65668C")]
        [AssociationId("C89A0E01-755D-4991-8126-8E11A97084BE")]
        [RoleId("3677BB82-D0B2-4600-9384-B2F115E20A4C")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Workspace]
        OrganisationContactRelationship[] CurrentOrganisationContactRelationships { get; set; }

        #region Allors
        [Id("73FE7485-099E-4579-B356-905EF9FE72D3")]
        [AssociationId("A7753B00-28BA-409B-88A4-052A69FB74E2")]
        [RoleId("4464EFFB-3A7F-4496-80D6-7CC002085341")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal OpenOrderAmount { get; set; }

        #region Allors
        [Id("0F51981D-446D-4976-B6DB-5F38396C5AD6")]
        [AssociationId("F4A840C3-3173-422F-8349-E9A3B9263991")]
        [RoleId("6569DAF6-5DE7-459F-BC73-04347C9D3049")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        TelecommunicationsNumber GeneralFaxNumber { get; set; }

        #region Allors
        [Id("6AB0C417-0E1D-425E-8C8A-67256C2C78D1")]
        [AssociationId("28B990D1-4CCB-4B30-BA37-17B8F4849FA7")]
        [RoleId("D6BD44F0-D046-45DC-96D3-7F6045917022")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        PaymentMethod DefaultPaymentMethod { get; set; }

        #region Allors
        [Id("4E15D086-2AC6-4D6D-A547-232F577B4B38")]
        [AssociationId("96653BB4-E909-4F92-9FEB-F1355F29F85F")]
        [RoleId("33A0E3B4-7D5C-4D4A-B332-38F5DB07F91B")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Derived]
        [Workspace]
        PartyContactMechanism[] CurrentPartyContactMechanisms { get; set; }

        #region Allors
        [Id("B9EBE9E8-C386-443C-A4C6-F15714E23E1B")]
        [AssociationId("45E3AC3A-1B6B-47AE-B645-F3EED1E6E4A3")]
        [RoleId("6010650B-4E43-44D3-A5E9-26270629A80E")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        TelecommunicationsNumber GeneralPhoneNumber { get; set; }

        #region Allors
        [Id("22CB9D7A-487F-4E8E-9FFC-3A2F3B1AE2C5")]
        [AssociationId("F3F73BD0-F4D6-4FEA-99E0-3A999B419971")]
        [RoleId("0390D2AB-C65E-4FB7-B1E1-855E724AEA77")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        Currency PreferredCurrency { get; set; }

        #region Allors
        [Id("36648D37-BBD7-4C78-87D0-8CB77DABF7ED")]
        [AssociationId("FEB03ADD-B2FE-4221-94A5-B690860407EB")]
        [RoleId("79BD622C-4C66-4FE7-B4A8-19821DD38A17")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        VatRegime VatRegime { get; set; }

        #region Allors
        [Id("9913B873-BECB-47FE-BA36-7475C2CBBF54")]
        [AssociationId("B9C781C1-6893-43A0-B058-46E6525E2B36")]
        [RoleId("8D3E1191-15AA-4F2F-AEDD-3349796B2E8C")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        decimal SimpleMovingAverage { get; set; }

        #region Allors
        [Id("7101A62E-A907-467B-AC41-1924F251FF15")]
        [AssociationId("51239D56-6B8A-4C12-9140-2270684E3208")]
        [RoleId("F9230526-7BB8-4EA0-8E34-FA825D8B05D5")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal AmountOverDue { get; set; }

        #region Allors
        [Id("01402801-A6DE-459F-BE2C-BB7A91842BD3")]
        [AssociationId("D7B777BA-84AE-4389-B80C-8C85CEFF16EC")]
        [RoleId("4C788F47-FE73-46A2-895B-73684D72DD26")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        DunningType DunningType { get; set; }

        #region Allors
        [Id("6A881F23-4889-4E77-9714-59BC3C3943D5")]
        [AssociationId("1D28EAC2-56FE-4794-90B9-612D6E5C5951")]
        [RoleId("C39C1BDF-F5D2-44C7-800C-D302F559B323")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal AmountDue { get; set; }

        #region Allors
        [Id("0645D95E-A4BE-4C2F-8F63-9067D243AFA1")]
        [AssociationId("4F39626E-62C9-4B5B-85B5-251795F6B443")]
        [RoleId("4996E66F-1BE2-4BE1-B127-455279D996F3")]
        #endregion
        [Workspace]
        DateTime LastReminderDate { get; set; }

        #region Allors
        [Id("CE64835C-5975-4597-96D7-1A45DCB94515")]
        [AssociationId("F8611D3B-708F-47B9-9D1C-507BCE044BF0")]
        [RoleId("E4ED5EAB-FF92-451B-98E8-000BC7FACA7D")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal CreditLimit { get; set; }

        #region Allors
        [Id("3B558A2E-9246-4815-8C03-3E1670F3F54A")]
        [AssociationId("4981E91D-8344-4F92-AE4F-D4FB64472212")]
        [RoleId("1C78FBAD-1F04-4552-905F-5810CCE86302")]
        #endregion
        [Required]
        [Workspace]
        int SubAccountNumber { get; set; }

        #region Allors
        [Id("A359AC27-C96A-4356-B4C9-24D2EE4ED0B3")]
        [AssociationId("4C764A0E-5400-45DA-86B5-29D2E0C127C3")]
        [RoleId("3C3FFE2F-CFB3-452A-B416-B534F512EBCF")]
        #endregion
        [Workspace]
        DateTime BlockedForDunning { get; set; }

        #region Allors
        [Id("F4AC4345-2562-489B-AD2B-692235A6F638")]
        [AssociationId("50E45399-5CF8-4BFA-BAA5-1DF03711B243")]
        [RoleId("E5A894DC-7B4E-4BE5-8191-8BC44085D384")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        Agreement[] Agreements { get; set; }

        #region Allors
        [Id("370CAEA6-C7DA-4C22-8DF2-B19B8EFA17BF")]
        [AssociationId("429D59EB-FB22-4018-9E44-189E45A27EEB")]
        [RoleId("62A101D6-594B-47A9-9DE1-AC97F18B4B7D")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        CommunicationEvent[] CommunicationEvents { get; set; }
    }
}