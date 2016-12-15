namespace Allors.Repository.Domain
{
    using System;

    #region Allors
    [Id("eab85f26-c3f4-4f47-97dc-8f9429856c00")]
    #endregion
    [Plural("RequestsForInformation")]
    public partial class RequestForInformation : Request 
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