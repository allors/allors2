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
        [Id("9dee4a94-26d5-410f-a3e3-3fcde21c5c89")]
        [AssociationId("0322b71b-0389-4393-8b1f-1b3fb12bb7b1")]
        [RoleId("68f80e6a-7ff4-4f07-b2c5-728459c376ae")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Currency PreferredCurrency { get; set; }

        #region Allors
        [Id("d9ea02e5-9aa1-4cbe-9318-06324529a923")]
        [AssociationId("6247e69d-4789-4ee0-a75b-c2de44a5fcce")]
        [RoleId("c11f31e1-75a7-4b23-9d58-7dfec256b658")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public SecurityToken AdministratorSecurityToken { get; set; }

        #region Allors
        [Id("a0fdc553-8081-43fa-ae1a-b9f7767d2d3e")]
        [AssociationId("c36bd0ce-d912-4935-b2e2-5aecc822a524")]
        [RoleId("65e3b040-4191-4f26-a51b-6c2a17ec35c7")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Workspace]
        public Media NoImageAvailableImage { get; set; }

        #region inherited methods

        #endregion
    }
}