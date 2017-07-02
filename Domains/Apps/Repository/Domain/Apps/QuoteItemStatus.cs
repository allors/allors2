namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("9ABDED82-E917-433F-A002-B4B6C8C4B390")]
    #endregion
    public partial class QuoteItemStatus : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("90C09C05-C71D-460A-A29B-D3CD412B333B")]
        [AssociationId("2D465E49-64D3-4D75-9EE9-A96AC5BCF014")]
        [RoleId("0F02F1AA-D95F-49B2-B64C-43848C6ED5C9")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public QuoteItemObjectState QuoteItemObjectState { get; set; }

        #region Allors
        [Id("435DB1E7-0A96-4666-BDFD-A0339070987D")]
        [AssociationId("D9A00EFD-FEB4-43F4-AF40-B6CA35FCB9C7")]
        [RoleId("21E4AC09-BF01-4028-8907-B48B750EE7B8")]
        #endregion
        [Required]
        [Workspace]
        public DateTime StartDateTime { get; set; }

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion
    }
}