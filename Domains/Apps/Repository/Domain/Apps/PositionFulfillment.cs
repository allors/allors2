namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("6a03924c-914b-4660-b7e8-5174caa0dff9")]
    #endregion
    public partial class PositionFulfillment : Commentable, Period, AccessControlledObject 
    {
        #region inherited properties
        public string Comment { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ThroughDate { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("30631f6e-3e70-4394-9540-0572230cd461")]
        [AssociationId("ebcfbd12-ea78-4dd1-b102-05110c7d4a95")]
        [RoleId("3fc029a2-3465-4518-830a-348bd2235a71")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public Position Position { get; set; }
        #region Allors
        [Id("4de369bb-6fa3-4fd4-9056-0e70a72c9b9f")]
        [AssociationId("23fa9951-ceb1-44b2-af36-f3e4955018d1")]
        [RoleId("76c3f430-bf53-4e6f-89af-ea91afbd6795")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public Person Person { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}



        #endregion

    }
}