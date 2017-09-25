namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("6cf7d076-5c39-48b5-a27e-5e7752afee2d")]
    #endregion
    public partial class PartyRevenue : AccessControlledObject, Deletable 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("166cc4a8-4f7f-411e-9a43-9c3f44357691")]
        [AssociationId("2cfc0f11-a730-4992-9917-1f6830a534fc")]
        [RoleId("7c8121e5-b9b9-4f4f-9b15-d47fdea0e661")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public Currency Currency { get; set; }
        #region Allors
        [Id("1b98381e-534b-4f65-9fc2-1638698da6fe")]
        [AssociationId("59387dc8-1742-4593-82f6-c74b361d4b35")]
        [RoleId("19e3cb2e-0170-437f-9c4e-ef7765d674d2")]
        #endregion
        [Size(256)]

        public int Month { get; set; }
        #region Allors
        [Id("ca4a5658-e964-4ae5-b5b6-c4e0747d9001")]
        [AssociationId("69e54844-d8e1-4f7b-be89-cde6e4c34431")]
        [RoleId("7e9a8b8f-f98d-402f-baba-a7d22b3a1525")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public Party Party { get; set; }

        #region Allors
        [Id("da71126e-5599-4c2e-9046-cf238501c61e")]
        [AssociationId("9da4cbdf-3010-40fc-bd80-e9d89c412cd9")]
        [RoleId("8aaf333f-3122-419d-b7cc-5a0703cb1615")]
        #endregion
        [Indexed]
        [Required]

        public int Year { get; set; }
        #region Allors
        [Id("f12eef62-4f05-4de5-878f-75a2353ae3b5")]
        [AssociationId("4f4fb02a-a5dc-4e73-a121-47195ec0c793")]
        [RoleId("d2060993-9551-4b5c-96e8-96852d52fe03")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal Revenue { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}



        public void Delete(){}
        #endregion

    }
}