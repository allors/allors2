namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("DC94D0BF-E08D-4B01-A91F-723CED6F3C36")]
    #endregion
    [Plural("Settingses")]

    public partial class Settings : Object
    {
        #region Allors
        [Id("9dee4a94-26d5-410f-a3e3-3fcde21c5c89")]
        [AssociationId("0322b71b-0389-4393-8b1f-1b3fb12bb7b1")]
        [RoleId("68f80e6a-7ff4-4f07-b2c5-728459c376ae")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Currency PreferredCurrency { get; set; }

        #region Allors
        [Id("a0fdc553-8081-43fa-ae1a-b9f7767d2d3e")]
        [AssociationId("c36bd0ce-d912-4935-b2e2-5aecc822a524")]
        [RoleId("65e3b040-4191-4f26-a51b-6c2a17ec35c7")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Workspace]
        public Media NoImageAvailableImage { get; set; }

        #region Allors
        [Id("C84C214C-B6CA-4017-912D-954BAC0946D6")]
        [AssociationId("D6786C6F-5CF7-40C8-B622-CFC7E7A9929B")]
        [RoleId("74E80E35-2129-473C-9ADA-FD7C58AFF8A7")]
        [Indexed]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.OneToOne)]
        public Counter SkuCounter { get; set; }

        #region Allors
        [Id("D306383F-B605-4635-8D06-DD3E4AF06FEF")]
        [AssociationId("1C30099E-86CD-48CB-87B8-0F52B8811B8A")]
        [RoleId("1FD37C0B-B710-499D-861C-172613BEF601")]
        #endregion
        [Workspace]
        public string SkuPrefix { get; set; }

        #region Allors
        [Id("01E190C7-B91E-4A48-A251-6F3E625CD6D3")]
        [AssociationId("6B6B0A35-A4C2-40B4-A558-0A75BE0B199B")]
        [RoleId("A838D5A1-2338-47A9-8E97-A94B2A78B907")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public Counter SerialisedItemCounter { get; set; }

        #region Allors
        [Id("F0B93DF3-E9E7-408D-980B-FB0889707FBE")]
        [AssociationId("31B15651-BE30-4B9D-8D68-A76180B2F886")]
        [RoleId("D040DAAF-AB68-4F5D-931F-5F1C1A022B71")]
        #endregion
        [Workspace]
        public string SerialisedItemPrefix { get; set; }

        #region Allors
        [Id("8438F903-1BFF-419D-9E89-D7A3943821D3")]
        [AssociationId("DB872406-A43E-4383-8928-E0DA02E7C676")]
        [RoleId("0167899D-957F-4323-BD05-4A9B1AB25B4F")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public Counter GlobalProductNumberCounter { get; set; }

        #region Allors
        [Id("E14816F1-65DA-4042-91E3-6F0906611D10")]
        [AssociationId("DE163874-FD5D-448B-841C-C13D3733129A")]
        [RoleId("8822D37A-DA68-4CE0-8DEE-F1A8414C49D1")]
        #endregion
        [Workspace]
        public string GlobalProductNumberPrefix { get; set; }

        #region Allors
        [Id("C1FA075A-2607-476D-BC27-A13656C56684")]
        [AssociationId("06D7AA12-6398-4074-8584-046AA66E7B2A")]
        [RoleId("434C5608-147F-4B00-8BD1-461980B2576C")]
        #endregion
        [Workspace]
        [Required]
        public bool UseGlobalProductNumber { get; set; }

        #region inherited methods
        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        #endregion
    }
}