namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("01fc58a0-89b8-4dc0-97f9-5f628b9c9577")]
    #endregion
    public partial class QuoteItem : IQuoteItem, Commentable, AccessControlledObject, Transitional
    {
        #region inherited properties
        public string InternalComment { get; set; }
        public Party Authorizer { get; set; }
        public Deliverable Deliverable { get; set; }
        public Product Product { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; }
        public DateTime RequiredByDate { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }
        public ProductFeature ProductFeature { get; set; }
        public decimal UnitPrice { get; set; }
        public Skill Skill { get; set; }
        public WorkEffort WorkEffort { get; set; }
        public QuoteTerm[] QuoteTerms { get; set; }
        public int Quantity { get; set; }
        public RequestItem RequestItem { get; set; }
        public QuoteItemObjectState CurrentObjectState { get; set; }
        public string Comment { get; set; }
        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
        public ObjectState PreviousObjectState { get; set; }
        public ObjectState LastObjectState { get; set; }
        #endregion

        #region Allors
        [Id("AD9C5BEA-6D7D-4417-8859-18D7D46DF8CC")]
        [AssociationId("4C91B89F-9207-4521-86EE-015A17DDB4B2")]
        [RoleId("0D558E2F-7D52-4AB8-99F6-484591B48EF0")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public QuoteItemVersion CurrentVersion { get; set; }

        #region Allors
        [Id("C7CC7428-440D-40A1-AA33-BB0B01E71C56")]
        [AssociationId("9DFB82A7-3355-41B7-86B3-AE6FEA039A8D")]
        [RoleId("0B8E8B6B-A57A-4B2C-8882-68407CFDA864")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public QuoteItemVersion PreviousVersion { get; set; }

        #region Allors
        [Id("DA5C696C-3496-49F4-B380-3D78851AC064")]
        [AssociationId("7FD9D35F-2F12-4E72-95F3-F6C56FAF495C")]
        [RoleId("1F87E6FA-379A-47EA-9FA9-71AFD7BF762B")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public QuoteItemVersion[] AllVersions { get; set; }

        #region Allors
        [Id("9159DA1A-ABB6-4F03-AA7F-5C76AA85B6B2")]
        [AssociationId("828F6176-1BFC-463B-BD73-A667DA663B90")]
        [RoleId("845B2252-5FBF-4CE5-B953-FFE24CEAAA04")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public QuoteItemVersion CurrentStateVersion { get; set; }

        #region Allors
        [Id("51374E44-C5F7-40C0-AE93-7B508864E1D5")]
        [AssociationId("3EA337DE-5492-48FA-8C8F-24277B3A4A11")]
        [RoleId("D9100347-AC13-43C2-851B-1AF82416ED9E")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public QuoteItemVersion[] AllStateVersions { get; set; }

        #region Allors
        [Id("57B07865-B4CA-4443-8877-0DDAC1EA106B")]
        #endregion
        [Workspace]
        public void Cancel() { }

        #region Allors
        [Id("C6494A74-92B0-4C9F-9931-8D5C97647DCA")]
        #endregion
        [Workspace]
        public void Submit() { }

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        #endregion
    }
}