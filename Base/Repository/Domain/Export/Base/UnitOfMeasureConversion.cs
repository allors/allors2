namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("2e216901-eab9-42e3-9e49-7fe8e88291d3")]
    #endregion
    public partial class UnitOfMeasureConversion : Object
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("3ae94702-ee60-4057-a649-f655ff4e2865")]
        [AssociationId("1ab7d188-af19-4742-a0e6-11043b666bd4")]
        [RoleId("5372ec1c-9b57-4ed5-b665-cdad8a13d933")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public IUnitOfMeasure ToUnitOfMeasure { get; set; }
        #region Allors
        [Id("5d7ed801-4a2e-4abc-a32d-d869210132af")]
        [AssociationId("a3467a5f-8c7d-453a-9a33-18d742f20d06")]
        [RoleId("4b8a465d-9334-427f-b799-d08b7c84200a")]
        #endregion

        public DateTime StartDate { get; set; }
        #region Allors
        [Id("835118da-148a-4c42-ab07-75b213a8e1f7")]
        [AssociationId("f9f78e34-6fe1-4863-b831-cabe46cbc764")]
        [RoleId("c06dd0a5-dabe-46fa-97f7-62f6f4b47983")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(9)]
        public decimal ConversionFactor { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {

        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        #endregion

    }
}
