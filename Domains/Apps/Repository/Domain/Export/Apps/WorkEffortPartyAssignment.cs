namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("0bdfb093-35af-4c87-9c1c-05ed9dae6df6")]
    #endregion
    public partial class WorkEffortPartyAssignment : AccessControlledObject, Commentable 
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string Comment { get; set; }

        public LocalisedText[] LocalisedComments { get; set; }

        #endregion

        #region Allors
        [Id("3F3D9387-0758-4559-B33F-0C7B352B171C")]
        [AssociationId("EAF539A6-4427-4BC1-B65E-F18E6B86CC08")]
        [RoleId("FD4E4258-8BA2-44D3-92ED-876AB7084458")]
        #endregion
        [Indexed]
        [Workspace]
        public DateTime FromDate { get; set; }

        #region Allors
        [Id("2A49EA68-DB8F-4186-9D7E-FE2CC1AFD6F5")]
        [AssociationId("126CDB1A-46F3-4C20-B93F-D753F6A5CD29")]
        [RoleId("DE319E9D-2814-4656-832A-8A4876FC08D3")]
        #endregion
        [Indexed]
        [Workspace]
        public DateTime ThroughDate { get; set; }

        #region Allors
        [Id("2723be72-6775-4f39-9bf6-e95abc2c0b24")]
        [AssociationId("9014f6ec-c005-43ab-861c-b150474b9dca")]
        [RoleId("ea3986d9-81c8-4353-bc13-7d03255ed9f8")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public WorkEffort Assignment { get; set; }

        #region Allors
        [Id("92081ae5-3e2a-4b13-98b8-0fc45403b877")]
        [AssociationId("2b11931c-a007-4fec-ab78-ecc8388b2a77")]
        [RoleId("90617602-f9c1-4f71-bc6c-c5e4987d008f")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public Party Party { get; set; }

        #region Allors
        [Id("f88ae79d-7605-4be9-972e-541489bdb72b")]
        [AssociationId("b527fed9-c720-45aa-b8c5-fe5336f43f5c")]
        [RoleId("92716fbf-aae3-433f-85f1-9fcfeb68568c")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Facility Facility { get; set; }

        #region Allors
        [Id("A5E053FB-957D-49A4-8EFD-D918043BEDBA")]
        [AssociationId("31B8B332-C2C9-4432-AE64-AFEE4571CDDF")]
        [RoleId("07F29231-3F75-4D3E-B803-5A372A896A2B")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public WorkEffortAssignmentRate[] AssignmentRates { get; set; }

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnInit()
        {
            
        }

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        public void OnPreFinalize(){} public void OnFinalize()
        {
            
        }

        public void OnPostFinalize()
        {
            
        }

        #endregion
    }
}