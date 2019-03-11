namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("f4b7ea51-eac4-479b-92e8-5109cfeceb77")]
    #endregion
    public partial class EmailAddress : ElectronicAddress 
    {
        #region inherited properties
        public string ElectronicAddressString { get; set; }

        public string Description { get; set; }

        public ContactMechanism[] FollowTo { get; set; }

        public ContactMechanismType ContactMechanismType { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        #endregion


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnInit()
        {
            
        }

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        public void Delete(){}
        #endregion

    }
}