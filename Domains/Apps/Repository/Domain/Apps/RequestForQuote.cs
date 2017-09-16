namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("874dfe70-2e50-4861-b26d-dc55bc8fa0d0")]
    #endregion
    [Plural("RequestsForQuote")]
    public partial class RequestForQuote : Request, IRequestForQuote 
    {
        #region inherited properties
        public string InternalComment { get; set; }
        public string Description { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime RequiredResponseDate { get; set; }
        public RequestItem[] RequestItems { get; set; }
        public string RequestNumber { get; set; }
        public RespondingParty[] RespondingParties { get; set; }
        public Party Originator { get; set; }
        public Currency Currency { get; set; }
        public RequestObjectState CurrentObjectState { get; set; }
        public ContactMechanism FullfillContactMechanism { get; set; }
        public string EmailAddress { get; set; }
        public string TelephoneNumber { get; set; }
        public string TelephoneCountryCode { get; set; }
        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
        public string Comment { get; set; }
        public User CreatedBy { get; set; }
        public User LastModifiedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public Guid UniqueId { get; set; }
        public string PrintContent { get; set; }
        public ObjectState PreviousObjectState { get; set; }
        public ObjectState LastObjectState { get; set; }
        #endregion

        #region Allors
        [Id("4C48ECD7-C684-4103-89B2-F5CFC0675124")]
        [AssociationId("200351D5-D66F-423E-8DDE-61FBA6F4D950")]
        [RoleId("94D40D97-7FAA-4C47-9199-7C693C89F34B")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public RequestForQuoteVersion CurrentVersion { get; set; }

        #region Allors
        [Id("AF2F9C8D-7D99-4CA7-857F-DD86ED0D426F")]
        [AssociationId("013904E5-440D-44BA-A23E-99DB0262F4CC")]
        [RoleId("9698C34E-7394-46BE-B43C-54AFE49779A8")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public RequestForQuoteVersion PreviousVersion { get; set; }

        #region Allors
        [Id("A14F99B1-A53B-4ABD-B5AC-810FF7CBAC6D")]
        [AssociationId("CA69625D-6D3E-435F-8520-843D26783033")]
        [RoleId("9B5447F2-A591-44CA-8414-2406C7BEB14D")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public RequestForQuoteVersion[] AllVersions { get; set; }

        #region Allors
        [Id("619BA764-44C4-4D2D-AD2A-A9AA05D4D95C")]
        [AssociationId("D7A95A82-CCF8-4149-8481-9CF4E872F84D")]
        [RoleId("F8E6D0EB-B15C-49B9-9715-CFA94CBC09BB")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public RequestForQuoteVersion CurrentStateVersion { get; set; }

        #region Allors
        [Id("ADACB87B-784E-426D-854D-D2097BA1C1BC")]
        [AssociationId("65C1E90E-79FC-43E8-94AF-376C1548A33F")]
        [RoleId("7C746A18-809B-47B6-93E3-59B61F72AB5E")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public RequestForQuoteVersion[] AllStateVersions { get; set; }
        
        #region inherited methods
        public void Cancel() { }

        public void Reject() { }

        public void Submit() { }

        public void Hold() { }

        public void AddNewRequestItem() { }

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        #endregion

        #region Allors
        [Id("A57F9C84-A17D-4F5E-91EE-C0AD38EF6985")]
        #endregion
        [Workspace]
        public void CreateQuote() { }
    }
}   