namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("7d3a207b-dbdd-48c4-9a92-8b12e4e77874")]
    #endregion
    public partial class SalesInvoiceStatus : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("22daba0b-1f86-4a00-ba83-c541e65822c6")]
        [AssociationId("d28b067f-bd90-45c5-9213-b231ff3bb03f")]
        [RoleId("eb1505fb-6caa-40a3-a09c-1b18fe7dc3ee")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public SalesInvoiceObjectState SalesInvoiceObjectState { get; set; }
        #region Allors
        [Id("74c60d54-b75f-4baa-b1d6-5a33e8ab3944")]
        [AssociationId("ea6bf951-414e-48e6-a579-a2ce8627f635")]
        [RoleId("22405cc3-c402-4236-9517-bdb381d3285f")]
        #endregion
        [Required]

        public DateTime StartDateTime { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

    }
}