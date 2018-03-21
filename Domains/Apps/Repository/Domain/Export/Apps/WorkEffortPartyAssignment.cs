namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("0bdfb093-35af-4c87-9c1c-05ed9dae6df6")]
    #endregion
    public partial class WorkEffortPartyAssignment : Period, AccessControlledObject, Commentable 
    {
        #region inherited properties
        public DateTime FromDate { get; set; }

        public DateTime ThroughDate { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string Comment { get; set; }

        #endregion

        #region Allors
        [Id("2723be72-6775-4f39-9bf6-e95abc2c0b24")]
        [AssociationId("9014f6ec-c005-43ab-861c-b150474b9dca")]
        [RoleId("ea3986d9-81c8-4353-bc13-7d03255ed9f8")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public WorkEffort Assignment { get; set; }
        #region Allors
        [Id("92081ae5-3e2a-4b13-98b8-0fc45403b877")]
        [AssociationId("2b11931c-a007-4fec-ab78-ecc8388b2a77")]
        [RoleId("90617602-f9c1-4f71-bc6c-c5e4987d008f")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public Party Party { get; set; }
        #region Allors
        [Id("f88ae79d-7605-4be9-972e-541489bdb72b")]
        [AssociationId("b527fed9-c720-45aa-b8c5-fe5336f43f5c")]
        [RoleId("92716fbf-aae3-433f-85f1-9fcfeb68568c")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public Facility Facility { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}



        #endregion

    }
}