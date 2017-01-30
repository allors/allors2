namespace Allors.Repository.Domain
{
    using System;

    #region Allors
    [Id("fdf777a8-2e6c-45c3-9385-2d53c1aa8469")]
    #endregion
    public partial class PartyProductRevenueHistory : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("317be52d-211d-4c11-8027-3cdffa9995e7")]
        [AssociationId("1359a604-a2ba-4291-84a9-82fe2f4d4108")]
        [RoleId("4ba5744c-d4ed-4218-ac61-681681fbe80e")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        public decimal Revenue { get; set; }
        #region Allors
        [Id("8a0f314e-522a-43d2-a1a5-5c24ac611f68")]
        [AssociationId("4a0d36f0-57b7-4080-a03e-88bcfed45550")]
        [RoleId("6e05bb8d-c23e-4e63-8dd0-25807023e7e9")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public Party Party { get; set; }
        #region Allors
        [Id("9cc5bc41-3dff-4628-aaa5-d6acaf245b5c")]
        [AssociationId("f9a1dcd6-b3f0-47d9-a17d-26c619c584b6")]
        [RoleId("bd2426e5-d1ae-4e0e-bd87-a1721cd6bd2b")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public Product Product { get; set; }
        #region Allors
        [Id("a717227b-927e-4fde-aece-2c70773e64c7")]
        [AssociationId("a34ff46d-a9cf-4584-858e-141a872bf1c2")]
        [RoleId("73be43b2-e675-4120-9a85-d5fc15cbe0d3")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        public decimal Quantity { get; set; }
        #region Allors
        [Id("d1703751-31aa-461c-af8a-7f52ac8bc96f")]
        [AssociationId("f1db8806-c9ee-4b0d-bb11-da333ecd6acf")]
        [RoleId("640ce1ea-0fea-475f-bb07-a45c34350314")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public InternalOrganisation InternalOrganisation { get; set; }
        #region Allors
        [Id("ffc08a0f-5abc-4f3c-9e8d-7ebb4b43a656")]
        [AssociationId("6bbf007d-4212-4437-b06e-daeb76566c23")]
        [RoleId("7b3cc4f1-6b9d-438b-b82a-9701ecfbd716")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public Currency Currency { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

    }
}