namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("b05371ff-0c9e-4ee3-b31d-e2edeed8649e")]
    #endregion
    public partial interface CommunicationEvent : Deletable, Commentable, UniquelyIdentifiable, Auditable, Transitional
    {
        #region ObjectStates
        #region EmailCommunicationState
        #region Allors
        [Id("CEB2FD49-5104-454F-BA3A-5B711B36CF84")]
        [AssociationId("12B37A90-9607-4927-89F7-3DC11A4B76AF")]
        [RoleId("E9522D21-3E20-46E3-813B-7B3B06FE7BC1")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        CommunicationEventState PreviousCommunicationState { get; set; }

        #region Allors
        [Id("F26D8789-D8D8-47C0-A4A2-30A3B2F648F5")]
        [AssociationId("80D58CEF-B47A-45D7-A2FA-40D7B229CC74")]
        [RoleId("E2E5046A-AE71-40B8-8C2B-3B56E0B73894")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        CommunicationEventState LastCommunicationState { get; set; }

        #region Allors
        [Id("80D2E559-CBF6-4C2F-8F89-43921EEF437C")]
        [AssociationId("B0DD2282-59C6-4AF2-8D45-68E3DFA045BC")]
        [RoleId("88955819-9BB6-471C-8FCA-DFF536EF2CE7")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        CommunicationEventState CommunicationEventState { get; set; }
        #endregion
        #endregion

        #region Allors
        [Id("7535B38A-A9EE-4990-B80B-10B83E29999D")]
        [AssociationId("16F9459A-D6D8-45D5-9CDF-98F03F8719E4")]
        [RoleId("C13E4CD8-30AE-45CA-A8D6-EE7F833AB493")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Required]
        [Derived]
        SecurityToken OwnerSecurityToken { get; set; }

        #region Allors
        [Id("193ED0BD-2645-4546-9CDA-AB894CCB569D")]
        [AssociationId("736FD47F-11DB-4408-B8F0-1B02ABC565C9")]
        [RoleId("B7C74220-F30C-49D6-9930-53A582FFDE08")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Required]
        [Indexed]
        [Derived]
        AccessControl OwnerAccessControl { get; set; }

        #region Allors
        [Id("01665c57-a343-441d-9760-53763badce51")]
        [AssociationId("82c1dad0-6d6d-440c-8bf0-f20d35ab0863")]
        [RoleId("0dd9728e-0887-4029-af20-dd69371fbba0")]
        #endregion
        [Workspace]
        DateTime ScheduledStart { get; set; }

        #region Allors
        [Id("16c8aada-318c-4bbb-b8a7-7fa20120eda4")]
        [AssociationId("1f7eaa6f-ccdf-464e-a58a-439c5a063827")]
        [RoleId("04e87ca8-16f0-408c-996c-23c63358c21b")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Indexed]
        [Workspace]
        Party[] ToParties { get; set; }

        #region Allors
        [Id("1aacf179-cf9f-43e1-b950-4121809fde2d")]
        [AssociationId("6c66dc32-0780-4159-afca-b952c3984d1f")]
        [RoleId("bec5291d-f50a-4673-8318-e9c5f553f625")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        ContactMechanism[] ContactMechanisms { get; set; }

        #region Allors
        [Id("28874ffe-f3b3-4aba-9f28-ba7c15b0cb65")]
        [AssociationId("544164cd-43e9-4e3c-a0b2-a33574accd7c")]
        [RoleId("cbf3c355-cf99-4bd4-8f8b-e0dca835b9d2")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Indexed]
        [Workspace]
        Party[] InvolvedParties { get; set; }

        #region Allors
        [Id("2fa315f8-6208-495c-bcc4-2ccda734cc09")]
        [AssociationId("6b5d29f8-7016-4cdb-9af9-8320b1c7304d")]
        [RoleId("8e7c8bab-063d-4f77-99ae-6e7979b63ce4")]
        #endregion
        [Workspace]
        DateTime InitialScheduledStart { get; set; }

        #region Allors
        [Id("3a5658bd-b1b9-47e3-b542-ea9de348a44e")]
        [AssociationId("6086288c-6880-4b98-a0ef-7b4a7ecd0af9")]
        [RoleId("d55ec601-4f3f-4834-baec-1675234e7ba5")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        CommunicationEventPurpose[] EventPurposes { get; set; }

        #region Allors
        [Id("3bc21bd3-1af9-492d-8dde-b0696e20a11a")]
        [AssociationId("439ec2e8-0f9c-474f-8cca-6b33887897ae")]
        [RoleId("1d09f872-86ef-4970-9459-d03075799145")]
        #endregion
        [Workspace]
        DateTime ScheduledEnd { get; set; }

        #region Allors
        [Id("43c26f1f-25bd-4b45-9cdf-c81d021b0b37")]
        [AssociationId("feaad4be-f3e2-4f76-9a87-9676d86bda35")]
        [RoleId("10be3680-44d1-41c9-a084-fbd27a36ecbb")]
        #endregion
        [Workspace]
        DateTime ActualEnd { get; set; }

        #region Allors
        [Id("51f3e08a-7b1b-4d5b-989c-ad2c734a1b2f")]
        [AssociationId("4f409a5c-1de8-4c4f-a157-02f79bef3efb")]
        [RoleId("1fce36c5-aa88-443b-a3a3-c8bd2fd032dd")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        WorkEffort[] WorkEfforts { get; set; }

        #region Allors
        [Id("52adc5f3-d6ef-4804-8755-b86532d8b6fe")]
        [AssociationId("3c7ad2b5-b1c0-4509-b1e3-6e902778bee6")]
        [RoleId("8722394b-3873-4eb2-8bf4-d70abaf0a77a")]
        #endregion
        [Size(-1)]
        [Workspace]
        string Description { get; set; }

        #region Allors
        [Id("65499ae5-ab06-4d21-8f94-8bf95a665e3d")]
        [AssociationId("e4844fd8-62d1-4057-8a9b-4ad4fdc3186b")]
        [RoleId("346af9bb-6091-4d5b-ad8e-92d254876f4a")]
        #endregion
        [Workspace]
        DateTime InitialScheduledEnd { get; set; }

        #region Allors
        [Id("7384d5c7-9af9-45b0-9969-dffe9781cc8c")]
        [AssociationId("540ba3fb-0ba9-4a66-97bb-dfdc33f5cfe8")]
        [RoleId("e571e53d-8b4e-4fa9-9b00-191ffd35c3c5")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Indexed]
        [Workspace]
        Party[] FromParties { get; set; }

        #region Allors
        [Id("79e945d3-1200-4a90-8e80-eba298bcda40")]
        [AssociationId("da2c6684-c940-439b-a4b0-76bb1c3cfc12")]
        [RoleId("22204173-7328-4fe4-a1a6-c394b5908a54")]
        #endregion
        [Required]
        [Size(-1)]
        [Workspace]
        string Subject { get; set; }

        #region Allors
        [Id("91a1555b-a126-4727-86a4-e57e20ebb5da")]
        [AssociationId("38c18c13-4e90-459e-8595-60f1b070cd2a")]
        [RoleId("767e994e-523c-4f2d-a974-470bedb64087")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        Media[] Documents { get; set; }

        #region Allors
        [Id("9e52b6a3-3f94-43d6-9fda-879f57499c05")]
        [AssociationId("9dd6ccef-f816-40d6-9bb4-e1e88b2e0c06")]
        [RoleId("7c309ce2-dd9a-4299-b462-b506b8ca54f4")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        Case Case { get; set; }

        #region Allors
        [Id("bdf87e9c-4ca3-4fba-8b3e-c1252f849953")]
        [AssociationId("2dff0c3b-b14e-4d0f-89d7-e6e371051a1f")]
        [RoleId("d8fb04b0-a113-4cf1-ab3d-1761e2423e76")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        Priority Priority { get; set; }

        #region Allors
        [Id("c43b6f6f-0fda-4794-9199-84b39373ecb3")]
        [AssociationId("f8f85fd4-3b97-4a67-8b42-12e17938c802")]
        [RoleId("bc58b136-9b36-4065-babb-934ede99aefd")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        Person Owner { get; set; }

        #region Allors
        [Id("e85169df-772c-46cc-a0ef-2bf413aec11d")]
        [AssociationId("684ad0be-d99a-4f67-a235-7d17d49ea224")]
        [RoleId("1b5cc695-29d7-48b7-991f-c8271f9a00d4")]
        #endregion
        [Size(-1)]
        [Workspace]
        string Note { get; set; }

        #region Allors
        [Id("ecc20a6a-ef70-4a09-8a3b-c8dce88eaa27")]
        [AssociationId("abdb3a26-ae86-4500-a9d9-d9546fb6f856")]
        [RoleId("406f48d7-a0be-48c9-81f5-7b506b41e114")]
        #endregion
        [Workspace]
        DateTime ActualStart { get; set; }

        #region Allors
        [Id("7A604BA4-05CA-4F01-8DF5-200D1831F8D7")]
        [AssociationId("2CBE5730-BBD4-40E8-A05F-B7E86CB7DFF6")]
        [RoleId("05825569-A1B1-4819-ACA6-62DDD6DE551D")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        bool SendNotification { get; set; }

        #region Allors
        [Id("A6EBA67F-7C65-44AF-9B66-03EB07165CD6")]
        [AssociationId("19641C8E-444C-4791-AF6E-6A3FCB764D0B")]
        [RoleId("819118A2-9EA9-4A94-8241-B2F228E40FDC")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        bool SendReminder { get; set; }

        #region Allors
        [Id("93759DE2-9170-41C9-A641-24D44B89F10F")]
        [AssociationId("A3A3E4D9-96ED-4411-A36F-252B49EEFCFE")]
        [RoleId("3307F535-E2C5-45C8-AF08-DAB96C6B9A4D")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        DateTime RemindAt { get; set; }


        #region Allors
        [Id("F1D66D21-15CC-45C3-980C-E4179F66FD57")]
        #endregion
        [Workspace]
        void Cancel();

        #region Allors
        [Id("97011DA3-10B1-4B27-A4A0-E06D5D6CE04A")]
        #endregion
        [Workspace]
        void Close();

        #region Allors
        [Id("731D1CF2-01CE-44FE-8065-762E4DB1C5E0")]
        #endregion
        [Workspace]
        void Reopen();
    }
}