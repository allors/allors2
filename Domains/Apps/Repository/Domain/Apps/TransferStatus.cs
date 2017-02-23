namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("c2b88d46-c321-48c4-9493-22a886d91bf9")]
    #endregion
    public partial class TransferStatus : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("05a4a5a6-cdaf-4ec8-9a34-cbd40753789b")]
        [AssociationId("a259de23-8d18-4d51-81e3-42796a144b5b")]
        [RoleId("b3fd264c-91a5-425b-b9a0-48eb5cc765fd")]
        #endregion
        [Required]

        public DateTime StartDateTime { get; set; }
        #region Allors
        [Id("a08cde84-30e0-4f99-b6b5-35b45c3fa2b8")]
        [AssociationId("0fb9e813-bd7d-40c8-a1c2-10a569e873c8")]
        [RoleId("63627877-78be-4ffc-aa0d-740049add137")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public TransferObjectState TransferObjectState { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

    }
}