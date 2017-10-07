namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("F76BB372-D433-4479-8324-3A232AC50A25")]
    #endregion
    public partial interface ShipmentVersion : Version
    {
        #region Allors
        [Id("91A8835F-1628-43BC-8564-C5D3DB90F40F")]
        [AssociationId("44DF81FE-CFC4-4262-A48B-74E5FF7E26F8")]
        [RoleId("F56D227D-7C43-4586-BDBF-A40ACC71C5EE")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        ShipmentMethod ShipmentMethod { get; set; }

        #region Allors
        [Id("8AF4C886-4E3C-4D4A-9E80-FC9B838B40EE")]
        [AssociationId("4122FB6B-6941-481C-8185-A017C0DA316E")]
        [RoleId("DB6BD941-5A8A-41EB-B1A2-F593A6904DDC")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        ContactMechanism BillToContactMechanism { get; set; }

        #region Allors
        [Id("94A73829-633B-4DD0-8388-82E3BE795BBC")]
        [AssociationId("CCBA233F-AC1A-43E5-890C-75A32E2A872B")]
        [RoleId("E2994387-6684-46BC-8F13-180B9B0094D6")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        ShipmentPackage[] ShipmentPackages { get; set; }

        #region Allors
        [Id("92BA5224-5123-462F-A1DF-EDDE727B2054")]
        [AssociationId("C5C33BA3-4C2B-4BB0-A532-1F0FD609B866")]
        [RoleId("4A32406B-D279-40A6-9BAE-D6869CFDE879")]
        #endregion
        [Required]
        [Size(256)]
        [Workspace]
        string ShipmentNumber { get; set; }

        #region Allors
        [Id("926845AF-224F-455E-B874-E5AF7D25F63F")]
        [AssociationId("7112611F-7E21-4496-8A33-31CFDECC6AE1")]
        [RoleId("C3834BC3-4883-49D5-8304-03214E0B0FE8")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        Document[] Documents { get; set; }

        #region Allors
        [Id("91BD77AB-215F-450A-9E67-89D1E60D5076")]
        [AssociationId("4702676D-57CF-474E-9D95-D0D8D15F4113")]
        [RoleId("E7FCC62B-58C5-45C7-BC52-4B183F87B043")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        Party BillToParty { get; set; }

        #region Allors
        [Id("A8EBADA5-1B73-4FB5-86C0-FC2EDD9C3264")]
        [AssociationId("9732B291-6DEA-47A7-B9B1-2EB09D075616")]
        [RoleId("68129AA4-F712-44E4-BCD6-EBF59B0E0BE6")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        Party ShipToParty { get; set; }

        #region Allors
        [Id("50757F14-FA1C-48B0-85E0-2EC4491C335C")]
        [AssociationId("C623212B-6C2B-4457-A8E4-245649DEDE3A")]
        [RoleId("E51F94DF-16F0-4808-AEEF-4C126DAF7CCE")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        ShipmentItem[] ShipmentItems { get; set; }

        #region Allors
        [Id("60A764E5-93B1-43DA-A847-58F367CF9F63")]
        [AssociationId("6B35BCC8-7D21-4BC1-BF25-74428111BE2F")]
        [RoleId("CBE42D98-6385-4B8D-A5F4-97A1929B62E9")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        ContactMechanism ReceiverContactMechanism { get; set; }

        #region Allors
        [Id("37AF13D7-3530-4249-872F-FF1E835F33F1")]
        [AssociationId("FE4182DE-E98B-413E-8582-7F184FA83660")]
        [RoleId("23B90A02-1465-452E-8864-9C6098F1F928")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        PostalAddress ShipToAddress { get; set; }

        #region Allors
        [Id("95B1D9E9-9FE4-4639-8D3B-47CFA50AF752")]
        [AssociationId("C94B3F29-980D-4ACE-B6C8-A7A7FB84567B")]
        [RoleId("CF522036-95E6-48FB-8E20-A6D565479307")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal EstimatedShipCost { get; set; }

        #region Allors
        [Id("E7BAFA23-3DE0-472B-B1D3-08CF54CD2F6C")]
        [AssociationId("85B33491-DD4E-4340-8F64-09AE37534EDF")]
        [RoleId("2E16DFFA-2846-467C-BD0E-38DC268BFDD6")]
        #endregion
        [Workspace]
        DateTime EstimatedShipDate { get; set; }

        #region Allors
        [Id("0F39AB42-3546-4AAB-8950-92A1CFE74812")]
        [AssociationId("8568A9D9-FD62-4E20-8BFB-135FBD8352ED")]
        [RoleId("ABBF2287-4F11-4ED1-93DF-7441026E3C85")]
        #endregion
        [Workspace]
        DateTime LatestCancelDate { get; set; }

        #region Allors
        [Id("ACB742CA-0A75-4056-B17F-C3A163954757")]
        [AssociationId("D8D90DF6-2EB1-4B08-826F-EB736BE5F7EF")]
        [RoleId("262C3C1A-4268-40E1-B604-C0D33A7DFD96")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        Carrier Carrier { get; set; }

        #region Allors
        [Id("4C9EA396-9CBE-4693-B929-59BB81EFAF05")]
        [AssociationId("D7C94491-8D7C-452A-B40B-16637E9EB9C3")]
        [RoleId("E666BDFC-77F4-487E-93F2-51C7A1EB30FD")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        ContactMechanism InquireAboutContactMechanism { get; set; }

        #region Allors
        [Id("F216F24A-286B-4C64-9673-AFA8D499FD25")]
        [AssociationId("09B8FFBE-9B0A-4EB0-B6E7-D5CC68AD265C")]
        [RoleId("24128198-F319-4C59-BDFD-ECEC58D7062B")]
        #endregion
        [Workspace]
        DateTime EstimatedReadyDate { get; set; }

        #region Allors
        [Id("C1E6385D-6893-49A0-B88B-CA1A0E3BC2DC")]
        [AssociationId("DFE92CA6-54BB-4244-953C-A3B06300D86A")]
        [RoleId("CD7FA66B-A8B6-42BA-8F36-5DE5D2E2F168")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        PostalAddress ShipFromAddress { get; set; }

        #region Allors
        [Id("B93C3E6E-27DB-4BFF-8D99-25055E7F6A36")]
        [AssociationId("B6EC6B83-73FD-4A72-BFD7-FC8F08C9FA3B")]
        [RoleId("680D93BB-ABBD-4608-BBD2-C416ADE25145")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        ContactMechanism BillFromContactMechanism { get; set; }

        #region Allors
        [Id("7147BB41-5937-416E-8F90-A8AFAB2E48FD")]
        [AssociationId("90307B35-8D8F-4D58-9208-2736BA333766")]
        [RoleId("DBDE20BC-CF23-4759-AC5D-C52E1FC00388")]
        #endregion
        [Size(-1)]
        [Workspace]
        string HandlingInstruction { get; set; }

        #region Allors
        [Id("E29E9265-EFC3-41DF-9596-DF5F145549D3")]
        [AssociationId("EAAA3595-F665-4EE7-ABE3-C8B682830E74")]
        [RoleId("0FCB4E22-4FCA-448B-8565-C3F7B5255D0E")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        Store Store { get; set; }

        #region Allors
        [Id("DFFCADE4-DECB-4BC0-94E2-E1F1FC46D9B0")]
        [AssociationId("07C29F48-2661-4B35-873B-AFDB68362DAA")]
        [RoleId("06C9FEB2-2C1C-45F4-8635-74BC15136B8E")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        Party ShipFromParty { get; set; }

        #region Allors
        [Id("35AE034A-73D8-4478-8E29-F9A2058CA6FE")]
        [AssociationId("ECE7B254-2F7C-467A-B595-A318D1E5A4D7")]
        [RoleId("34BC98CD-4EB3-47D5-853A-DD0FE0292D1E")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        ShipmentRouteSegment[] ShipmentRouteSegments { get; set; }

        #region Allors
        [Id("7A302883-FC2E-483A-9A60-FFD783453770")]
        [AssociationId("E81C1C30-DB71-4D03-99ED-D31D4D156344")]
        [RoleId("FD0581BC-1249-453F-B534-DF80818B9B49")]
        #endregion
        [Workspace]
        DateTime EstimatedArrivalDate { get; set; }
    }
}