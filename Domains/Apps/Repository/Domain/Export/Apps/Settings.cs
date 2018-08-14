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
        [Multiplicity(Multiplicity.ManyToOne)]
        public Counter SkuCounter { get; set; }

        #region Allors
        [Id("D306383F-B605-4635-8D06-DD3E4AF06FEF")]
        [AssociationId("1C30099E-86CD-48CB-87B8-0F52B8811B8A")]
        [RoleId("1FD37C0B-B710-499D-861C-172613BEF601")]
        #endregion
        [Workspace]
        public string SkuPrefix { get; set; }

        #region Allors
        [Id("6849E67D-E615-4DE2-93CC-040884D1DC02")]
        [AssociationId("F47A7914-23A5-49F9-84F0-4572E10955E0")]
        [RoleId("46162A91-CF11-451C-ADAC-17A6F8A040B8")]
        [Indexed]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.ManyToOne)]
        public Counter ReferenceNumberCounter { get; set; }

        #region Allors
        [Id("C5A7A9F9-F8A4-4AB3-AB03-E7919F749CA2")]
        [AssociationId("D29758B5-7C3A-4A8C-B8B1-DA6CDBCD69FF")]
        [RoleId("5EA9177F-FACD-453B-8B1C-6F4876FA6ACC")]
        #endregion
        [Workspace]
        public string ReferenceNumberPrefix { get; set; }

        #region inherited methods
        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        #endregion
    }
}