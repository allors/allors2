namespace Allors.Repository.Domain
{
    using System;

    #region Allors
    [Id("7fec090e-3d4a-4ec7-895f-4b30d01f59bb")]
    #endregion
    public partial class PickListItem : AccessControlledObject, Deletable 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("6e89daf6-f07f-4a7d-8032-cc3c08d443c2")]
        [AssociationId("73671086-9f60-489a-a77b-69ab508862cc")]
        [RoleId("50ef0257-edfd-44c4-ab76-230432d1be7d")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal RequestedQuantity { get; set; }
        #region Allors
        [Id("8a8ad2c2-e301-40be-8c7e-3c8291c3bbe9")]
        [AssociationId("1b28c7b7-f770-4e49-acbf-ade8e67ba939")]
        [RoleId("eb4da206-6f39-4615-84e7-afb12f1cf486")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public InventoryItem InventoryItem { get; set; }
        #region Allors
        [Id("f32d100b-a6e8-4cb2-98b4-c06264789c76")]
        [AssociationId("3b75e3a8-7580-4f07-bf75-ae7541a00609")]
        [RoleId("a1017517-e337-4c43-891e-c716ad615b07")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        public decimal ActualQuantity { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}



        public void Delete(){}
        #endregion

    }
}