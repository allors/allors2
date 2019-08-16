namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("ded168ad-b674-47ab-855c-46b3e1939e32")]
    #endregion
    public partial class ActivityUsage : DeploymentUsage
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
        [Id("1c8929c2-090a-41f2-8a22-691a63df4ff7")]
        [AssociationId("ab9d6daf-e245-4281-9ff0-fb865c275f79")]
        [RoleId("9acc53b1-4e7a-46c7-a34a-158f5eb05d07")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal Quantity { get; set; }

        #region Allors
        [Id("b7672e5b-5ddc-46ba-82f2-4804f8b43ebf")]
        [AssociationId("3c0cd8a9-c033-4ff1-9ff5-60e90cfffdf5")]
        [RoleId("ed7d8046-4596-4055-af88-b2e4c9da6898")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        public UnitOfMeasure UnitOfMeasure { get; set; }

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
