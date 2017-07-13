namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("563c9706-0b34-4bf0-a09f-72881f10fe6c")]
    #endregion
    public partial class PickListStatus : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("e1187cc2-9518-4387-986a-e989b303035f")]
        [AssociationId("b47b4537-e686-4f86-b45f-5366f05de7d3")]
        [RoleId("67ffe9b3-3916-48e3-9c64-d1427f350737")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public PickListObjectState PickListObjectState { get; set; }
        #region Allors
        [Id("f87a3dcf-742c-4a3c-afbb-af1969164db9")]
        [AssociationId("153a2b44-da58-4db4-9b57-bfa9992c0353")]
        [RoleId("52862edb-477c-4522-85c8-bcedb6affcdd")]
        #endregion
        [Required]

        public DateTime StartDateTime { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

    }
}