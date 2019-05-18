namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("0ae3caca-9b4b-407f-bd98-46db03b72a43")]
    #endregion
    public partial class SupplierOffering : Commentable, Period, AccessControlledObject 
    {
        #region inherited properties
        public string Comment { get; set; }

        public LocalisedText[] LocalisedComments { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ThroughDate { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("44e38ad4-833c-4da9-894d-bbe57d0f784e")]
        [AssociationId("c5769d37-d236-4ab6-9cab-dcc861dfbade")]
        [RoleId("68ab327e-4ad4-460a-8b9f-f740a19670e0")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public RatingType Rating { get; set; }
        
        #region Allors
        [Id("74895df9-e416-41cb-ab36-24694dc63334")]
        [AssociationId("b81877b2-f7cd-4951-b02e-e60722ca0d72")]
        [RoleId("80326eaa-5546-490e-b433-9ff57f42f85e")]
        #endregion
        [Workspace]
        public int StandardLeadTime { get; set; }

        #region Allors
        [Id("a59d91cc-610f-46b6-8935-e95a42edc31e")]
        [AssociationId("668c50de-36ba-4ba4-9e89-5319a466d5b0")]
        [RoleId("728b18f7-cfaf-4bc0-84d4-5f2c8d0e8b8c")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(4)]
        [Workspace]
        public decimal Price { get; set; }

        #region Allors
        [Id("aa7af527-e616-4d01-86b4-e116c3087a37")]
        [AssociationId("54e165e0-61ac-46cb-bf92-7aa5d62493d0")]
        [RoleId("4a60cdad-817e-4ae8-801a-13dce2d2c772")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public UnitOfMeasure UnitOfMeasure { get; set; }

        #region Allors
        [Id("c16a7bec-e1fc-4034-8eb7-0223b776db7a")]
        [AssociationId("64d0db60-e291-4113-b471-8ac78f9f381d")]
        [RoleId("cc93b5e0-d7f3-4ae4-910e-f7b2539049e0")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public Currency Currency { get; set; }

        #region Allors
        [Id("9c3458aa-7062-4c4c-9160-2f978b088082")]
        [AssociationId("2efde592-4a60-4c79-bc20-f389c5df5966")]
        [RoleId("99b85157-6b6a-4556-a910-af955802b6da")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Ordinal Preference { get; set; }
        
        #region Allors
        [Id("b4cdcc85-583a-49e7-ba35-8985936c7f64")]
        [AssociationId("2133d78d-9f26-46bf-b706-e01e032402df")]
        [RoleId("12dd7fcb-0777-43a6-9524-b2b79c92c40c")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal MinimalOrderQuantity { get; set; }
        
        #region Allors
        [Id("459274A7-2A3C-45DF-B1B8-14171A279AE4")]
        [AssociationId("E1C11BF6-451B-4AC5-84B1-D330DC6B9B36")]
        [RoleId("B7BDC180-8482-4113-A015-F2D3CE734E1E")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public int QuantityIncrements { get; set; }
        
        #region Allors
        [Id("d2de1e9e-196f-43d7-903e-566a4858bc02")]
        [AssociationId("a78c953d-0feb-463a-a7c6-e00640db9e44")]
        [RoleId("4dfd5ba4-ebdf-4ea1-b4d4-ecff642525cb")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public Party Supplier { get; set; }
        
        #region Allors
        [Id("d741765d-d17e-4e6a-88fd-9eee70c82bcf")]
        [AssociationId("3e237d3b-6d44-4afd-a248-f9d15e7822d7")]
        [RoleId("79affcb8-28b2-4629-a918-c863089f1dbc")]
        #endregion
        [Workspace]
        public string SupplierProductName { get; set; }

        #region Allors
        [Id("E7CBF9F3-9762-4102-9CAA-78D1B5F1F39C")]
        [AssociationId("E253BB7F-95C0-45AB-B2D0-0F41AF17122B")]
        [RoleId("067D9FB0-A4F5-447D-A039-89A1F92DDEF7")]
        #endregion
        [Workspace]
        public string SupplierProductId { get; set; }

        #region Allors
        [Id("ea5e3f12-417c-40c4-97e0-d8c7dd41300c")]
        [AssociationId("ba708825-f930-445c-8eaf-29221a405edf")]
        [RoleId("b43787c4-8d38-425a-ab87-b5d3b80f9a5d")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Part Part { get; set; }

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnInit()
        {
            
        }

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion
    }
}