namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("daf83fcc-832e-4d5e-ba71-5a08f42355db")]
    #endregion
    public partial class RequestItem : IRequestItem, AccessControlledObject, Commentable, Transitional
    {
        #region inherited properties
        public string InternalComment { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }
        public Requirement[] Requirements { get; set; }
        public Deliverable Deliverable { get; set; }
        public ProductFeature ProductFeature { get; set; }
        public NeededSkill NeededSkill { get; set; }
        public Product Product { get; set; }
        public decimal MaximumAllowedPrice { get; set; }
        public DateTime RequiredByDate { get; set; }
        public RequestItemObjectState CurrentObjectState { get; set; }
        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
        public string Comment { get; set; }
        public ObjectState PreviousObjectState { get; set; }
        public ObjectState LastObjectState { get; set; }
        #endregion

        #region Allors
        [Id("AE96F162-EE98-47BA-940F-45C81412702D")]
        [AssociationId("E303A1A5-FF15-4DA3-9281-AAC482EBD3A8")]
        [RoleId("D1DDDFA7-2C05-4527-BADC-253187B3FE74")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public RequestItemVersion CurrentVersion { get; set; }

        #region Allors
        [Id("AF1D8408-2972-4C59-B6BD-819645672E61")]
        [AssociationId("85D24550-8E80-42ED-BD52-2B7D511D83AC")]
        [RoleId("15FD4D94-C198-429A-876F-80C590CA6378")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public RequestItemVersion PreviousVersion { get; set; }

        #region Allors
        [Id("0E0D556B-BB62-4E88-A127-C6CD1C9CB4AB")]
        [AssociationId("9D051007-86F0-4C25-A932-B9C6F365D20D")]
        [RoleId("18D6FA02-5504-4934-A66D-C89594BE6511")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public RequestItemVersion[] AllVersions { get; set; }

        #region Allors
        [Id("13BD89A9-2D31-4ACA-A2E1-CA16B2E0F307")]
        [AssociationId("2E4BBCB8-0F3A-4EB8-BA35-262BCBB2BDAC")]
        [RoleId("FF5FD4D4-5F53-4F83-A9E1-83DC730BB029")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public RequestItemVersion CurrentStateVersion { get; set; }

        #region Allors
        [Id("F4CAB729-E8BD-4755-A9BF-D9032C0CF19A")]
        [AssociationId("09B05189-B945-4686-8031-05C2263B820E")]
        [RoleId("85D5C2A9-8D43-41AB-8AF0-EC3BF5CAA3EA")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public RequestItemVersion[] AllStateVersions { get; set; }

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        #endregion

        #region Allors
        [Id("67541211-256A-4CC8-BCAB-EEA2CCAEBE5F")]
        #endregion
        [Workspace]
        public void Cancel() { }

        #region Allors
        [Id("C3342A3D-A82C-4AD7-815C-921E7A19B5E3")]
        #endregion
        [Workspace]
        public void Submit() { }

        #region Allors
        [Id("{7B95A518-9656-4E18-B10B-CB3C59F2229A}")]
        #endregion
        [Workspace]
        public void Hold() { }
    }
}