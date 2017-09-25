namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("4c163351-b42e-4bd3-8cbf-db110eba05fc")]
    #endregion
    public partial class BudgetStatus : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("070418ab-f9aa-4286-9395-879b06cf832a")]
        [AssociationId("ee3be6af-f2b5-411a-a07b-24eb676bd923")]
        [RoleId("ceee8ab2-a8da-45d8-be09-61e353e8b1a3")]
        #endregion
        [Required]

        public DateTime StartDateTime { get; set; }
        #region Allors
        [Id("125c0c29-4f69-4e0b-b885-76c1e908737e")]
        [AssociationId("f5e1e19d-2c13-4163-b796-a8e0b7a80fcc")]
        [RoleId("554bd320-adce-40ac-83b0-5710e69a0b25")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public BudgetObjectState BudgetObjectState { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

    }
}