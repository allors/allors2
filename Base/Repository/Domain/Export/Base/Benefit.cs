namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("8cea6932-d589-4b5b-99b8-ffba33936f8f")]
    #endregion
    public partial class Benefit : Object
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("0c3efe28-a934-467d-a361-293175330b62")]
        [AssociationId("d9d8872d-3b77-48e6-9902-74560a60c3ef")]
        [RoleId("98a07703-261f-4a9a-8c1b-02af1a4a4e0b")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        public decimal EmployerPaidPercentage { get; set; }
        #region Allors
        [Id("6239a2cc-97ce-49cb-b5aa-23e1e9ff7e71")]
        [AssociationId("759c76cd-01ba-4be6-a1d1-3f9f305e69b5")]
        [RoleId("97ebeb12-9ae4-4364-8f85-c4cfd565180b")]
        #endregion
        [Size(-1)]

        public string Description { get; set; }
        #region Allors
        [Id("6e1e0ef1-0e2a-406f-afa4-a6c97657801f")]
        [AssociationId("de7199dd-6a61-41d3-b3dc-847a1a1eb596")]
        [RoleId("f9a3fee7-4b05-4bef-af33-064cac668021")]
        #endregion
        [Required]
        [Size(256)]

        public string Name { get; set; }
        #region Allors
        [Id("89460288-d09e-43f9-960a-86b6c1e2e0be")]
        [AssociationId("97cf596f-bed3-4309-9b88-50be9b82f7a1")]
        [RoleId("4554ecdf-95a8-4d3a-9415-fe397d14831e")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        public decimal AvailableTime { get; set; }


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