namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("7ee3b00b-4e63-4774-b744-3add2c6035ab")]
    #endregion
    [Plural("MailboxAddresses")]
    public partial class MailboxAddress : Object, Address 
    {
        #region inherited properties
        public Place Place { get; set; }

        #endregion

        #region Allors
        [Id("03c9970e-d9d6-427d-83d0-00e0888f5588")]
        [AssociationId("8d565792-4315-44eb-9930-55aa30e8f23a")]
        [RoleId("10b46f89-7f3a-4571-8621-259a2a501dc7")]
        [Size(256)]
        #endregion
        public string PoBox { get; set; }


        #region inherited methods

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnInit()
        {
            
        }

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

    }
}