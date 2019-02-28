namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("7f13c77f-1ef1-446d-928d-1c96f9fc8b05")]
    #endregion
    public partial class OrderKind : UniquelyIdentifiable, AccessControlledObject 
    {
        #region inherited properties
        public Guid UniqueId { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("cb4c5cfa-5c2c-4cdf-898b-9afcd28229c4")]
        [AssociationId("6b9b043f-c629-439d-be92-e825177d8c29")]
        [RoleId("8f85bba3-1eaa-4352-bfd4-68f29ce4f71c")]
        #endregion
        [Required]
        [Size(-1)]

        public string Description { get; set; }
        #region Allors
        [Id("e35c295c-a4a8-4441-af9a-bd2d3e96bab3")]
        [AssociationId("c2158f51-489a-4618-b289-dff18a05afb5")]
        [RoleId("c07c051a-e204-46dd-bac2-1ce957c8c6d9")]
        #endregion
        [Required]

        public bool ScheduleManually { get; set; }


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