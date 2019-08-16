namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("444e431b-f078-46e0-9c8e-694e15e807c7")]
    #endregion
    public partial class ShipmentPackage : UniquelyIdentifiable, Object
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid UniqueId { get; set; }

        #endregion

        #region Allors
        [Id("293eb102-b098-4e5d-8cef-d5e0b4f1ca5d")]
        [AssociationId("24a2efe7-c10e-4cb0-807b-3c3ae7d4361f")]
        [RoleId("82196a58-d9ba-4508-bdbf-84964f2d2590")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]

        public PackagingContent[] PackagingContents { get; set; }
        #region Allors
        [Id("7f009302-d4f4-4b06-9e18-fb1c35bd79e7")]
        [AssociationId("30cfc1be-1131-4914-888f-30f29e770332")]
        [RoleId("7d4a4b20-3424-43b5-a7cf-7e9422c5870d")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]

        public Document[] Documents { get; set; }
        #region Allors
        [Id("afd7e182-d201-4eee-803c-9fb4dff0feed")]
        [AssociationId("5b2b0551-afcb-4cc3-863e-ba351492da45")]
        [RoleId("d00256d2-fbc8-4935-bfe9-0b0843622936")]
        #endregion
        [Required]

        public DateTime CreationDate { get; set; }
        #region Allors
        [Id("d767222a-b528-4a3f-ac3f-333de19f7ae1")]
        [AssociationId("d1d55767-7b92-49fa-891e-8b701bd56213")]
        [RoleId("a6e84f4d-ebde-4ca8-9cee-57642f0dc41e")]
        #endregion
        [Derived]
        [Required]

        public int SequenceNumber { get; set; }

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
