namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("94be4938-77c1-488f-b116-6d4daeffcc8d")]
    #endregion
    public partial class Order : Transitional, Versioned
    {
        #region inherited properties
        public ObjectState PreviousObjectState { get; set; }

        public ObjectState LastObjectState { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("26560f5b-9552-42ea-861f-8a653abeb16e")]
        [AssociationId("d0cdd4a7-6323-4571-85e0-875a5adc56f7")]
        [RoleId("f97ce5e4-88e2-4a4f-a26c-01a68db33efa")]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        #endregion
        public OrderObjectState CurrentObjectState { get; set; }
        
        #region Allors
        [Id("4819AB04-B36F-42F8-B6DE-1F15FFC65233")]
        [AssociationId("8431642A-6874-4931-A4E1-CE696BF3AF84")]
        [RoleId("F1456D98-BAC8-4C2F-9EA6-C3A5C8955621")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        public OrderLine[] OrderLines { get; set; }

        #region Allors
        [Id("5aa7fa5c-c0a5-4384-9b24-9ecef17c4848")]
        [AssociationId("ffcb8a00-571f-4032-b038-82b438f96f74")]
        [RoleId("cf1629aa-2aa0-4dc3-9873-fbf3008352ac")]
        #endregion
        public decimal Amount { get; set; }
        
        #region Allors
        [Id("B8F02B30-51A3-44CD-85A3-1E1E13DBC0A4")]
        [AssociationId("17D327FA-FFF5-40FC-AD7C-E2A57ACA7878")]
        [RoleId("F4160293-1445-4033-8E6E-BED07EBC9A46")]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        #endregion
        public OrderObjectState NonVersionedCurrentObjectState { get; set; }

        #region Allors
        [Id("1879ABB2-78D9-40AF-B404-6CEEF76C7EEC")]
        [AssociationId("CE5AF221-116D-4717-B167-9096A4864797")]
        [RoleId("BEF6A273-AC77-4B7F-946D-B749449B4B68")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        public OrderLine[] NonVersionedOrderLines { get; set; }

        #region Allors
        [Id("D237EF03-A748-4A89-A009-40D73EFBE9AA")]
        [AssociationId("741DF0CD-1204-450D-8A96-12D1CC24D47A")]
        [RoleId("8FAE9C9C-98D5-44E0-944C-BD983CCFAC1B")]
        #endregion
        public decimal NonVersionedAmount { get; set; }

        #region Versioning
        #region Allors
        [Id("4058FCBA-9323-47C5-B165-A3EED8DE70B6")]
        [AssociationId("7FD58473-6579-4269-A4A1-D1BFAE6B3542")]
        [RoleId("DAB0E0A8-712B-4278-B635-92D367F4D41A")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public OrderVersion CurrentVersion { get; set; }

        #region Allors
        [Id("DF0E52D4-07B3-45AC-9F36-2C0DE9802C2F")]
        [AssociationId("08A55411-57F6-4015-858D-BE9177328319")]
        [RoleId("BF309243-98E3-457D-A396-3E6BCB06DE6A")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public OrderVersion[] AllVersions { get; set; }
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