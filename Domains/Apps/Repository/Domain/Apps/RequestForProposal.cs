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

        public DateTime RequiredResponseDate { get; set; }

        public RequestItem[] RequestItems { get; set; }

        public string RequestNumber { get; set; }

        public RespondingParty[] RespondingParties { get; set; }

        public Party Originator { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string Comment { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        #endregion


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}



        #endregion

    }
}