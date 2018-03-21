namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("b397c075-215a-4d5b-b962-ea48540a64fa")]
    #endregion
    public partial class BudgetItem : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("24645d36-9f98-4d08-a7e0-51c1132a110d")]
        [AssociationId("27704145-5c5c-4267-b1aa-27f8a64284bb")]
        [RoleId("ef36805b-89db-4195-9848-234e3adf9ba8")]
        #endregion
        [Required]
        [Size(256)]

        public string Purpose { get; set; }
        #region Allors
        [Id("6b313789-9a6d-47ca-adad-def39af1e11f")]
        [AssociationId("bab74221-37b5-424d-895b-6e79e54fbf0d")]
        [RoleId("013505ff-07c0-4e13-9efe-6347111c2ce8")]
        #endregion
        [Size(256)]

        public string Justification { get; set; }
        #region Allors
        [Id("a4e584cc-7cf6-4590-83e4-a827a7a06624")]
        [AssociationId("76a35c5f-4f20-4520-ba02-f68a0dd61e0d")]
        [RoleId("528620b6-e0a7-424a-8c8a-a9fa9c8ed84c")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]

        public BudgetItem[] Children { get; set; }
        #region Allors
        [Id("cced2368-6a7d-4aea-8112-57dead05f7b4")]
        [AssociationId("3af60015-58d7-4bcb-8776-b209875d44ba")]
        [RoleId("b2badaf8-f9fb-4869-a0ab-d5c8fb9a7f51")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal Amount { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

    }
}