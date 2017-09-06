namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("eab85f26-c3f4-4f47-97dc-8f9429856c00")]
    #endregion
    [Plural("RequestsForInformation")]
    public partial class RequestForInformation : Request 
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

        #region inherited methods
        public void Cancel() { }

        public void Reject() { }

        public void Submit() { }

        public void Hold() { }
        public void AddNewRequestItem() { }

        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion
    }
}