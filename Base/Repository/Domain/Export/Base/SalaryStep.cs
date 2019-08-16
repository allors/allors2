namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("6ebf4c66-dd19-494f-8081-67d7a10a16fc")]
    #endregion
    public partial class SalaryStep : Object
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("162b31b7-78fd-4ec5-95f7-3913be0662e2")]
        [AssociationId("c00111ef-5eb8-4155-a621-fd09d0aa1a6c")]
        [RoleId("2872381c-833b-4dce-83f4-a56bbbd416b3")]
        #endregion
        [Required]

        public DateTime ModifiedDate { get; set; }
        #region Allors
        [Id("7cb593b7-48ac-4049-b78c-1e84bdd2fa3a")]
        [AssociationId("39c58f18-a640-4c5e-9878-2f82ea90bd0a")]
        [RoleId("553fe45b-2c69-432d-9686-c2f049610eaa")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal Amount { get; set; }


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
