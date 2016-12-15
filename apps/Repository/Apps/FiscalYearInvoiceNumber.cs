namespace Allors.Repository.Domain
{
    using System;

    #region Allors
    [Id("341fa885-0161-406b-89e6-08b1c92cd3b3")]
    #endregion
    public partial class FiscalYearInvoiceNumber : Object 
    {
        #region inherited properties
        #endregion

        #region Allors
        [Id("14f064a8-461c-4726-93c4-91bc34c9c443")]
        [AssociationId("02716f0b-8fef-4791-85ae-7c15a5581433")]
        [RoleId("5377c7e0-8bc0-4621-83c8-0829c3fae3f2")]
        #endregion
        [Derived]
        [Required]

        public int NextSalesInvoiceNumber { get; set; }
        #region Allors
        [Id("c1b0dcb6-8627-4a47-86d0-2866344da3f1")]
        [AssociationId("3d1c515f-a52f-4038-9820-794f44927beb")]
        [RoleId("ba7329de-0176-4782-92e1-1cd932823ec0")]
        #endregion
        [Required]

        public int FiscalYear { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}
        #endregion

    }
}