namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("87939632-40ff-4a3a-a874-74790e810890")]
    #endregion
    public partial class PurchaseShipmentStatus : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("01d6a244-e174-4a91-8f27-5af54401bed1")]
        [AssociationId("09311427-0d20-4c65-85eb-371d1bcfb23f")]
        [RoleId("125cbf28-2721-4e1b-8cb5-ce3f5a6a464e")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public PurchaseShipmentObjectState PurchaseShipmentObjectState { get; set; }
        #region Allors
        [Id("a243d65e-81ac-49e7-af1b-1b97faa7360e")]
        [AssociationId("9d74270a-7197-43ee-92c9-8f06bd1b48db")]
        [RoleId("fac16474-a909-4566-b55e-5849927aa431")]
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