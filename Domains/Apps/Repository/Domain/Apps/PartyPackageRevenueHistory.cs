namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("7e9b7f2f-887a-491d-8dab-6a42c908d5a5")]
    #endregion
    public partial class PartyPackageRevenueHistory : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("06fdef2c-73f1-4365-9c74-4a994c3d8f97")]
        [AssociationId("901f16e6-2007-4da0-abb5-b8d36adceb16")]
        [RoleId("2eca8cef-2790-42a7-8d28-8fb4024c9230")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public Package Package { get; set; }

        #region Allors
        [Id("c8ed0a7f-62c9-44e4-a7c5-57a06f9aaedd")]
        [AssociationId("e9398597-8748-41dd-89b9-7f0cf7366d7a")]
        [RoleId("188c541c-7d81-4dbf-8f89-17ce44744f8b")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public Currency Currency { get; set; }
        #region Allors
        [Id("e3aeeaa7-3407-45cd-98da-6f36de3587b2")]
        [AssociationId("0c896c5b-06a6-44ec-8b0a-b038cc560988")]
        [RoleId("bc19a2d0-2efd-4e01-97bd-ea97fa020c3b")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public Party Party { get; set; }
        #region Allors
        [Id("f1f0c7f4-fba8-4388-b5ca-a59a251152f9")]
        [AssociationId("7a7bbe2b-26af-4f05-8e5f-1de7cffa858b")]
        [RoleId("e8797e06-4e7b-453b-9f1d-4f43c779c437")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal Revenue { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

    }
}