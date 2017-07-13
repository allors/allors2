namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("49cdc4a2-f7af-43c9-b160-4c7da9a0ca42")]
    #endregion
    public partial class RequirementCommunication : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("5a4d9541-4a8a-4661-bec3-e65db5298857")]
        [AssociationId("d7103ab4-c796-4efd-83bd-256e90c40a14")]
        [RoleId("8edb2d05-b8aa-4d09-90ef-79ce9051df66")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public CommunicationEvent CommunicationEvent { get; set; }
        #region Allors
        [Id("b65140b1-8dc4-4836-9ad8-fe01f43dad7a")]
        [AssociationId("b2ddd7e5-fa91-4257-9400-f776787fffb7")]
        [RoleId("09fb424a-eece-4617-bc65-9fb6861eeb3b")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public Requirement Requirement { get; set; }
        #region Allors
        [Id("cdb72b3f-9920-4082-83a7-a0211a29cf77")]
        [AssociationId("f0743736-d40a-4831-a075-8bdd33cb68f6")]
        [RoleId("208ee5d1-7f60-4c12-888f-04f25c38bc46")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public Person AssociatedProfessional { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

    }
}