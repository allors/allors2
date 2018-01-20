namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("5A0B6477-7B54-48FA-AF59-7B664587F197")]
    #endregion
    public partial class SerialisedInventoryItemCharacteristicType : Enumeration
    {
        #region inherited properties

        public LocalisedText[] LocalisedNames { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public Guid UniqueId { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }
        #endregion

        #region Allors
        [Id("A141BDEF-06EF-4441-BACE-C5E3B42F755C")]
        [AssociationId("7D8146E2-78C6-4D10-81BA-13BF6F474F0A")]
        [RoleId("C18F7738-0988-4173-9B64-9447CC50767D")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public UnitOfMeasure UnitOfMeasure { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }
        #endregion

        #region Allors
        [Id("642DC9D7-E6D8-45A5-8109-B80013C6CF32")]
        #endregion
        [Workspace]
        public void Delete() { }
    }
}