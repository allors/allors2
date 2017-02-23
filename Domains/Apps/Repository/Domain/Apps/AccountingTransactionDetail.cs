namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("e41be1b2-715b-4bc0-b095-ac23d9950ee4")]
    #endregion
    public partial class AccountingTransactionDetail : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("63447735-fdfc-4f32-ab89-d848903d71eb")]
        [AssociationId("8207e741-2250-4b1a-a6aa-86ca531c9d7c")]
        [RoleId("6bba133d-e323-4acd-a26e-2cb4ee6d8821")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public AccountingTransactionDetail AssociatedWith { get; set; }

        #region Allors
        [Id("644b956b-58e3-465b-b431-5637d9a209e5")]
        [AssociationId("1d6d156a-ac84-4453-9efc-40840539d48b")]
        [RoleId("8cdd505a-f5f9-4e85-a49d-d88673f08004")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        public OrganisationGlAccountBalance OrganisationGlAccountBalance { get; set; }
        
        #region Allors
        [Id("9b5a3978-9859-432a-939b-73838c2bb3b2")]
        [AssociationId("bc276111-7fc2-4ae0-a3d8-ac9af05229b2")]
        [RoleId("f2f407bb-7350-4d02-95ea-e35e680a352d")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal Amount { get; set; }
        
        #region Allors
        [Id("d04a0632-e5ec-4a06-bc57-413cf58d2086")]
        [AssociationId("e874eb66-e9d3-4bb5-bac0-b322d3db4fd5")]
        [RoleId("c457a4ce-368f-4956-8749-4a66d703c59b")]
        #endregion
        [Required]
        public bool Debit { get; set; }
        
        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion
    }
}