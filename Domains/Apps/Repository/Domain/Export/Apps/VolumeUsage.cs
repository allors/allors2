namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("c219edcd-71dc-4f0b-afee-4b06f3d785be")]
    #endregion
    public partial class VolumeUsage : DeploymentUsage 
    {
        #region inherited properties
        public TimeFrequency Frequency { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string Comment { get; set; }

        public LocalisedText[] LocalisedComments { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ThroughDate { get; set; }

        #endregion

        #region Allors
        [Id("52e7e94c-3df5-46b5-97f7-27977fc82940")]
        [AssociationId("9b4f98c0-206b-4324-8f58-9adacead03c8")]
        [RoleId("9c2c4c4e-ed7c-467f-8a35-65beee383a9d")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal Quantity { get; set; }
        #region Allors
        [Id("db33fa70-1a64-4f4a-97a8-ee1103b44e62")]
        [AssociationId("2f3b8c14-8eb0-41d5-9fc8-76d29c81d329")]
        [RoleId("03a0c297-8d28-475e-88b7-ffad88d852e8")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public UnitOfMeasure UnitOfMeasure { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnInit()
        {
            
        }

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        public void OnPreFinalize(){} public void OnFinalize()
        {
            
        }

        public void OnPostFinalize()
        {
            
        }

        #endregion

    }
}