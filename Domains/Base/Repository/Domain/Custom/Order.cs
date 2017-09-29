namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("94be4938-77c1-488f-b116-6d4daeffcc8d")]
    #endregion
    public partial class Order : Transitional, Versioned
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public ObjectState[] PreviousObjectStates { get; set; }

        public ObjectState[] LastObjectStates { get; set; }

        public ObjectState[] ObjectStates { get; set; }
        #endregion

        #region ObjectStates
        #region OrderState
        #region Allors
        [Id("7CFAFE73-FEBD-4BFF-B42F-BE9ECF9E74DD")]
        [AssociationId("C03C9BBB-9590-44AA-BF5D-334B064752D7")]
        [RoleId("3498CEC2-0AA8-4AFA-ABED-24C5FA5C8BED")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public OrderState PreviousOrderState { get; set; }

        #region Allors
        [Id("427C6D78-2272-4069-A326-2F551812D6BD")]
        [AssociationId("12D93BE8-E0CD-4D87-ABC4-DA3741B8968B")]
        [RoleId("74F2F239-B7B7-4713-8C18-E9282B49ED5B")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public OrderState LastOrderState { get; set; }

        #region Allors
        [Id("B11EBAC9-5867-4A96-A59B-8A160614FFD6")]
        [AssociationId("0EA6C0AC-F40A-45AD-83EB-CC51EF382886")]
        [RoleId("9233E129-7CDD-41F8-82A2-7CF90004799B")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        public OrderState OrderState { get; set; }
        #endregion

        #region ShipmentState
        #region Allors
        [Id("412BACF5-F927-42D0-BE29-F2870768FA76")]
        [AssociationId("C5F41CDE-77D8-4188-9715-84720DEA9848")]
        [RoleId("46561AC8-3F7F-4BA2-A4EC-270D49B5211A")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public ShipmentState PreviousShipmentState { get; set; }

        #region Allors
        [Id("6C724955-90CA-4069-ACF0-E2A228A928AD")]
        [AssociationId("3EC54D8B-43EA-476D-BC1A-BDE764BD0C2C")]
        [RoleId("936320F5-3172-4864-808E-56468E405CED")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public ShipmentState LastShipmentState { get; set; }

        #region Allors
        [Id("5FEE0701-6C67-478D-9763-25E1E1C70BA1")]
        [AssociationId("BDF89048-6AB3-4A75-A8B9-23C7211729A0")]
        [RoleId("05CA7F49-65F6-4E53-92B9-866CB39ED059")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        public ShipmentState ShipmentState { get; set; }
        #endregion

        #region PaymentState
        #region Allors
        [Id("45981825-4E17-440A-9F60-9DE93DBCA7D3")]
        [AssociationId("61AC5469-1B08-4D51-A72A-84A44740D089")]
        [RoleId("14F708A8-9809-40A4-9DC7-C38991EF9711")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public PaymentState PreviousPaymentState { get; set; }

        #region Allors
        [Id("4E56EDF6-F45F-4CEC-8BDA-28536490503A")]
        [AssociationId("B7B0F3EC-E2C7-4650-BCC3-788D3EBBC240")]
        [RoleId("E21C6C3D-A30B-48FC-BC9F-7B817F1B29D0")]
        [Indexed]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public PaymentState LastPaymentState { get; set; }

        #region Allors
        [Id("BB076A8A-D2E6-47FA-A334-08B0E7E89F05")]
        [AssociationId("5B47A7FA-211B-475A-8741-3E2D020C3F9A")]
        [RoleId("B8AA8155-7F01-4A32-A00A-CCF92F18A974")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        public PaymentState PaymentState { get; set; }
        #endregion
        #endregion
        #endregion

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
        public OrderState NonVersionedCurrentObjectState { get; set; }

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