namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("360cf15d-c360-4d68-b693-7d1544388169")]
    #endregion
    public partial class Proposal : Quote 
    {
        #region inherited properties
        public DateTime ValidFromDate { get; set; }

        public QuoteTerm[] QuoteTerms { get; set; }

        public Party Issuer { get; set; }

        public DateTime ValidThroughDate { get; set; }

        public string Description { get; set; }

        public Party Receiver { get; set; }

        public decimal Amount { get; set; }

        public DateTime IssueDate { get; set; }

        public QuoteItem[] QuoteItems { get; set; }

        public string QuoteNumber { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public ObjectState PreviousObjectState { get; set; }

        public ObjectState LastObjectState { get; set; }

        #endregion

        #region Allors
        [Id("1C12ABA6-A429-4A3E-987F-D9278F090023")]
        [AssociationId("49E4A43B-A29B-4B10-952A-D2D1C35D1340")]
        [RoleId("DB71D8DA-5123-49E3-A9D7-E5C52826180B")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Required]

        public ProposalObjectState CurrentObjectState { get; set; }

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        #endregion

    }
}