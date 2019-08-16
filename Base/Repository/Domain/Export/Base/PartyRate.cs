namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("2E6F43A2-3D1D-401B-86BF-7DE63FC9FF3E")]
    #endregion
    public partial class PartyRate : Period, Object
    {
        #region inherited properties
        public DateTime FromDate { get; set; }

        public DateTime ThroughDate { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }
        #endregion

        #region Allors
        [Id("6906DD20-C4B4-4516-B09F-9F93E2849F19")]
        [AssociationId("AF17580B-2907-48DB-A8C6-849673BE2AD9")]
        [RoleId("F4EF561C-FDCE-4FFA-8B83-A754AE2AF95E")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal Rate { get; set; }

        #region Allors
        [Id("C99C4330-5578-4B8D-872A-C72C43DA42FA")]
        [AssociationId("60857A63-F46D-4FF5-87A8-45232BF85F24")]
        [RoleId("25CB9BD0-B2FC-43E4-8899-891F39F1B173")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public RateType RateType { get; set; }

        #region Allors
        [Id("AE04E207-CCEC-492C-8C66-1AB5A1A4BFA7")]
        [AssociationId("6E6F64F8-9A25-4684-B275-2518EB4FE93F")]
        [RoleId("0BD4E1C3-2C83-4FA1-807A-6AF8D92BD562")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal Cost { get; set; }

        #region Allors
        [Id("5563FC19-D342-4254-A4A7-9CBD41B43868")]
        [AssociationId("32E4468D-CECE-4647-9189-53B1FACA5E18")]
        [RoleId("504B800C-96FF-4D84-8365-A597918392C6")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        public PriceComponent[] GoverningPriceComponents { get; set; }

        #region Allors
        [Id("6943FC0E-9A7C-4024-90A9-D0848C148AEF")]
        [AssociationId("240AFD92-0144-43E4-931F-1E5992E37D02")]
        [RoleId("F9850F45-AC4B-467E-9149-FB8FE7C30D8F")]
        #endregion
        [Size(-1)]
        [Workspace]
        public string ChangeReason { get; set; }

        #region Allors
        [Id("21C7B4A0-F33B-401F-B136-AFA7B226EACE")]
        [AssociationId("39A8F947-0DEC-4FCB-8DE0-EE47E88E6B00")]
        [RoleId("ACFC2FC2-C57A-4DEC-B213-62DE82A542FD")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public TimeFrequency Frequency { get; set; }

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
