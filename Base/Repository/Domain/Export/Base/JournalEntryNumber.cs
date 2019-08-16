namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("c47bf25f-7d16-4dcd-af3b-5e893a1cdd92")]
    #endregion
    public partial class JournalEntryNumber : Object
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("8fd6ce7a-0b08-4af4-9b7f-05a7e12445ed")]
        [AssociationId("0d39f242-de6a-4192-88e7-a78e5ddfcdb1")]
        [RoleId("f5564eaa-202c-43c2-9dda-2e1500f0606d")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public JournalType JournalType { get; set; }
        #region Allors
        [Id("99719445-24e6-445e-8ce1-60c0b5911723")]
        [AssociationId("4d278d9b-a817-4311-ba52-d1bd14db8cc2")]
        [RoleId("2d669167-ac38-4dd1-a846-ba0f1b724bd2")]
        #endregion

        public int Number { get; set; }
        #region Allors
        [Id("a47d5af5-21a8-4d4f-a2be-956ae7da8819")]
        [AssociationId("fd990275-4217-46fd-9f2d-e7af28ff5598")]
        [RoleId("863f988b-ffab-4896-bd0a-02daaabc6fc0")]
        #endregion

        public int Year { get; set; }

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
