namespace Allors.Repository
{
    using System;

    using Attributes;

    public partial class Singleton : Auditable
    {
        #region inherited properties
        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }
        #endregion

        #region Allors
        [Id("D53B80EE-468D-463C-8BBF-725105DBA376")]
        [AssociationId("18C9321B-0846-401D-825F-16BAAE18708D")]
        [RoleId("4541B748-349F-4BEF-86F2-3644423FE0AB")]
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        #endregion
        [Workspace]
        public Settings Settings { get; set; }

        #region Allors
        [Id("d9ea02e5-9aa1-4cbe-9318-06324529a923")]
        [AssociationId("6247e69d-4789-4ee0-a75b-c2de44a5fcce")]
        [RoleId("c11f31e1-75a7-4b23-9d58-7dfec256b658")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public SecurityToken AdministratorSecurityToken { get; set; }

        #region inherited methods

        #endregion
    }
}