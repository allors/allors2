namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("321a6047-2233-4bec-a1b1-9b965c0099e5")]
    #endregion
	public partial interface Request : AccessControlledObject, Commentable, Auditable, Printable, Transitional
    {
        #region Allors
        [Id("1bb3a4b8-224a-47ab-b05b-c0c8a87ec09c")]
        [AssociationId("57109e48-b116-4ea5-b636-73816c0dda68")]
        [RoleId("d63a2e09-95e1-4c90-83a1-a5366a3d5ca3")]
        #endregion
        [Required]
        [Size(256)]
        [Workspace]
        string Description { get; set; }

        #region Allors
        [Id("0DE2002C-D38C-44A3-9475-471C95B90919")]
        [AssociationId("081B764E-6CC9-44DB-8026-63F83F99A138")]
        [RoleId("4736F9E2-6814-406A-8E35-72E312D50640")]
        #endregion
        [Required]
        [Workspace]
        DateTime RequestDate { get; set; }

        #region Allors
        [Id("208f711f-5d9d-4dc3-89ad-b1583ad06582")]
        [AssociationId("d91ef645-f5ef-4f09-9d6b-c023d02978f5")]
        [RoleId("c1467dbc-9b64-49a0-8715-90ad277b02c9")]
        #endregion
        [Workspace]
        DateTime RequiredResponseDate { get; set; }

        #region Allors
        [Id("25332874-3ec6-41d8-ac6a-77dd4328e503")]
        [AssociationId("acae3045-3612-4cac-9994-ca81d350da74")]
        [RoleId("576e5797-b3d3-41ab-a788-2b3eeba36f18")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        RequestItem[] RequestItems { get; set; }

        #region Allors
        [Id("8ac90ec6-9d3e-45fe-aaba-27d0c1c058a1")]
        [AssociationId("438f6e6a-292b-4579-bf87-7478c48b9159")]
        [RoleId("c16a1509-cfd6-4f9d-87ce-4b903365b9e5")]
        #endregion
        [Size(256)]
        [Workspace]
        string RequestNumber { get; set; }

        #region Allors
        [Id("c3389cec-ee8e-45e2-a1eb-01c9a87b2df0")]
        [AssociationId("b5aaad5b-568c-405d-9018-3ff0fcde7dd2")]
        [RoleId("934585ce-6dc2-46cd-a227-24a1cb85fa60")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        RespondingParty[] RespondingParties { get; set; }

        #region Allors
        [Id("f1a50d9d-2e79-45ac-8f23-8f38fab985c1")]
        [AssociationId("fe8fd88b-8b7d-4998-bf59-56b4e8d44571")]
        [RoleId("2e871e31-a702-4955-8922-ed49a41d5ef1")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        Party Originator  { get; set; }

        #region Allors
        [Id("20028FAE-94FF-47F8-9944-EA558CBDC93B")]
        [AssociationId("D152DF4A-3A5B-48DA-AE5E-BBDE1706EEDF")]
        [RoleId("80216604-A123-4F1B-BDCF-F77AE5D31DFF")]
        #endregion
        [Workspace]
        string InternalComment { get; set; }

        #region Allors
        [Id("BBCF2C40-E793-4FA4-B4E1-612E20971408")]
        [AssociationId("820C9384-8ADE-4AC7-8B34-7CC781C56595")]
        [RoleId("FCE8FB5A-4171-4247-90BC-6EFA6BA43B64")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        Currency Currency { get; set; }

        #region Allors
        [Id("FC848AAB-C683-4338-B9E2-000A5B1C36B1")]
        [AssociationId("F60BB9C7-D8BA-4E85-90B5-2448DCA74818")]
        [RoleId("90EA5CB9-54E3-44D2-8E0F-02183CEFD8C4")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Derived]
        [Indexed]
        [Workspace]
        RequestStatus[] RequestStatuses { get; set; }

        #region Allors
        [Id("BF08C594-A59A-4994-BA91-DB158269DC6E")]
        [AssociationId("53FFEECC-29F9-4F89-A798-AEEBE586DF8A")]
        [RoleId("0FA2602B-2AC0-4904-AA43-4129BC125F84")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Required]
        [Workspace]
        RequestObjectState CurrentObjectState { get; set; }

        #region Allors
        [Id("229BF770-66BA-4896-90CB-BC0B72990619")]
        [AssociationId("EA680DEF-92EE-4505-BE15-B8BD576A58A6")]
        [RoleId("D5DD35D9-AB40-4AD8-9C3E-2D28B4C105FC")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Derived]
        [Workspace]
        RequestStatus CurrentRequestStatus { get; set; }

        #region Allors
        [Id("0FB57C62-05C2-47FA-940C-5285BECB7458")]
        [AssociationId("A9D6D0BA-8DDA-4C9E-BCC7-61A6B5FE4231")]
        [RoleId("56F9E691-52C3-4E8F-84E0-38AA7B53CA4C")]
        [Indexed]
        [Workspace]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        ContactMechanism FullfillContactMechanism { get; set; }

        #region Allors
        [Id("1C897312-5897-4F04-98DC-899179B58680")]
        #endregion
        [Workspace]
        void Cancel();

        #region Allors
        [Id("01CA10DA-C0EE-4435-8E23-F2CC8C98AE59")]
        #endregion
        [Workspace]
        void Complete();

        #region Allors
        [Id("2510F8F6-52E1-4024-A0B1-623DFB62395A")]
        #endregion
        [Workspace]
        void Reject();

        #region Allors
        [Id("7458A51F-5EAD-41A0-B44C-A22B4BA2A372")]
        #endregion
        [Workspace]
        void Submit();

        #region Allors
        [Id("0E26CA10-B0D4-47B0-BEDE-F8EC1AE6BD36")]
        #endregion
        [Workspace]
        void Hold();

    }
}