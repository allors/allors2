namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("4AF5DEB9-1466-4691-ADD0-D39E3D65B720")]
    #endregion
    public partial class UnitOfMeasureConversion : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("56E9366E-1474-4F72-A026-047683323049")]
        [AssociationId("4B1F272D-E44C-4556-AD27-FC1831CCB62A")]
        [RoleId("E0DA0143-FB75-46EC-9CC5-697A4EC6A770")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public UnitOfMeasure ToUnitOfMeasure { get; set; }
        #region Allors
        [Id("9738B75D-C8D8-42E4-882F-4C77D6A2C4EE")]
        [AssociationId("22FD56C7-185A-40A3-BA06-2E8D3C3F7AED")]
        [RoleId("E663BADF-3BCE-499C-9C3B-3A642A00180A")]
        #endregion

        public DateTime StartDate { get; set; }
        #region Allors
        [Id("0659D900-2266-4911-9DCB-8527A5880A26")]
        [AssociationId("14E6E4DA-7870-4A58-AD47-DE20C834FE32")]
        [RoleId("A1BAADAA-05E0-465D-9347-68A9E787985C")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(9)]
        public decimal ConversionFactor { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

    }
}