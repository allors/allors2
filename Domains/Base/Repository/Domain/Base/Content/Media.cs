namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("da5b86a3-4f33-4c0d-965d-f4fbc1179374")]
    #endregion
    public partial class Media : UniquelyIdentifiable, AccessControlledObject, Deletable 
    {
        #region inherited properties
        public Guid UniqueId { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("B74C2159-739A-4F1C-ADA7-C2DCC3CDCF83")]
        [AssociationId("96B21673-F124-4C30-A2F0-DF56D29E03F5")]
        [RoleId("DE0FE224-C40D-469C-BDC5-849A7412EFEC")]
        #endregion
        [Workspace]
        [Indexed]
        [Derived]
        public Guid Revision { get; set; }

        #region Allors
        [Id("67082a51-1502-490b-b8db-537799e550bd")]
        [AssociationId("e8537dcf-1bd7-46c4-a37c-077bee6a78a1")]
        [RoleId("02fe1ce8-c019-4a40-bd6f-b38d2f47a288")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Required]
        public MediaContent MediaContent { get; set; }

        #region Allors
        [Id("18236718-1835-430C-A936-7EC461EEE2CF")]
        [AssociationId("8A79E6C5-4BAE-468D-B57C-C7788D3E21E3")]
        [RoleId("877ABDC8-8915-4640-8871-8CEF7EF69072")]
        #endregion
        [Size(-1)]
        public byte[] InData { get; set; }

        #region Allors
        [Id("79B04065-F13B-43B3-B86E-F3ADBBAAF0C4")]
        [AssociationId("287B7291-39F0-43E5-8770-811940E81BAE")]
        [RoleId("CE17BFC7-5A4E-415A-9AE0-FAE429CEE69C")]
        #endregion
        [Workspace]
        [Size(-1)]
        public string InDataUri { get; set; }

        #region inherited methods
        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}
        
        public void Delete(){}
        #endregion
    }
}