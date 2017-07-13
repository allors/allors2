namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("33e9355b-b3db-43e0-a250-8ebc576e6221")]
    #endregion
    public partial class WorkEffortAssignment : Period, AccessControlledObject, Commentable, Deletable 
    {
        #region inherited properties
        public DateTime FromDate { get; set; }

        public DateTime ThroughDate { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string Comment { get; set; }

        #endregion

        #region Allors
        [Id("54bbdb5d-74b9-4ac7-b638-b1ef4a210b6e")]
        [AssociationId("91713efa-721d-43b7-99dd-ec7681456781")]
        [RoleId("dafa5322-4905-40fa-ae14-ae5ee80f0f1c")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public Person Professional { get; set; }
        #region Allors
        [Id("93cb0818-2599-4652-addd-4a1032d5dde9")]
        [AssociationId("2d5c955f-4bd5-43d2-a8f4-3df03ef6b78b")]
        [RoleId("c42be8db-6e5a-459c-afbc-39bcac3e1eb2")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public WorkEffort Assignment { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}





        public void Delete(){}
        #endregion

    }
}