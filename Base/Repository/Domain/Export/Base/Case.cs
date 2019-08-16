namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("a0705b81-2eef-4c51-9454-a31bcedc20a3")]
    #endregion
    public partial class Case : Transitional, UniquelyIdentifiable, Versioned
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid UniqueId { get; set; }

        public ObjectState[] PreviousObjectStates { get; set; }

        public ObjectState[] LastObjectStates { get; set; }

        public ObjectState[] ObjectStates { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

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

        #region Versioning
        #region Allors
        [Id("FDEF4590-CE75-4E71-AB15-4142CDD44C42")]
        [AssociationId("5FDBCBA7-1692-444E-B3C3-C71ADDBE8DFE")]
        [RoleId("0875295C-145B-4DE9-BC87-BF6AA24F0DEB")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public CaseVersion CurrentVersion { get; set; }

        #region Allors
        [Id("3DDE0045-7D6F-49AF-9B92-7BE3FE6EA05C")]
        [AssociationId("4D5588DF-EA6D-4302-8BC7-247CE1D420C8")]
        [RoleId("CC28344D-3E3C-4210-B2D9-919F51BBD1DD")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public CaseVersion[] AllVersions { get; set; }
        #endregion

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
        [Size(-1)]
        public string Description { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {

        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        #endregion
    }
}
