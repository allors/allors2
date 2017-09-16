namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("F7176B94-6DF0-4CF3-BA96-1DD4A676B28F")]
    #endregion
    public partial class RequestVersion : RequestVersioned
    {
        #region inherited properties

        public RequestStatus[] RequestStatuses { get; set; }
        public RequestStatus CurrentRequestStatus { get; set; }
        public string InternalComment { get; set; }
        public string Description { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime RequiredResponseDate { get; set; }
        public RequestItem[] RequestItems { get; set; }
        public string RequestNumber { get; set; }
        public RespondingParty[] RespondingParties { get; set; }
        public Party Originator { get; set; }
        public Currency Currency { get; set; }
        public RequestObjectState CurrentObjectState { get; set; }
        public ContactMechanism FullfillContactMechanism { get; set; }
        public string EmailAddress { get; set; }
        public string TelephoneNumber { get; set; }
        public string TelephoneCountryCode { get; set; }
       #endregion

        #region Allors
        [Id("6FD0B261-8467-4EC4-935E-7A0259D8A936")]
        [AssociationId("09871520-C7A9-4047-82A0-549E58D2D67E")]
        [RoleId("8F861234-BC50-44A1-BCB2-66C19C55FBB8")]
        #endregion
        [Workspace]
        public DateTime TimeStamp { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }
        #endregion

        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
        public ObjectState PreviousObjectState { get; set; }
        public ObjectState LastObjectState { get; set; }
    }
}