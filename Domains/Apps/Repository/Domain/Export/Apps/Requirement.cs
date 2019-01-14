namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("d3f90525-b7fe-4f81-bccd-adf4f57260bc")]
    #endregion
    public partial class Requirement : Transitional, UniquelyIdentifiable, Versioned
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public ObjectState[] PreviousObjectStates { get; set; }

        public ObjectState[] LastObjectStates { get; set; }

        public ObjectState[] ObjectStates { get; set; }

        public Guid UniqueId { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }
        #endregion

        #region ObjectStates
        #region RequirementState
        #region Allors
        [Id("048F8002-E484-4AB7-880A-DB57B9F3634A")]
        [AssociationId("98347F7F-A79D-437F-A4A6-B9870A5D5A35")]
        [RoleId("971FC9AA-B96E-4876-AB06-6E48C4BB2761")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public RequirementState PreviousRequirementState { get; set; }

        #region Allors
        [Id("DCCF39B6-E085-4778-B732-F45A51BA4CA2")]
        [AssociationId("3E6576F3-E99E-44EA-894C-E834702548FD")]
        [RoleId("C1F2029D-CD72-4DE2-A7BE-DB9595ACF87C")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public RequirementState LastRequirementState { get; set; }

        #region Allors
        [Id("B0009B12-8439-4F1A-81F6-7126E5B10D47")]
        [AssociationId("184D3A93-DA09-4585-8482-B9D0999F9460")]
        [RoleId("4C6F78D8-C70E-42F1-B209-33E11E09391F")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public RequirementState RequirementState { get; set; }
        #endregion
        #endregion

        #region Versioning
        #region Allors
        [Id("3651CD57-2472-44F1-8140-4260434C1A1C")]
        [AssociationId("8C9C700C-76D5-40A4-9AC7-4AAA90D88668")]
        [RoleId("F3146121-2D56-4976-9B10-3E89BE52ED30")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public RequirementVersion CurrentVersion { get; set; }

        #region Allors
        [Id("A473CA42-DDB5-4021-B629-8D407A59EB1A")]
        [AssociationId("09016AE6-C349-4690-B40E-4187AC0ADCCC")]
        [RoleId("8F1260D8-1EDB-4672-AF20-C8DF675BDE4D")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public RequirementVersion[] AllVersions { get; set; }
        #endregion

        #region Allors
        [Id("0f2c9ca2-9f2a-403e-8110-311fc0622326")]
        [AssociationId("099c426c-7b3f-4c9a-9059-525851488030")]
        [RoleId("178dfe82-99e2-4026-84f9-223e10e852c7")]
        #endregion
        [Workspace]
        public DateTime RequiredByDate { get; set; }

        #region Allors
        [Id("2202F95A-9D57-4792-BD8F-E35DECDD515E")]
        [AssociationId("C1FFBD64-B2AE-4C94-BE9B-C8E8A0DE0AA8")]
        [RoleId("F14A2315-B95C-4991-8180-FC0DC04D9657")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public RequirementType RequirementType { get; set; }

        #region Allors
        [Id("2b828f2b-201d-4ae2-b64c-b2c5be713653")]
        [AssociationId("8bd1a8cc-4f4d-41ad-b4fb-d43d4759c0e4")]
        [RoleId("041107e2-0936-48a6-86dd-58ace8cbf7ac")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Party Authorizer { get; set; }

        #region Allors
        [Id("3a6ba1d0-3efb-44f3-b90b-7e504ed11140")]
        [AssociationId("5e36946c-46d4-4cd4-9ba7-e1c94746ffe9")]
        [RoleId("93f93798-b587-4f8f-9a82-2e0e9c870a52")]
        #endregion
        [Size(-1)]
        [Workspace]
        public string Reason { get; set; }

        #region Allors
        [Id("3ecf2b1e-ac3d-4533-9da1-341111fca04d")]
        [AssociationId("ea9f2ab4-6774-44eb-91ce-545f499ae792")]
        [RoleId("483b60d4-f3b7-47da-abb4-c7cefee78e2a")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        public Requirement[] Children { get; set; }

        #region Allors
        [Id("43e11ee6-dcee-4a2c-80a7-8e04ee36ceb8")]
        [AssociationId("d2351d54-e600-400b-a350-9d2f81b5cf3d")]
        [RoleId("0d52a5f8-3852-4483-9f0d-a6877fc3b5a0")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Party NeededFor { get; set; }

        #region Allors
        [Id("5ed2803c-02d4-4187-8155-bee79e1a0829")]
        [AssociationId("e0d08055-60ad-4417-b861-ef3b44f00e79")]
        [RoleId("c4abf003-69be-4e79-8958-701aac912d13")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Party Originator { get; set; }

        #region Allors
        [Id("b6b7e1e9-6cce-4ca0-a085-0afd3a58ec50")]
        [AssociationId("fc02e70b-da78-4f1e-aac3-8b4ba32cea90")]
        [RoleId("1137e61a-5efc-4c7c-9073-0f02c03b9408")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Facility Facility { get; set; }

        #region Allors
        [Id("bfce13c0-b5c2-46f0-b0fd-d0d288f8dc07")]
        [AssociationId("7c7ea2fb-451e-4a94-b5fd-cdeab8d97844")]
        [RoleId("f3923b48-a297-43b6-b318-bdafac87c36b")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Party ServicedBy { get; set; }

        #region Allors
        [Id("c34694b4-bd8e-46e9-8bf1-fb1296738ab4")]
        [AssociationId("3bd6d711-d49b-4477-9173-e4f8a17f1d8b")]
        [RoleId("6f53fe03-c9a2-43b8-b38e-99597d751a82")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal EstimatedBudget { get; set; }

        #region Allors
        [Id("d902fe48-c91f-43fe-b402-e0d87606124a")]
        [AssociationId("dfda3196-d793-4f58-af1e-661d943c8908")]
        [RoleId("943f924a-5e11-4e5e-9f3a-fc3df42acfc7")]
        #endregion
        [Required]
        [Size(-1)]
        [Workspace]
        public string Description { get; set; }

        #region Allors
        [Id("f553ad3c-675f-4b97-95c9-42f4d85eb5f9")]
        [AssociationId("995dbc52-905b-4572-a41f-8d39584f4132")]
        [RoleId("81fa089d-cc7f-4893-8186-ef6c98780b68")]
        #endregion
        [Workspace]
        public int Quantity { get; set; }

        #region inherited methods


        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }



        #endregion

        #region Allors

        [Id("8B09FA26-51AC-4286-8304-439E54A1CB2A")]

        #endregion
        public void Reopen()
        {
        }

        #region Allors

        [Id("F96CD431-5143-463E-9C6E-1703AFC2F5E1")]

        #endregion
        public void Cancel()
        {
        }

        #region Allors
        [Id("5C5C6AA9-C8C8-428E-976F-76BC355A1602")]
        #endregion
        public void Hold()
        {
        }

        #region Allors
        [Id("00FBB6C0-BDE5-4913-AF34-2F80AA759B3A")]
        #endregion
        public void Close()
        {
        }
    }
}