namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("EB8B32AD-6F37-4E6F-8E20-59E88DA51573")]
    #endregion
    public partial class RequestForProposalVersion : RequestVersion
    {
        #region inherited properties

        public RequestState RequestState { get; set; }

        public string InternalComment { get; set; }
        public string Description { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime RequiredResponseDate { get; set; }
        public RequestItem[] RequestItems { get; set; }
        public string RequestNumber { get; set; }
        public RespondingParty[] RespondingParties { get; set; }
        public Party Originator { get; set; }
        public Currency Currency { get; set; }
        public ContactMechanism FullfillContactMechanism { get; set; }
        public string EmailAddress { get; set; }
        public string TelephoneNumber { get; set; }
        public string TelephoneCountryCode { get; set; }
        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }

        public Guid DerivationId { get; set; }

        public DateTime DerivationTimeStamp { get; set; }
        #endregion

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }
        #endregion
    }
}