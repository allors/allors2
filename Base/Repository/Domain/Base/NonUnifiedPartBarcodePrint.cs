using System;

namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("B02988B9-043C-47C0-99B5-C8149E92D1BA")]
    #endregion
    public partial class NonUnifiedPartBarcodePrint: Printable
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public PrintDocument PrintDocument { get; set; }

        #endregion

        #region Allors
        [Id("E350E850-7FF2-42E1-A253-C7712DE21A9E")]
        [AssociationId("A576DB06-A9E8-4A6C-BBFB-894EBB50043D")]
        [RoleId("00673CA6-78B2-499B-8BFC-195A5DE45928")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        public NonUnifiedPart[] Parts{ get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        public void Print() { }

        #endregion
    }
}