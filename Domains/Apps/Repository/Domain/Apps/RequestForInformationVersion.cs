namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("5716EEEF-0ED3-43F1-92B5-8C6B7DCF6498")]
    #endregion
    public partial class RequestForInformationVersion : IRequestForInformation
    {
        #region inherited properties

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
        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
        public ObjectState PreviousObjectState { get; set; }
        public ObjectState LastObjectState { get; set; }
       #endregion

        #region Allors
        [Id("7B884657-C8E6-4D80-A053-CE43296A51E1")]
        [AssociationId("FBF1C786-B81D-4E76-B5D1-AB70131570B0")]
        [RoleId("B6DBAED2-3AD4-46FE-A137-324709E6374E")]
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
    }
}