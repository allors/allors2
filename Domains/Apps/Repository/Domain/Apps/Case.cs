namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("a0705b81-2eef-4c51-9454-a31bcedc20a3")]
    #endregion
    public partial class Case : AccessControlledObject, Transitional, UniquelyIdentifiable 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid UniqueId { get; set; }

        public ObjectState[] PreviousObjectStates { get; set; }

        public ObjectState[] LastObjectStates { get; set; }

        public ObjectState[] ObjectStates { get; set; }


        #endregion

        #region ObjectStates
        #region CaseState
        #region Allors
        [Id("8865A76F-2D5F-425A-8F28-CC17E8280D6E")]
        [AssociationId("4233007E-1897-4BE7-A2B8-AFE5AE2F348A")]
        [RoleId("60B1D7BB-DB13-45A4-88D1-D301EEBB18E2")]
        [Indexed]
        #endregion        
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public CaseState PreviousCaseState { get; set; }

        #region Allors
        [Id("BEFFB894-CC36-40F5-9BEE-2DC71EB28434")]
        [AssociationId("1691B00E-F231-4C69-912B-1A073B603964")]
        [RoleId("4DF5C32B-E6BC-4DB5-9AD6-D72B480AB4D7")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public CaseState LastCaseState { get; set; }

        #region Allors
        [Id("5DB5AE45-14A7-4CB0-9634-E2F347D07F0E")]
        [AssociationId("A419D51E-4EC1-43A5-A477-7174F7D3672F")]
        [RoleId("2B626C07-55F6-43F0-8ABD-24D103FA405F")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public CaseState CaseState { get; set; }
        #endregion
        #endregion

        #region Allors
        [Id("2286f83b-7992-4aa0-80fe-ad19e3c8c572")]
        [AssociationId("484381bd-6dbc-4a78-bc59-c21422b942b2")]
        [RoleId("c9951d63-5b1a-4053-9756-16b46a336288")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Derived]
        [Indexed]
        public CaseStatus CurrentCaseStatus { get; set; }

        #region Allors
        [Id("51bfbe94-46a5-411f-ac10-8623bfc4472c")]
        [AssociationId("b8b5d65b-14c9-4ab0-89b9-4124d60cfeb7")]
        [RoleId("b49c43fd-798c-4608-a055-af04d97aa72d")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Derived]
        [Indexed]
        public CaseStatus[] CaseStatuses { get; set; }

        #region Allors
        [Id("65e310b5-1358-450c-aec2-985dcc724cdd")]
        [AssociationId("d815e7c2-fe40-470c-9ab9-007f7bc0465b")]
        [RoleId("fee6ebfb-3ce6-473b-9142-ea70ade93709")]
        #endregion
        public DateTime StartDate { get; set; }

        #region Allors
        [Id("87f64957-53f9-446e-ac1f-323a00da027f")]
        [AssociationId("289d52aa-fb69-4e7d-ba49-e4521614e19b")]
        [RoleId("dec26736-f037-48c1-a4b2-0247b9abf92b")]
        #endregion
        [Required]
        [Size(256)]
        public string Description { get; set; }

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}



        #endregion
    }
}