namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("b0a42c94-3d4e-47f1-86a2-cf45eeba5f0d")]
    #endregion
    public partial class PositionResponsibility : Commentable, Object
    {
        #region inherited properties
        public string Comment { get; set; }

        public LocalisedText[] LocalisedComments { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("493412a4-c29c-4e1c-9167-6c0c0dca831f")]
        [AssociationId("030fa5c5-e41f-4141-a91e-02b37a20e685")]
        [RoleId("fe87742c-4238-4be0-9f58-70ae3f01c96b")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public Position Position { get; set; }
        #region Allors
        [Id("9c8794b9-2c7b-4afa-86a6-21fb48fc902f")]
        [AssociationId("7613dcb8-0c6f-4c65-96c0-75d2cc9db16e")]
        [RoleId("70d2a311-d09b-406c-89d4-3adbbc0a8fe2")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public Responsibility Responsibility { get; set; }

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
