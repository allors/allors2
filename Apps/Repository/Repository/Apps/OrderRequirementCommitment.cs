namespace Allors.Repository.Domain
{
    using System;

    #region Allors
    [Id("2fcdaf95-c3ec-4da2-8e7e-09c55741082f")]
    #endregion
    public partial class OrderRequirementCommitment : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("a03b08be-82d9-4678-803a-0463c658d4c4")]
        [AssociationId("2ed48b3d-1c77-49f9-a970-836d066cc00f")]
        [RoleId("4f5be1db-964c-4c09-86ec-5b7bd06a4008")]
        #endregion
        [Required]

        public int Quantity { get; set; }
        #region Allors
        [Id("a9020377-d721-4329-868d-33ab63aed074")]
        [AssociationId("5654ce5d-3453-404c-86cb-dfc1cc175345")]
        [RoleId("85a19592-2e58-4d45-8463-2119658fa0b7")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public OrderItem OrderItem { get; set; }
        #region Allors
        [Id("e36224d2-cc6f-43b0-82e1-e300710f6407")]
        [AssociationId("5f56109c-0578-4db7-9c8a-de9617d374d8")]
        [RoleId("2f69978e-bd92-48b2-a711-58b4cf728d96")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public Requirement Requirement { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

    }
}