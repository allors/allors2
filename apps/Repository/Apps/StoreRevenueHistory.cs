namespace Allors.Repository.Domain
{
    using System;

    #region Allors
    [Id("648d1b19-d3f8-4ace-86bc-b113827f5e8e")]
    #endregion
    public partial class StoreRevenueHistory : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("4c44c10c-7577-424a-9361-43d9b264e297")]
        [AssociationId("dbe1fab7-525b-4824-bb9d-959b9e2c8afd")]
        [RoleId("09e6e626-d5e4-4ed1-acce-271cd26ccdcf")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public InternalOrganisation InternalOrganisation { get; set; }
        #region Allors
        [Id("5165e08b-97cc-457b-ba65-6592c31360e5")]
        [AssociationId("67272022-b5fb-4d33-aada-2df814418b64")]
        [RoleId("f15e82a9-563c-4fda-bbb3-ba9c3d542d2c")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public Currency Currency { get; set; }
        #region Allors
        [Id("80a3da61-9df3-4100-b81b-323f293194a7")]
        [AssociationId("189a3f1e-1878-4e85-921d-9ae9d7ce520e")]
        [RoleId("0f864ab4-599f-451f-9088-d7380981a46f")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public Store Store { get; set; }
        #region Allors
        [Id("a23658cb-80aa-4880-ae11-b0584856f1f8")]
        [AssociationId("db5e7707-20cf-49ef-9aaa-473e192e86eb")]
        [RoleId("0a79fe7c-68ac-4484-9054-026fb4dc556c")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        public decimal Revenue { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

    }
}