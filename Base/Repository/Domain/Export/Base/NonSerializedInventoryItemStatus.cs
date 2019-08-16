namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("700360b9-56be-4e51-9610-f1e5951dd765")]
    #endregion
    public partial class NonSerialisedInventoryItemStatus : Object
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("590f1d9b-a805-4b0e-a2bd-8e274608fe3c")]
        [AssociationId("bcc05955-7c14-45f9-b2ea-ab36feca7287")]
        [RoleId("30a81620-9871-414c-9b09-c2fad0358bb4")]
        #endregion
        [Required]

        public DateTime StartDateTime { get; set; }
        #region Allors
        [Id("959aa0a9-a197-4eb4-bc9e-e40da8892dd0")]
        [AssociationId("d3b7eb35-52d2-48fc-a416-a2185ae347ee")]
        [RoleId("78059f92-3345-4e5a-8d04-d30d15eee05a")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public NonSerialisedInventoryItemObjectState NonSerialisedInventoryItemObjectState { get; set; }

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
