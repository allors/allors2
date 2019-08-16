namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("9ffd634a-27b9-46a5-bf77-4ae25a9b9ebf")]
    #endregion
    public partial class JournalEntryDetail : DelegatedAccessControlledObject
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("9e273a44-b68f-4379-b2cd-f6ac1d524c4a")]
        [AssociationId("003c293d-650f-422b-a5a4-aa8caff4ce3d")]
        [RoleId("a1639744-d44f-472d-821b-ec9eaf5a8530")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public OrganisationGlAccount GeneralLedgerAccount { get; set; }
        #region Allors
        [Id("b51ddcf7-ae36-4fbc-b8b5-3b2befa4a720")]
        [AssociationId("9a4e561c-df1e-4d5f-8b31-384959d56e4f")]
        [RoleId("e49734eb-3232-4fcb-b923-d873804545c9")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        public decimal Amount { get; set; }
        #region Allors
        [Id("bc59e72d-935c-46fd-a595-4de24369fc12")]
        [AssociationId("d157af85-88b6-4a13-b142-ba7d176bbb40")]
        [RoleId("539cedbf-3d80-45b3-8d3c-2e154f541ce9")]
        #endregion

        public bool Debit { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {

        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }
        public void DelegateAccess() { }

        #endregion

    }
}
