namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("4e2d5dee-1dcf-4c14-8acc-d60fd47a3400")]
    #endregion
    public partial class ProductPurchasePrice : AccessControlledObject, Period 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ThroughDate { get; set; }

        #endregion

        #region Allors
        [Id("a59d91cc-610f-46b6-8935-e95a42edc31e")]
        [AssociationId("668c50de-36ba-4ba4-9e89-5319a466d5b0")]
        [RoleId("728b18f7-cfaf-4bc0-84d4-5f2c8d0e8b8c")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal Price { get; set; }
        #region Allors
        [Id("aa7af527-e616-4d01-86b4-e116c3087a37")]
        [AssociationId("54e165e0-61ac-46cb-bf92-7aa5d62493d0")]
        [RoleId("4a60cdad-817e-4ae8-801a-13dce2d2c772")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public UnitOfMeasure UnitOfMeasure { get; set; }
        #region Allors
        [Id("c16a7bec-e1fc-4034-8eb7-0223b776db7a")]
        [AssociationId("64d0db60-e291-4113-b471-8ac78f9f381d")]
        [RoleId("cc93b5e0-d7f3-4ae4-910e-f7b2539049e0")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public Currency Currency { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        #endregion

    }
}