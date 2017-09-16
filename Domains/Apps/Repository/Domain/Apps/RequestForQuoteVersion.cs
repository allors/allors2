namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("068BDB19-B74C-43FC-8D6F-3F03E6F73DEC")]
    #endregion
    public partial class RequestForQuoteVersion : IRequestForQuote
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
        [Id("EC77B142-9A48-40DE-B5DE-E46B67C1CA40")]
        [AssociationId("C7F14C79-5114-4E33-B648-EE75AB7E31EE")]
        [RoleId("28B9D3E2-CB77-42F3-8918-BB671AC59B53")]
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