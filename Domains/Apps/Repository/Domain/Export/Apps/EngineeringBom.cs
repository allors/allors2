namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("14a85148-0d92-4869-8a94-b102f047931f")]
    #endregion
    public partial class EngineeringBom : PartBillOfMaterial 
    {
        #region inherited properties
        public Part Part { get; set; }

        public string Instruction { get; set; }

        public int QuantityUsed { get; set; }

        public Part ComponentPart { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string Comment { get; set; }

        public LocalisedText[] LocalisedComments { get; set; }

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