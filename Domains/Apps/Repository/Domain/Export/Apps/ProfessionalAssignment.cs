namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("9e679821-8eeb-4dce-b090-d8ade95cb47f")]
    #endregion
    public partial class ProfessionalAssignment : AccessControlledObject, Period 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ThroughDate { get; set; }

        #endregion

        #region Allors
        [Id("18af73aa-336f-4120-8508-a59a9acf17bc")]
        [AssociationId("31da78aa-5e06-48f8-90e4-018ef021a280")]
        [RoleId("cf515b68-b198-4348-881c-fd9a0bcf22bf")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public Person Professional { get; set; }
        #region Allors
        [Id("a75d3ec2-c4f8-4de6-a10c-fe5e3897e663")]
        [AssociationId("70e8f936-27c8-42cb-9459-9a823aaa6318")]
        [RoleId("bb592768-a6f0-47fb-bc74-a15fc5867b34")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public EngagementItem EngagementItem { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        #endregion

    }
}