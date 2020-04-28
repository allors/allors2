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

        #region Allors
        [Id("0cb73f54-31c8-4f9c-af50-492a90c7e94a")]
        [AssociationId("c33fbef7-389b-4a53-8eba-c798f902a50d")]
        [RoleId("d6ec9aa5-3782-4e80-83fc-b9924322861b")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Locale Locale{ get; set; }

        #region Allors
        [Id("ed177d0e-c542-404a-979d-4c70731d2860")]
        [AssociationId("dc53aec4-e23a-4ad4-919b-6fcd17d538ec")]
        [RoleId("9e93b1ca-241d-4a74-a1d1-2eb6531c1be1")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Facility Facility { get; set; }

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
