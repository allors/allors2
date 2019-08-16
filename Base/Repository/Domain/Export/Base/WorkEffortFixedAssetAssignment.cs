namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("3b43da7f-5252-4824-85fe-c85d6864838a")]
    #endregion
    public partial class WorkEffortFixedAssetAssignment : Commentable, Period, Deletable, DelegatedAccessControlledObject
    {
        #region inherited properties
        public string Comment { get; set; }

        public LocalisedText[] LocalisedComments { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ThroughDate { get; set; }

        #endregion

        #region Allors
        [Id("2b6eca80-294c-4a2d-a15c-a57c0c815aa1")]
        [AssociationId("1c5736df-6218-45ce-8f86-f668d9dc7fe2")]
        [RoleId("cfb5334c-5843-45f4-90f7-f7ea813e7ec4")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public AssetAssignmentStatus AssetAssignmentStatus { get; set; }

        #region Allors
        [Id("2d7dd4b3-a0bd-45aa-9d1a-a0ffa4a98061")]
        [AssociationId("6d66eb02-1eea-4b2e-8712-be6e1dde98be")]
        [RoleId("e2327e4a-dd69-4e90-983b-dcf29b799201")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public WorkEffort Assignment { get; set; }

        #region Allors
        [Id("a2816fd1-babb-480c-8e29-0f7192aaff71")]
        [AssociationId("c02a3dc0-c977-4893-b7e1-691cfe0c1b03")]
        [RoleId("c097dea3-8f6f-456c-b4cc-7c87c5441517")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal AllocatedCost { get; set; }

        #region Allors
        [Id("e90cb555-e6d9-4d7d-8d98-6f9c28c4bc14")]
        [AssociationId("739ac865-7c8c-45e3-b240-349e4092a56b")]
        [RoleId("4087a04c-3f31-4c82-b7b1-bd5c7818981f")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public FixedAsset FixedAsset { get; set; }

        #region inherited methods


        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {

        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }
        public void Delete() { }
        public void DelegateAccess() { }

        #endregion
    }
}
