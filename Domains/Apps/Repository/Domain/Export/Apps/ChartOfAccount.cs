namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("6cf4845d-65a0-4957-95e9-f2b5327d6515")]
    #endregion
    [Plural("ChartsOfAccounts")]
    public partial class ChartOfAccounts : UniquelyIdentifiable, AccessControlledObject 
    {
        #region inherited properties
        public Guid UniqueId { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("65f44f44-a613-4cbf-a924-1098c9876f20")]
        [AssociationId("d4bd5e5f-e973-489c-879d-31b0023de770")]
        [RoleId("0f6c3b14-f165-41df-aa8d-f49f53c53e05")]
        #endregion
        [Required]
        [Size(256)]

        public string Name { get; set; }
        #region Allors
        [Id("71d503fb-ebb9-45b3-af62-1b233677adce")]
        [AssociationId("ca0820dd-e0b2-4714-8e2f-f3613dcdbd15")]
        [RoleId("d855adc2-f70e-48d3-a185-957bf27d3d58")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]

        public GeneralLedgerAccount[] GeneralLedgerAccounts { get; set; }

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        #endregion
    }
}