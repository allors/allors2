namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("0091574c-edac-4376-8d03-c7e2c2d8132f")]
    #endregion
    public partial class PartSpecification : UniquelyIdentifiable, Commentable, Transitional, Versioned
    {
        #region inherited properties

        public Guid UniqueId { get; set; }

        public string Comment { get; set; }

        public LocalisedText[] LocalisedComments { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public ObjectState[] PreviousObjectStates { get; set; }

        public ObjectState[] LastObjectStates { get; set; }

        public ObjectState[] ObjectStates { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }
        #endregion

        #region ObjectStates
        #region 
        #region Allors
        [Id("99C34A9A-D41A-415C-A4B7-2D1931451A68")]
        [AssociationId("E2F5FF24-EA16-48BD-9000-92EA64BEF2B2")]
        [RoleId("8D089037-3D19-4D99-90C7-7BDAF8BD032D")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public PartSpecificationState PreviousPartSpecificationState { get; set; }

        #region Allors
        [Id("41EBF5DA-B98C-4588-84E5-8DE87B508716")]
        [AssociationId("B3EECAAD-C2C6-4E7B-A619-F2329F6A1F7C")]
        [RoleId("EC4607D7-6D12-4DDF-9F62-19856D5308A3")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public PartSpecificationState LastPartSpecificationState { get; set; }

        #region Allors
        [Id("41FAB301-8559-44A2-B47B-7FE00ED1BBCC")]
        [AssociationId("01D25BCA-81E8-4B31-9D2D-53257C59F899")]
        [RoleId("28FC34FA-2F4A-466C-9EA8-05AEBE6DEC33")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public PartSpecificationState PartSpecificationState { get; set; }
        #endregion
        #endregion

        #region Versioning
        #region Allors
        [Id("425C11EE-048F-4CA3-878B-C158BD6CABCC")]
        [AssociationId("E2ADDC60-D372-424B-B7B0-A19B2235FE3E")]
        [RoleId("86327804-156B-4A7E-9DC0-16AC2D58A930")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public PartSpecificationVersion CurrentVersion { get; set; }

        #region Allors
        [Id("2456FCE3-44CD-467E-96EA-BA16E2DE0772")]
        [AssociationId("F14DA392-B739-4B63-951D-B53824F6B81C")]
        [RoleId("81FA98DD-11C5-44F4-9BF3-F43DF0E09501")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public PartSpecificationVersion[] AllVersions { get; set; }
        #endregion

        #region Allors
        [Id("6a83ef4b-1ef5-4782-b9fd-19e3231c29c5")]
        [AssociationId("93f4241d-23ea-46ad-bcaa-fd1f5c909c43")]
        [RoleId("c2b4a79f-c245-40d5-834e-5939c7748462")]
        #endregion
        public DateTime DocumentationDate { get; set; }

        #region Allors
        [Id("e20b0fd5-f10a-44df-8bef-f454e7d23bce")]
        [AssociationId("0c7ad60f-57c9-469b-b8e4-dabeae4398ee")]
        [RoleId("6a208020-712c-4ce8-b69b-ea4523ba2e85")]
        #endregion
        [Required]
        [Size(-1)]
        public string Description { get; set; }

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
        [Id("21279E2E-60A0-4B07-9FB3-D49892001DD2")]

        #endregion

        public void Approve()
        {
        }
    }
}