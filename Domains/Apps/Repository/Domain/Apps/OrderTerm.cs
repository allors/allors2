namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("86cf6a28-baeb-479d-ac9e-fabc7fe1994d")]
    #endregion
    public partial class OrderTerm : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("04cd1dd4-6f4f-4cd5-8ca0-5d3ccae06400")]
        [AssociationId("13b304b8-a945-4302-bd45-6a51f03aa8c9")]
        [RoleId("059b0064-a361-48d5-8340-f1ae43db454b")]
        #endregion
        [Size(256)]

        public string TermValue { get; set; }
        #region Allors
        [Id("0540ccac-4970-4026-9529-be62db0350a0")]
        [AssociationId("d5bc8696-24d9-408f-ba50-c20a2c43dec1")]
        [RoleId("76541960-6f11-4cd3-bc78-3018480cf742")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public TermType TermType { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

    }
}