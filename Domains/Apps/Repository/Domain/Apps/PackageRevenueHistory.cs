namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("9f995d6f-972a-46e4-bbe4-d1e9bedf09ef")]
    #endregion
    public partial class PackageRevenueHistory : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("3d753279-fce6-4d44-a712-08fc21a562f5")]
        [AssociationId("91ab2864-0e91-40bc-bac1-293b7f696f5f")]
        [RoleId("498418d5-938c-4a6f-bf7e-ad683faf0bea")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal Revenue { get; set; }
        #region Allors
        [Id("662d235e-1d36-48d1-a945-f0b60e579ca1")]
        [AssociationId("ffc5e375-a778-459e-978c-e542952cd0fa")]
        [RoleId("771e3c35-cb1a-4fb7-8c0d-01f6bbb9f4e3")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public Package Package { get; set; }
        #region Allors
        [Id("7e7e4510-d1fb-4389-8a8c-d444f1c87daa")]
        [AssociationId("89879f07-2f9a-4c6c-bf3a-8d2ec8742d27")]
        [RoleId("28f27ac4-af47-41a1-b264-1c58bb73a852")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public Currency Currency { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

    }
}