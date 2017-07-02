namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("E4542BC5-2DEE-42AA-A106-48C65E9EDDF6")]
    #endregion
    public partial class QuoteStatus : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("9C53E6F3-CFDB-4075-8F23-C7285693E9D1")]
        [AssociationId("13B36DC3-7704-41A9-9786-E9C59AF53517")]
        [RoleId("88010439-6125-4965-9157-73000A3AC6BE")]
        #endregion
        [Required]
        [Workspace]
        public DateTime StartDateTime { get; set; }

        #region Allors
        [Id("132AFB32-2E7C-4669-A7B8-FB5BE5F58202")]
        [AssociationId("FE69BD25-16E4-4952-9C02-BC9D50468B82")]
        [RoleId("A57A67E9-B6EA-4653-BF90-5F4112ABF7D0")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public QuoteObjectState QuoteObjectState { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

    }
}