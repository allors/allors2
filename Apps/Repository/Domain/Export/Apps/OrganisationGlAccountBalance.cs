namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("67a8352d-7fe0-4398-93c3-50ec8d3e8038")]
    #endregion
    public partial class OrganisationGlAccountBalance : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("347426a0-8678-4eaa-9733-4bf719bad0c2")]
        [AssociationId("754539d8-c07f-420b-a8c1-6201b6015147")]
        [RoleId("c8fb5d7a-b351-49d0-83ad-c0a8b797af36")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public OrganisationGlAccount OrganisationGlAccount { get; set; }
        #region Allors
        [Id("94c5bafb-29ef-4268-846e-5fda5c62af5c")]
        [AssociationId("a3f8a8a3-f837-4ae9-a718-8ab30149086e")]
        [RoleId("c5f7c8d8-a654-4f20-a0b3-a2013e964158")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal Amount { get; set; }
        #region Allors
        [Id("f7325700-87e9-4753-8b0b-de459a6926e7")]
        [AssociationId("58379bfa-a272-4877-98ce-5e46063bc1c2")]
        [RoleId("f7278113-6da8-49af-b205-615cf8df50fd")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public AccountingPeriod AccountingPeriod { get; set; }


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