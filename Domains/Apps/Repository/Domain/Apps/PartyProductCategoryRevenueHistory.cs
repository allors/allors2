namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("02dec829-d0f0-4dfe-8dea-74aeadbe4fc3")]
    #endregion
    public partial class PartyProductCategoryRevenueHistory : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("045cbf8e-1fef-4d3b-a111-1eaccfceba3b")]
        [AssociationId("6b752901-eecb-4f07-bf49-73ef66fd11a8")]
        [RoleId("4c5aa074-4d26-476e-bd2f-ff80e348dec1")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public ProductCategory ProductCategory { get; set; }
        #region Allors
        [Id("06ae29c3-7375-47ee-8e0f-9eaa62874adc")]
        [AssociationId("d1a9033b-14cd-4bd6-aaea-0edff9c28ac3")]
        [RoleId("ef5995f9-61c8-49cc-afa8-28bea455e573")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public Party Party { get; set; }
        #region Allors
        [Id("4211bfb3-5162-448a-878c-79e107af79e9")]
        [AssociationId("3023fbcd-d4ab-4d59-9250-332ed7dd45a3")]
        [RoleId("c3659cb8-de68-401a-b206-a58bdc94dd27")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal Quantity { get; set; }
        #region Allors
        [Id("4e4845c9-9c37-49ec-9b5c-1cd5f247edd4")]
        [AssociationId("f311eb9c-0aa9-4a8d-9283-ab8f32760519")]
        [RoleId("bd33990b-5cf6-4e53-85c2-bfc743f1dfb8")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal Revenue { get; set; }

        #region Allors
        [Id("9154e239-b23b-43f5-b77d-e5bb81c0bcc2")]
        [AssociationId("71b45408-43d1-457f-aba1-30c6641fe996")]
        [RoleId("bdc40f4d-2bbe-485f-ae7e-1e8942a339cf")]
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