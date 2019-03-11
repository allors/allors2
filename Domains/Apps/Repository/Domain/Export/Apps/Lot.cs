namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("d900e278-7add-4e90-8bea-0a65d03f4fa7")]
    #endregion
    public partial class Lot : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("4888a06a-fcf5-42a7-a1c3-721d3abaa755")]
        [AssociationId("0f922c04-b617-4b72-8c22-02f43ac2afb9")]
        [RoleId("46b3ec4d-0463-48eb-8764-8dedf8c48b1a")]
        #endregion
        [Workspace]
        public DateTime ExpirationDate { get; set; }

        #region Allors
        [Id("8680f7e2-c5f1-43af-a127-68ac8404fbf4")]
        [AssociationId("e350d93d-c5ce-496b-a210-c01b4ff82c60")]
        [RoleId("92953ece-133e-4402-ad5c-5357c34bb99e")]
        #endregion
        [Workspace]
        public int Quantity { get; set; }

        #region Allors
        [Id("ca7a3e0f-e036-40ed-9346-0d1dae45c560")]
        [AssociationId("fdb9e9dc-1395-43ed-8234-187f35b8a7ef")]
        [RoleId("03e6a4fc-2336-4761-807f-20c1b5b80af0")]
        #endregion
        [Required]
        [Size(256)]
        [Workspace]
        public string LotNumber { get; set; }

        #region inherited methods


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