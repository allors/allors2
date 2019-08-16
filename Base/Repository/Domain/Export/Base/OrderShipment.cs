namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("00be6409-1ca0-491e-b0a1-3d53e17005f6")]
    #endregion
    public partial class OrderShipment : Deletable
    {
        #region inherited properties
        #endregion

        #region Allors
        [Id("1494758b-f763-48e5-a5a9-cd5c83a8af95")]
        [AssociationId("5aa8e3aa-cc9c-4b12-9126-5ab6f160d661")]
        [RoleId("d49541c8-7cf6-439f-84e0-c8a1d73e5f3c")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public OrderItem OrderItem { get; set; }

        #region Allors
        [Id("b55bbdb8-af05-4008-a6a7-b4eea78096bd")]
        [AssociationId("a4d6f79e-c204-44ca-b7db-3a0a3eacff69")]
        [RoleId("a6a0d0ac-15c6-489f-ab15-197314f4f52c")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        public ShipmentItem ShipmentItem { get; set; }

        #region Allors
        [Id("d4725e9c-b72c-4cdf-95f9-70f9c4b57b11")]
        [AssociationId("4f4c74fc-44d8-445e-aa2e-1e79c2fd6b87")]
        [RoleId("c9ce4f17-3bef-4b0b-a5e0-4fc38641f8ed")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal Quantity { get; set; }

        #region inherited methods

        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {

        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        public void Delete() { }
        #endregion
    }
}