namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("01fc58a0-89b8-4dc0-97f9-5f628b9c9577")]
    #endregion
    public partial class QuoteItem : Commentable, AccessControlledObject, Transitional
    {
        #region inherited properties
        public string Comment { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public ObjectState PreviousObjectState { get; set; }

        public ObjectState LastObjectState { get; set; }

        #endregion

        #region Allors
        [Id("05c69ae6-e671-4520-87c7-5fa24a92c44d")]
        [AssociationId("3f668e84-81dc-479a-a26f-b4fbc1cd79ee")]
        [RoleId("e47f270a-f3d9-4c7b-968f-395bbf8e7e68")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Party Authorizer { get; set; }

        #region Allors
        [Id("1214acee-1b91-4c16-b6d0-84f865b6a43a")]
        [AssociationId("b9120662-ebae-4f52-a913-4a3f9a91398e")]
        [RoleId("d008f8e2-a378-4e50-a9dd-32ffa427708c")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Deliverable Deliverable { get; set; }

        #region Allors
        [Id("20a5f3d3-8b12-4717-874f-eb62ad0a1654")]
        [AssociationId("10c5839d-c046-4b43-919b-d647c70bd94f")]
        [RoleId("56e57558-988c-4b1a-a6f8-7f93f621bd06")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Product Product { get; set; }

        #region Allors
        [Id("262a458d-0b38-4123-b210-576633297f44")]
        [AssociationId("e252b457-9fac-429d-a337-0c48a46c2bf0")]
        [RoleId("a7ae793d-d315-4ac1-93c7-783391b2d294")]
        #endregion
        [Workspace]
        public DateTime EstimatedDeliveryDate { get; set; }

        #region Allors
        [Id("D071BBFA-8960-4F02-8F55-702112A0F608")]
        [AssociationId("F4EA603F-AE0D-425A-AFA9-1162D9BB46CB")]
        [RoleId("EAA89947-2651-470E-8172-581B30929E12")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Derived]
        [Workspace]
        public DateTime RequiredByDate { get; set; }

        #region Allors
        [Id("28c0e280-16ce-48fc-8bc4-734e1ea0cd36")]
        [AssociationId("49bd248e-a34f-43ce-b2fd-9db0d5b01db4")]
        [RoleId("6eb4000d-559d-42b2-b02b-452370fa15b4")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public UnitOfMeasure UnitOfMeasure { get; set; }

        #region Allors
        [Id("28f5767e-16fa-40aa-89d9-c23ee29572d1")]
        [AssociationId("4d7a3080-b3f9-47e8-8363-474a94699772")]
        [RoleId("1da894ac-53bb-4414-b582-9bc6717f369a")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public ProductFeature ProductFeature { get; set; }

        #region Allors
        [Id("73ecd49f-9614-4902-8ec6-9b503bffe9f2")]
        [AssociationId("7ee32e78-a214-4e4a-bb43-d2f6642e997a")]
        [RoleId("e64fb7aa-75de-41a6-a76c-f25f22dfcf47")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal UnitPrice { get; set; }

        #region Allors
        [Id("8b1280eb-0fef-450e-afc8-dbdc6fc65abb")]
        [AssociationId("8a93a23b-6be9-44db-8c92-4ad4c2cc405b")]
        [RoleId("1961e2a8-ecf5-4c7b-8815-8ee4b2461820")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Skill Skill { get; set; }

        #region Allors
        [Id("8be8dc07-a358-4b8d-a84c-01bd3efea6fb")]
        [AssociationId("803fc0c9-ad84-4679-8906-4f9536c7ff6d")]
        [RoleId("a997bb36-f534-4d90-9a90-947cc2a56a64")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public WorkEffort WorkEffort { get; set; }

        #region Allors
        [Id("d1f7f2cb-cbc8-42b4-a3f0-198ff35957de")]
        [AssociationId("0f429c19-5cb8-459a-b95a-9e3ec1e045f3")]
        [RoleId("0750e77a-40bd-4a0b-89a6-6e6fbb797cc4")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        public QuoteTerm[] QuoteTerms { get; set; }

        #region Allors
        [Id("d7805656-dd9c-4144-a11f-efbb32e6ecb3")]
        [AssociationId("a1d818f2-8e1a-4984-b2d7-4b1f34558568")]
        [RoleId("3a3442f4-26af-407d-90c6-38c4d5d40bae")]
        #endregion
        [Workspace]
        public int Quantity { get; set; }

        #region Allors
        [Id("dc00905b-bb4f-4a47-88d6-1ae6ce0855f7")]
        [AssociationId("f9a2cdde-485c-46a0-8f06-9f9687328737")]
        [RoleId("e3308741-e48e-4b91-81ef-de38dcb5d80d")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public RequestItem RequestItem { get; set; }

        #region Allors
        [Id("80D78EB3-EA79-47CA-A155-3B84BC1F1A82")]
        [AssociationId("42D02569-8588-4CFF-B726-99567D4EE8DF")]
        [RoleId("EE08AFBF-D87D-4A22-81CA-AD20E75F2FB8")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Derived]
        [Workspace]
        [Indexed]
        public QuoteItemStatus[] QuoteItemStatuses { get; set; }

        #region Allors
        [Id("36DBF05C-6BB6-4479-B142-DA84688602FA")]
        [AssociationId("D9C8ADDA-2E3C-4789-BECD-3B5FF654055D")]
        [RoleId("A2E450CA-5E60-47DE-9BD5-90574345A294")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Derived]
        [Workspace]
        [Indexed]
        public QuoteItemStatus CurrentQuoteItemStatus { get; set; }

        #region Allors
        [Id("C423E1F0-758C-404D-BAF1-6C1B40C6BDA5")]
        [AssociationId("6FBA1706-21A6-479E-8AC7-7AD21534CABF")]
        [RoleId("BB3841D3-E7AF-4C98-9305-0706BAAD5DCE")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Required]
        [Workspace]
        public QuoteItemObjectState CurrentObjectState { get; set; }

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