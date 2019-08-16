namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("11ADC7EF-323A-49AF-8EF4-ACAC19A923D3")]
    #endregion
    public partial class SerialisedItemState : ObjectState
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }

        public Permission[] ObjectDeniedPermissions { get; set; }

        public string Name { get; set; }

        public Guid UniqueId { get; set; }

        #endregion

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

        #region Allors
        [Id("94381867-DF7D-4B84-8A69-2C0EF8F4401F")]
        [AssociationId("634471AA-D309-4770-9948-9795EFC59549")]
        [RoleId("AED799D5-857A-496C-AF91-EE37DCB15846")]
        #endregion
        [Indexed]
        [Workspace]
        public bool IsActive { get; set; }
    }
}
