namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("35d5e80f-e65f-4b0d-9e81-d1604b36a5e3")]
    #endregion
    public partial class SalesChannelRevenueHistory : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("5093d76d-920b-454a-951b-ba123e1ee001")]
        [AssociationId("3aad24b2-c4d8-4aa1-a293-64aa9af82dbc")]
        [RoleId("7f891912-fbaa-409a-816d-6cd85553aeab")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public SalesChannel SalesChannel { get; set; }
        #region Allors
        [Id("6f6af8ac-a4db-46e2-8bfd-a55161b12b66")]
        [AssociationId("345ec7c5-d2c5-489c-99eb-404659c7abba")]
        [RoleId("119c0e16-d47a-46d4-bf57-4ece6cca2bf3")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public Currency Currency { get; set; }
        #region Allors
        [Id("b98b4de3-e2ad-45b4-bbf7-c3b90261979c")]
        [AssociationId("f0e38eaa-8470-451a-ae6f-52b7822ff05f")]
        [RoleId("2119608f-9fd4-48ef-af0f-279abbbc0d4e")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        public decimal Revenue { get; set; }
        #region Allors
        [Id("d1fe6003-0415-4826-ac5b-6fdaff587410")]
        [AssociationId("b87bb3d9-07d4-4178-99eb-77be7baba818")]
        [RoleId("b392d765-f353-4c20-9410-f73f73978eae")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public InternalOrganisation InternalOrganisation { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

    }
}