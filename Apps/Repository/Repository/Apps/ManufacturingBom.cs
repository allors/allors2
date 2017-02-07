namespace Allors.Repository.Domain
{
    using System;

    #region Allors
    [Id("68a0c645-4671-4dda-87a5-53395934a9fc")]
    #endregion
    public partial class ManufacturingBom : PartBillOfMaterial 
    {
        #region inherited properties
        public Part Part { get; set; }

        public string Instruction { get; set; }

        public int QuantityUsed { get; set; }

        public Part ComponentPart { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string Comment { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ThroughDate { get; set; }

        #endregion


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}




        #endregion

    }
}