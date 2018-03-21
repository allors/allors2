namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("962e5149-546b-4b18-ab09-e4de59b709ff")]
    #endregion
    public partial class PerformanceReviewItem : Commentable, AccessControlledObject 
    {
        #region inherited properties
        public string Comment { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("6d7bb4b2-885d-4f7b-9d31-d517c3d03ac2")]
        [AssociationId("4c8cd6fe-acea-43ae-90e9-41ae1b84f269")]
        [RoleId("1a5977eb-1914-4b02-a0d3-feaad843465d")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public RatingType RatingType { get; set; }
        #region Allors
        [Id("d62d7236-458f-4e30-8df4-27eb877d0931")]
        [AssociationId("7056f19c-c67e-4b54-a08c-c49155326a5e")]
        [RoleId("cac7ce59-1969-43b8-99a9-a90af638558d")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public PerformanceReviewItemType PerformanceReviewItemType { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        #endregion

    }
}