namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("38b0ac1b-497c-4286-976e-64b3d523ad9d")]
    #endregion
    public partial class Citizenship : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("45d0dd4b-6d8c-4727-b38b-f7ed850023c1")]
        [AssociationId("3944907d-5815-46a3-b380-08a78b637995")]
        [RoleId("d60e6859-26fd-4d01-8458-221e845b75da")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]

        public Passport[] Passports { get; set; }
        #region Allors
        [Id("ca2b2d3e-ba3c-4e92-a86f-92d5d47b8e01")]
        [AssociationId("f2b5857f-064b-4b4d-bf7f-877a46e015e3")]
        [RoleId("c477d58e-d187-4c8c-af20-5f845e143898")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public Country Country { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

    }
}