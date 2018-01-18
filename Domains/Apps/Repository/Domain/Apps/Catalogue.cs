namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("974DCB55-4D12-460F-A45D-9EBCCA54DA0B")]
    #endregion
    public partial class Catalogue : AccessControlledObject, UniquelyIdentifiable, Deletable
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid UniqueId { get; set; }

        #endregion

        #region Allors
        [Id("21F55EB3-4DC1-42C5-AB16-4C47DBCF0456")]
        [AssociationId("E5F96073-E3F1-41FE-8DB4-D387CA3A8C34")]
        [RoleId("DDE889D2-FF86-45E3-AC75-214EDCD13EB8")]
        #endregion
        [Required]
        [Size(256)]
        [Workspace]
        public string Name { get; set; }

        #region Allors
        [Id("4B3D6E3A-29C9-463A-A733-7C2E71BA4AA6")]
        [AssociationId("2EE386C1-BF80-425C-BD1C-2C847C9232FD")]
        [RoleId("EF7FEB9C-05C7-4C30-A42D-0D6844FB5BD2")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        public LocalisedText[] LocalisedNames { get; set; }

        #region Allors
        [Id("72EFFC9B-3233-4E3A-AB7F-1E0FAA386DB9")]
        [AssociationId("DF683A76-FC63-45E6-837E-A80D298E01F2")]
        [RoleId("2E69FB7A-53D1-4AC0-A95A-6AF17867D4B3")]
        #endregion
        [Workspace]
        [Size(-1)]
        public string Description { get; set; }

        #region Allors
        [Id("C5D0293C-42E0-4754-A513-1DE6D886A392")]
        [AssociationId("1A237C1C-30B1-417C-A8B2-576587DA57FB")]
        [RoleId("6B7B649D-C588-4088-A8BF-20E757C2AE4C")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        public LocalisedText[] LocalisedDescriptions { get; set; }

        #region Allors
        [Id("7D57369D-2B53-4F7F-863A-70C61D73903D")]
        [AssociationId("6A22C949-E35F-467A-9B94-CA4C1CE7CC7E")]
        [RoleId("1529ECF6-B052-4D72-A644-38228DED980D")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Media CatalogueImage { get; set; }

        #region Allors
        [Id("902035D4-5333-448D-A161-3D2BC8B0F2F6")]
        [AssociationId("69B97A91-413F-4865-84D2-7FC111CDD43B")]
        [RoleId("73C4D1D9-7980-49C7-8DB8-F024A14351BA")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        public ProductCategory[] ProductCategories { get; set; }

        #region Allors
        [Id("6ED2A606-6937-4711-8750-7137D285FE35")]
        [AssociationId("B9691607-CCCF-4F09-A530-D109DE26569D")]
        [RoleId("0AD81596-E806-42A8-8FE9-F15B8D0B8634")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        [Required]
        public CatScope CatScope { get; set; }
        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        public void Delete() {}

        #endregion

    }
}