namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("0112ddd0-14de-43e2-97d3-981766dd957e")]
    #endregion
    [Plural("RequestsForProposal")]
    public partial class RequestForProposal : Request 
    {
        #region inherited properties
        public string Description { get; set; }

        public DateTime RequestDate { get; set; }

        public DateTime RequiredResponseDate { get; set; }

        public RequestItem[] RequestItems { get; set; }

        public string RequestNumber { get; set; }

        public RespondingParty[] RespondingParties { get; set; }

        public Party Originator { get; set; }

        public string InternalComment { get; set; }

        public Currency Currency { get; set; }

        public RequestStatus[] RequestStatuses { get; set; }

        public RequestObjectState CurrentObjectState { get; set; }

        public RequestStatus CurrentRequestStatus { get; set; }

        public ContactMechanism FullfillContactMechanism { get; set; }
        public string EmailAddress { get; set; }
        public string TelephoneNumber { get; set; }
        public string TelephoneCountryCode { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string Comment { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public Guid UniqueId { get; set; }

        public string PrintContent { get; set; }

        public ObjectState PreviousObjectState { get; set; }

        public ObjectState LastObjectState { get; set; }
        #endregion

        #region Allors
        [Id("30472626-909D-4B7E-A153-B2754D6398E3")]
        #endregion
        public void CreateProposal() { }

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        public void Cancel() {}

        public void Reject() { }

        public void Submit() { }

        public void Hold() { }
        public void AddNewRequestItem() { }

        #endregion
    }
}