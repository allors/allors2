namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("321a6047-2233-4bec-a1b1-9b965c0099e5")]
    #endregion
    public partial interface Request : Transitional, AccessControlledObject, Commentable, Auditable, Printable
    {
        #region Allors
        [Id("D75426C3-2B47-4CF0-82FB-E70CBE2053CA")]
        [AssociationId("7E016046-C569-411B-BE08-056018C7D61B")]
        [RoleId("0E4935D1-D25E-4E2A-874D-4DDDBB271B86")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        RequestObjectState CurrentObjectState { get; set; }

        #region Allors
        [Id("918022F4-D2D6-4596-AF42-2009E981AE73")]
        [AssociationId("7D3B9DC8-3C7B-44BF-A6E2-47C41E5E75B9")]
        [RoleId("40A85126-6FBF-4111-9C64-AE9DE0A45837")]
        #endregion
        [Workspace]
        [Size(-1)]
        string InternalComment { get; set; }

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
        Party Originator { get; set; }

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
        [Id("0FB57C62-05C2-47FA-940C-5285BECB7458")]
        [AssociationId("A9D6D0BA-8DDA-4C9E-BCC7-61A6B5FE4231")]
        [RoleId("56F9E691-52C3-4E8F-84E0-38AA7B53CA4C")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        ContactMechanism FullfillContactMechanism { get; set; }

        #region Allors
        [Id("00066124-DEC8-47DA-B00D-28A3D4BC949D")]
        [AssociationId("5BF00DC5-320E-4FBC-8AC2-9E4B2F9B084C")]
        [RoleId("7E6010F8-5644-4A27-8512-F10B0FCB125B")]
        #endregion
        [Workspace]
        string EmailAddress { get; set; }

        #region Allors
        [Id("04D35F19-31F1-4AA5-A2FB-C02B2D3CBC08")]
        [AssociationId("8DD50E5C-4737-4A5A-8627-831FA0DEFDB0")]
        [RoleId("ABA43478-4CD3-4C3C-AB65-A97DE902DC3F")]
        #endregion
        [Workspace]
        string TelephoneNumber { get; set; }

        #region Allors
        [Id("C38275E1-AC46-46C9-9E27-B08D6BA30BE3")]
        [AssociationId("98519B8D-663B-48DE-A608-0BD76C842276")]
        [RoleId("019EE89A-A427-4A45-AAF6-E403B44A3AFA")]
        #endregion
        [Workspace]
        string TelephoneCountryCode { get; set; }

        #region Allors
        [Id("B30EDA48-5E99-44AE-B3A9-D053BCFA4895")]
        #endregion
        void Cancel();

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

        #region Allors

        [Id("96F7CC66-A06D-4AF0-B163-038FFD83AED7")]

        #endregion
        [Workspace]
        void AddNewRequestItem();
    }
}