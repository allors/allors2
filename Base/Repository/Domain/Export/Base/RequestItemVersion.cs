namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("CBEEED83-1411-4081-8605-8D2F4628BB52")]
    #endregion
    public partial class RequestItemVersion : Version
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid DerivationId { get; set; }

        public DateTime DerivationTimeStamp { get; set; }

        public User LastModifiedBy { get; set; }

        #endregion

        #region Allors
        [Id("4768BB1B-5113-42D2-B301-6AEA705922BB")]
        [AssociationId("3C1D2843-93E1-49D5-A75A-2DC5B91CB0A9")]
        [RoleId("8559F7B0-7A9C-41F2-8DAB-F09BFE56F489")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Required]
        [Workspace]
        public RequestItemState RequestItemState { get; set; }

        #region Allors
        [Id("AFC66E59-4E01-4322-ADB8-3458AF745608")]
        [AssociationId("D468BA46-F8B9-4F97-BB97-5486C865A8C4")]
        [RoleId("18900D9B-08E7-43B9-8186-21275458AF00")]
        #endregion
        [Workspace]
        [Size(-1)]
        public string InternalComment { get; set; }

        #region Allors
        [Id("8DB3CC20-5583-4842-BA46-F644C5BB8D53")]
        [AssociationId("2585AE87-F606-483F-81A6-DBC213AD3F53")]
        [RoleId("EBE771E8-9C8E-4496-9EEA-39ADAC2F0C5F")]
        #endregion
        [Size(-1)]
        [Workspace]
        public string Description { get; set; }

        #region Allors
        [Id("6D0ACDF5-5BF9-4930-8FC3-4C39A87DE7A2")]
        [AssociationId("DDC8B1E0-BDAD-4A62-8F24-E67905FE395B")]
        [RoleId("F5C59D20-406C-49D9-B5B5-17E943490C1B")]
        #endregion
        [Workspace]
        public int Quantity { get; set; }

        #region Allors
        [Id("8A087F8A-6E70-45F2-9E4D-65465D0B8939")]
        [AssociationId("9EA95341-32B5-48DF-9FD7-EEBD35AE6A04")]
        [RoleId("4FFF49FE-D8A6-484A-B6DA-000510708550")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public UnitOfMeasure UnitOfMeasure { get; set; }

        #region Allors
        [Id("D6CF8624-A47B-44D8-BDA2-208EBFF7D55A")]
        [AssociationId("C50F4FFF-8543-45C5-853B-C4F42977495B")]
        [RoleId("F6384A0A-DDC7-40D7-B104-F2114A2CA77B")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        public Requirement[] Requirements { get; set; }

        #region Allors
        [Id("7285DB03-008F-45BF-9449-A596B22494B0")]
        [AssociationId("7FE42015-EEA7-4D0F-AF9D-1E425DDE6A9F")]
        [RoleId("60A33115-9ED6-4E37-8560-CEC6FDEA02F5")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Deliverable Deliverable { get; set; }

        #region Allors
        [Id("DD74F2CE-9738-41D8-B683-94BFCC30B604")]
        [AssociationId("3301047F-D908-4DDE-B6F7-3192FB0CF4D2")]
        [RoleId("1624F499-5C13-4215-8E01-033FC115302D")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public ProductFeature ProductFeature { get; set; }

        #region Allors
        [Id("6CA576BB-FD8A-43E2-B504-23BE0237072B")]
        [AssociationId("794FA083-D74E-4743-9EC8-12ECB07FB7AA")]
        [RoleId("37088352-599A-49D5-87A8-73341BE1533D")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public NeededSkill NeededSkill { get; set; }

        #region Allors
        [Id("C8D8AB3A-42F9-40C2-951A-F806EC16A3E9")]
        [AssociationId("AA4CF59D-4292-4D17-8390-4F48FEC0C65E")]
        [RoleId("4B5A29E8-982F-4E65-B831-383D861FB8CD")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Product Product { get; set; }

        #region Allors
        [Id("275F5E14-8DB8-4705-A202-631920EAC5FE")]
        [AssociationId("DDB2E44D-01F9-4FAB-9AF1-B0DD4518385C")]
        [RoleId("8104E068-4E89-4EF1-9E53-89CAE349F303")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal MaximumAllowedPrice { get; set; }

        #region Allors
        [Id("66ED32DE-B1E6-40FF-9D3B-14FE7CEF7755")]
        [AssociationId("3BF26CE0-50A0-47B4-94F3-A10C650A0FE4")]
        [RoleId("C02ADD29-CAE7-4934-8302-8380A410FBD5")]
        #endregion
        [Workspace]
        public DateTime RequiredByDate { get; set; }

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