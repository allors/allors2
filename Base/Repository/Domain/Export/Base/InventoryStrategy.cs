namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("5E5FE517-4AF6-4977-9E15-8D377E518F03")]
    #endregion
    public partial class InventoryStrategy : UniquelyIdentifiable
    {
        #region inheritedProperties
        public Guid UniqueId { get; set; }
        #endregion inheritedProperties

        #region Allors
        [Id("87EAE8DA-47CF-405E-BFA0-799C87284CC9")]
        [AssociationId("D8E99EB3-A04B-4F86-9601-2271961C2FB9")]
        [RoleId("878CFEEB-D53D-46A9-B861-D1A1B60080EA")]
        #endregion
        [Size(256)]
        [Workspace]
        public string Name { get; set; }

        /* SerialisedInventoryItemState InventoryStrategy Items 
         ******************************************************/

        /// <summary>
        /// The SerialisedInventoryItemStates included in AvailableToPromise calculations for this InventoryStrategy
        /// </summary>
        #region Allors
        [Id("6E36E878-B821-4A74-B722-B834E8204D18")]
        [AssociationId("D386A087-7929-42B0-A142-441AE626FE14")]
        [RoleId("CF6C5294-540A-4C9E-912D-B4A7C4F9D1DF")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        public SerialisedInventoryItemState[] AvailableToPromiseSerialisedStates { get; set; }

        /// <summary>
        /// The SerialisedInventoryItemStates included in QuantityOnHand calculations for this InventoryStrategy
        /// </summary>
        #region Allors
        [Id("9D87E54C-5EB5-4014-84E8-9957126430CA")]
        [AssociationId("0A74B462-E6AB-4C21-BC23-54484444B458")]
        [RoleId("3F1D90C3-4EAB-4026-9EEB-773EF43599F5")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        public SerialisedInventoryItemState[] OnHandSerialisedStates { get; set; }

        /* NonSerialisedInventoryItemState InventoryStrategy Items 
         *********************************************************/

        /// <summary>
        /// The NonSerialisedInventoryItemStates included in AvailableToPromise calculations for this InventoryStrategy
        /// </summary>
        #region Allors
        [Id("2F90BA87-BEEC-4BB6-BF49-45A622B22BD4")]
        [AssociationId("8B0742E6-11FA-4AE4-8A94-FF9C557BC628")]
        [RoleId("01FE80D4-88E9-4421-B5C6-13833FF55349")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        public NonSerialisedInventoryItemState[] AvailableToPromiseNonSerialisedStates { get; set; }

        /// <summary>
        /// The NonSerialisedInventoryItemStates included in QuantityOnHand calculations for this InventoryStrategy
        /// </summary>
        #region Allors
        [Id("F20911A6-B1A6-46E8-955B-4286DE54D806")]
        [AssociationId("04C41736-A073-4BFC-8C4A-4AC3DD87544D")]
        [RoleId("8BCA64CB-7768-4ED5-AB2D-8C164EA3A266")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        public NonSerialisedInventoryItemState[] OnHandNonSerialisedStates { get; set; }

        #region inheritedMethods

        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
        public void OnBuild() { }

        public void OnDerive() { }

        public void OnPostBuild() { }

        public void OnInit()
        {

        }

        public void OnPostDerive() { }

        public void OnPreDerive() { }
        #endregion inheritedMethods
    }
}