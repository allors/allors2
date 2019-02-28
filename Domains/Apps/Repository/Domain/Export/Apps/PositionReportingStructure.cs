namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("b50d0780-bcbf-4041-8576-164577d40c55")]
    #endregion
    public partial class PositionReportingStructure : AccessControlledObject, Commentable 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string Comment { get; set; }

        public LocalisedText[] LocalisedComments { get; set; }

        #endregion

        #region Allors
        [Id("23b91508-508f-4afe-8259-a17f16381833")]
        [AssociationId("71f3915d-e412-40fa-abb3-c083e8b2488b")]
        [RoleId("b658e2b8-0929-4770-b231-e532653d0841")]
        #endregion

        public bool Primary { get; set; }
        #region Allors
        [Id("5fbc72bf-2153-4b91-83f9-6fd057e4b1d6")]
        [AssociationId("c06de12f-bf0e-4d91-b8f6-9f6b250b107c")]
        [RoleId("26944bd3-762b-4436-ba19-5e5c34c1247f")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public Position ManagedByPosition { get; set; }
        #region Allors
        [Id("e2e60d09-ebfa-4bf3-94e9-759279b00919")]
        [AssociationId("1e94dba5-c7d3-41ca-ae79-80b0d2b2ce3c")]
        [RoleId("7b375d54-2364-422c-a264-dd6438d53d33")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public Position Position { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnInit()
        {
            
        }

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        #endregion

    }
}