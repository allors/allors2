namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("17b9c8f1-ddf2-4db0-8358-ae66a02395ce")]
    #endregion
    public partial class PositionTypeRate : Period, Object
    {
        #region inherited properties

        public DateTime FromDate { get; set; }
        public DateTime ThroughDate { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }
        #endregion

        #region Allors
        [Id("ab942018-51fd-4135-9005-c81443b72a96")]
        [AssociationId("c35de10d-2f22-4be8-b1e0-6a0e8e3b0922")]
        [RoleId("a443b5af-ae94-4a8b-9c56-2bd9459d9fd8")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal Rate { get; set; }

        #region Allors
        [Id("c49a44b8-dff1-471c-8309-cf9c7e9188c2")]
        [AssociationId("7de3e158-9900-40c4-a015-c62947c0248a")]
        [RoleId("651d72f5-61af-4800-af6a-704159998bfa")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public RateType RateType { get; set; }

        #region Allors
        [Id("CA9D5A86-F155-4EB8-A8C0-D600F9EA0919")]
        [AssociationId("7C25B519-E83D-45A0-8625-B4DF47DC6D1E")]
        [RoleId("65E7460D-4A98-4332-A8C6-0F5A974E5740")]
        [Indexed]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal Cost { get; set; }

        #region Allors
        [Id("f49e4e9e-2e8f-49f6-9c10-4aefb4bb61bf")]
        [AssociationId("6f36fb29-7820-45fa-9dca-888c11d8b0a3")]
        [RoleId("135731d2-4120-45dd-b36c-36c8c93ea99e")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public TimeFrequency Frequency { get; set; }

        #region inherited methods
        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnInit()
        {
            
        }

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion
    }
}