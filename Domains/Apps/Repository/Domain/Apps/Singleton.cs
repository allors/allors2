namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    public partial class Singleton 
    {
        #region inherited properties
        #endregion

        #region Allors
        [Id("64aed238-7009-4157-8395-7eb58ebf7889")]
        [AssociationId("2f79ecfe-5fd4-44d1-9c39-457bb3dc6815")]
        [RoleId("d861c8f8-7362-4805-9941-661a99ab11ac")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Workspace]
        public PrintQueue DefaultPrintQueue { get; set; }
    
        #region Allors
        [Id("d9ea02e5-9aa1-4cbe-9318-06324529a923")]
        [AssociationId("6247e69d-4789-4ee0-a75b-c2de44a5fcce")]
        [RoleId("c11f31e1-75a7-4b23-9d58-7dfec256b658")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public SecurityToken AdministratorSecurityToken { get; set; }
    
        #region Allors
        [Id("9dee4a94-26d5-410f-a3e3-3fcde21c5c89")]
        [AssociationId("0322b71b-0389-4393-8b1f-1b3fb12bb7b1")]
        [RoleId("68f80e6a-7ff4-4f07-b2c5-728459c376ae")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Currency DefaultCurrency { get; set; }
        
        #region Allors
        [Id("a0fdc553-8081-43fa-ae1a-b9f7767d2d3e")]
        [AssociationId("c36bd0ce-d912-4935-b2e2-5aecc822a524")]
        [RoleId("65e3b040-4191-4f26-a51b-6c2a17ec35c7")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Media NoImageAvailableImage { get; set; }
        
        #region Allors
        [Id("f154f01e-e8bb-49c0-be80-ef6c6c195ff3")]
        [AssociationId("2c42c9e4-72e3-4673-8653-aaf586ebb06a")]
        [RoleId("979d1e59-7a9f-462a-9927-efb8ad2cada5")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public InternalOrganisation DefaultInternalOrganisation { get; set; }
        
        #region inherited methods

        #endregion
    }
}